using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsvXlsImport {
    /// <summary>
    /// Manages importing data from a file into a collection of model objects. T is the class of the
    /// model objects created.
    /// 
    /// This class can be used directly, but for some models it might be worth deriving a class
    /// from it. In particular, if it is used to import directly into an entity framework DbSet
    /// and records are to be updated, rather than only added, it is necessary to override the ExistingItem
    /// method.
    /// 
    /// Two sources are provided to import from, csv and xls (first sheet only at present). In both cases,
    /// the first row must contain column headings. Other sources can be defined by creating a new ImportFile
    /// class and passing in an instance in the constructor.
    /// </summary>
    /// <typeparam name="T">The type of the objects to be created from the imported data.</typeparam>
    public class Importer<T> where T : class, new() {

        // The source data.
        private ImportFile _importFile;

        /// <summary>
        /// The fields in the source data. One is created automatically for each column.
        /// </summary>
        public List<ImportField<T>> ImportFields { get; }

        /// <summary>
        /// The properties of the model that data can be copied into. Properties can be
        /// set on these after the Importer has been constructed if necessary.
        /// </summary>
        public List<ImportTargetField<T>> TargetFields { get; }

        /// <summary>
        /// Constructs an Importer object, and sets up the Target and Import fields.
        /// </summary>
        /// <param name="importFile">The source data</param>
        public Importer(ImportFile importFile) {
            _importFile = importFile;
            TargetFields = GetTargetFields().ToList();
            ImportFields = ImportField<T>.Create(importFile).ToList();

            // If importing directly into a DbSet, this attempts to determine the primary key property
            // by looking for a property with the Key attribute. It can alternatively be set manually
            // in the ImportTargetField object.
            var keyField = TargetFields.FirstOrDefault(f => f.Prop.CustomAttributes.Any(a => a.AttributeType == typeof(KeyAttribute)));
            if (keyField != null) keyField.IsKeyField = true;

            // Tries to guess which import fields match which target properties.
            Guess();
        }

        /// <summary>
        /// Given a filename, creates a suitable ImportFile object and Importer.
        /// </summary>
        /// <param name="importFilename"></param>
        /// <returns>An Importer, or null if the type of data in the source could not be determined.</returns>
        public static Importer<T> Create(string importFilename) {
            var ext = Path.GetExtension(importFilename).TrimStart('.').ToLower();
            ImportFile impFile
                = ext == "csv" ? (ImportFile)new CsvImportFile(importFilename)
                : ext.StartsWith("xl") ? new XlsImportFile(importFilename)
                : null;
            return impFile != null ? new Importer<T>(impFile) : null;
        }

        /// <summary>
        /// Tries to set up the Target property for each import field by matching up the names of both.
        /// This is only an initial attempt - they can be changed in code afterwards and the user 
        /// override the matchings if DisplaySelectionForm is called.
        /// </summary>
        protected virtual void Guess() {
            foreach (var impf in ImportFields) {
                var t = TargetFields.FirstOrDefault(f => f.Name.ToLower() == impf.FieldName.Replace(" ", "").ToLower());
                if (t != null) impf.ImportTarget = t;
            }
        }

        /// <summary>
        /// Determines the potential Target properties in the model. This can be overridden to specify them explicitly (maybe
        /// to remove some, or set some properties of the target). By default, it includes all properties with a set
        /// method that are not marked with the IgnoreForImport attribute.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<ImportTargetField<T>> GetTargetFields() =>
            typeof(T).GetProperties().Where(p => p.SetMethod != null && !p.CustomAttributes.Any(a => a.AttributeType == typeof(IgnoreForImportAttribute)))
            .Select(p => new ImportTargetField<T>(p));

        /// <summary>
        /// Displays a dialogue that allows the user to drag and drop target properties onto their corresponding source
        /// fields.
        /// </summary>
        /// <param name="owner"></param>
        /// <returns>False if the user Cancels the dialogue.</returns>
        public virtual bool DisplaySelectionForm(IWin32Window owner) {
            using (var f = new FormImportFields<T>(this)) {
                return f.ShowDialog(owner) != DialogResult.Cancel;
            }
        }

        /// <summary>
        /// Imports data from the source.
        /// </summary>
        /// <returns>A model object for each row in the source.</returns>
        public IEnumerable<T> Import() {
            int rowNbr = 1;
            foreach (var row in _importFile.GetRecords()) {
                T model = new T();
                foreach (var impFld in ImportFields) {
                    impFld.Import(model, row, rowNbr);
                }
                rowNbr++;
                yield return model;
            }
        }

        /// <summary>
        /// When importing into a DbSet, determines whether a record already exists on the
        /// database, and needs to be updated rather than imported.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="check">The model object to be checked. Only the primary key property will be filled in.</param>
        /// <returns>Null if the record does not already exist, otherwise the existing record.</returns>
        protected virtual T ExistingItem(DbSet<T> table, T check) => null;

        /// <summary>
        /// Can be overridden to not exclude records from the import - for example, if necessary fields
        /// for that record have no data in them.
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        protected virtual bool Include(T check) => true;

        /// <summary>
        /// Imports directly into a DbSet. ExistingItem should be overridden if records can
        /// be updated, otherwise records are added.
        /// </summary>
        /// <param name="table"></param>
        public void Import(DbSet<T> table) {
            var newRecs = new List<T>();
            var usedFields = ImportFields.Where(f => f.ImportTarget != null && !f.ImportTarget.IsKeyField && f.ImportTarget.Prop != null).ToList();
            var keyField = ImportFields.FirstOrDefault(f => f.ImportTarget != null && f.ImportTarget.IsKeyField && f.ImportTarget.Prop != null);
            int rowNbr = 1;
            foreach (var row in _importFile.GetRecords()) {
                T rec = new T();
                T exist = null;
                if (keyField != null) {
                    keyField.Import(rec, row, rowNbr);
                    exist = ExistingItem(table, rec);
                    if (exist != null) rec = exist;
                }
                foreach (var f in usedFields) f.Import(rec, row, rowNbr);
                if (exist == null && Include(rec)) newRecs.Add(rec);
                rowNbr++;
            }
            table.AddRange(newRecs);
        }
    }

    /// <summary>
    /// Marks a property in a model as not to be imported into.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreForImportAttribute : Attribute { }

    /// <summary>
    /// A field in the input data that is being imported from.
    /// </summary>
    /// <typeparam name="T">The type of object being imported into (the model)</typeparam>
    public class ImportField<T> {
        /// <summary>
        /// The name of the field in the input data (from the column header).
        /// </summary>
        public string FieldName { get; }

        /// <summary>
        /// The property in the model that data for this field will be copied into.
        /// This is normally set either by the Guess routine in the Importer,
        /// or by the import dialogue (FormImportFields).
        /// </summary>
        public ImportTargetField<T> ImportTarget { get; set; }

        /// <summary>
        /// This can be set to process data before putting on pulling it out of the source.
        /// </summary>
        public Func<object, object> ConvertFunc { get; set; }

        public ImportField(string fieldName) {
            FieldName = fieldName;
        }

        /// <summary>
        /// The property the data will be copied into.
        /// </summary>
        public PropertyInfo TargetProperty => ImportTarget?.Prop;

        /// <summary>
        /// The name of the target property.
        /// </summary>
        public string TargetName => ImportTarget?.Name;

        /// <summary>
        /// Copies the data for this field from one row of the input into the target object.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="row"></param>
        /// <param name="rowNbr"></param>
        internal void Import(T target, ImportRecord row, int rowNbr) {
            if (TargetProperty == null) return;
            try {
                object val;
                if (ConvertFunc == null) {
                    val = row.GetValue(FieldName, TargetProperty.PropertyType);
                } else { 
                    val = ConvertFunc(row.GetValue(FieldName));
                    val = ImportRecord.Convert(val, TargetProperty.PropertyType);
                }
                if (ImportTarget.ConvertFunc != null) val = ImportTarget.ConvertFunc(val);
                TargetProperty.SetValue(target, val);
            } catch (Exception ex) {
                throw new ImportException($"There was a problem importing the data for {FieldName} at row {rowNbr}", ex);
            }
        }

        /// <summary>
        /// Creates a set of ImportField objects from the input source.
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static IEnumerable<ImportField<T>> Create(ImportFile inputData) {
            return inputData.FieldNames.Select(f => new ImportField<T>(f));
        }
    }

    /// <summary>
    /// A property on the model that is being imported into.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ImportTargetField<T> {
        /// <summary>
        /// The name of the property. This can be modified to something friendlier after creating the Importer if necessary.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The property within the model.
        /// </summary>
        public PropertyInfo Prop { get; private set; }

        /// <summary>
        /// True if this field is the primary key of the DbSet. Normally set automatically when the Importer is created.
        /// </summary>
        public bool IsKeyField { get; set; }

        /// <summary>
        /// This can be set to process data before putting into the property.
        /// </summary>
        public Func<object, object> ConvertFunc { get; set; }

        /// <summary>
        /// Constructs a target field object.
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="displayName"></param>
        public ImportTargetField(PropertyInfo prop, string displayName = null) {
            Prop = prop;
            Name = displayName ?? prop.Name;
        }

        private ImportTargetField() {
            Name = "<Ignore>";
        }

        public override string ToString() {
            return Name;
        }

        /// <summary>
        /// Returns an empty property with the name Ignore. This is used in FormImportFields
        /// to allow the user to not import into a property the Importer has guessed should be copied into.
        /// </summary>
        public static ImportTargetField<T> IgnoreField => new ImportTargetField<T>();
    }


    public class ImportException : ApplicationException {
        public ImportException() { }

        public ImportException(string message, Exception innerException) : base(message, innerException) { }
    }
}
