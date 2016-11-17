using FlexCel.XlsAdapter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvXlsImport {
    /// <summary>
    /// Base class for classes that implement reading from a data source for import.
    /// </summary>
    public abstract class ImportFile : IDisposable {
        /// <summary>
        /// This should be filled in by the implementing classes constructor.
        /// </summary>
        protected List<string> _fieldNames = new List<string>();

        /// <summary>
        /// The names of the fields in the input data. Blank field names are not included.
        /// </summary>
        public IEnumerable<string> FieldNames => _fieldNames.Where(n => !string.IsNullOrWhiteSpace(n));
        
        /// <summary>
        /// The data read from the input source. This is expected to be returned
        /// using yield return.
        /// </summary>
        /// <returns></returns>
        internal abstract IEnumerable<ImportRecord> GetRecords();

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// There's nothing to dispose of in here. IDisposable is implemented
        /// to make it easy to handle in case an implementing class uses it.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                }
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            Dispose(true);
        }
        #endregion
    }

    /// <summary>
    /// Contains one row of data from the input source.
    /// </summary>
    class ImportRecord {
        private List<string> _fieldNames;

        private object[] _values;

        /// <summary>
        /// Constructs an import record with one row of data/
        /// </summary>
        /// <param name="fieldNames">The names of the fields (same for all rows).</param>
        /// <param name="values">The values (in whatever type they have been read in).</param>
        public ImportRecord(IEnumerable<string> fieldNames, IEnumerable<object> values) {
            _fieldNames = fieldNames.ToList();
            _values = values.ToArray();
        }

        /// <summary>
        /// Gets the value for a field as a string.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public string GetString(string fieldName) {
            int i = _fieldNames.IndexOf(fieldName);
            return i < 0 ? null : _values[i]?.ToString();
        }

        /// <summary>
        /// Gets the value for a field as an object of whatever class was read in.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public object GetValue(string fieldName) {
            int i = _fieldNames.IndexOf(fieldName);
            return i < 0 ? null : _values[i];
        }

        /// <summary>
        /// Gets the value for a field as an object of the specified type. 
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        public object GetValue(string fieldName, Type returnType) {
            return Convert(GetString(fieldName), returnType);
        }

        public static object Convert(object value, Type returnType) {
            var s = value?.ToString();
            if (returnType == typeof(string)) return s;
            if (returnType == typeof(DateTime) || returnType == typeof(DateTime?) && !string.IsNullOrEmpty(s)) {
                return new DateTimeConverter().ConvertFromString(s);
            }
            var conv = TypeDescriptor.GetConverter(returnType);
            return conv.IsValid(value) ? conv.ConvertFrom(value) : null;
        }
    }

    /// <summary>
    /// Implements reading from an Excel spreadsheet (the first in the workbook).
    /// </summary>
    public class XlsImportFile : ImportFile {
        private XlsFile _xls;

        /// <summary>
        /// Constructs a reader for an Excel spreadsheet.
        /// </summary>
        /// <param name="filename"></param>
        public XlsImportFile(string filename) {
            _xls = new XlsFile(filename);
            // Get the field names from the first row.
            for (int c = 1; c <= _xls.ColCount; c++) {
                _fieldNames.Add(_xls.GetCellValue(1, c)?.ToString());
            }
        }

        internal override IEnumerable<ImportRecord> GetRecords() {
            for (int r = 2; r <= _xls.RowCount; r++) {
                yield return getRecord(r);
            }
        }

        private ImportRecord getRecord(int r) {
            List<object> vals = new List<object>();
            for (int c = 1; c <= _xls.ColCount; c++) {
                object v = _xls.GetCellValue(r, c);
                vals.Add(v);
            }
            return new ImportRecord(_fieldNames, vals);
        }

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
            if (disposing) {
                _xls = null;
            }
        }
    }

    /// <summary>
    /// Implements reading from a csv file.
    /// </summary>
    public class CsvImportFile : ImportFile {
        private string _filename;
        private StreamReader _reader;

        public CsvImportFile(string filename) {
            _filename = filename;
            // Read the field names.
            using (var reader = new StreamReader(filename)) {
                _fieldNames = reader.GetCsvFieldNames().ToList();
            }
            _reader = new StreamReader(filename);
        }

        internal override IEnumerable<ImportRecord> GetRecords() {
            return _reader.FromCsv<ReadModel>().Select(m => m.GetData());
        }

        /// <summary>
        /// This is used to read rows of data from the csv file and automatically matching up the values
        /// with the field names. The CsvData attribute tells the FromCsv extension method to put all of the row
        /// data into the Fields property.
        /// </summary>
        private class ReadModel {
            [CsvData]
            public IEnumerable<CsvField> Fields { get; set; }

            public ImportRecord GetData() {
                return new ImportRecord(Fields.Select(f => f.Name), Fields.Select(f => f.Value));
            }
        }

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
            if (disposing) _reader.Dispose();
        }
    }
}
