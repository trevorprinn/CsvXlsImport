using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsvXlsImport {
    partial class FormImportFields<T> : Form where T : class, new() {
        private Importer<T> _importer;

        public FormImportFields(Importer<T> importer) {
            InitializeComponent();

            _importer = importer;
            gridFields.AutoGenerateColumns = false;
            gridFields.DataSource = importer.ImportFields.ToList();
            listTargets.Items.Add(ImportTargetField<T>.IgnoreField);
            listTargets.Items.AddRange(importer.TargetFields.ToArray());
        }

        private void listTargets_MouseDown(object sender, MouseEventArgs e) {
            var ix = listTargets.IndexFromPoint(e.X, e.Y);
            if (ix < 0) return;
            listTargets.DoDragDrop(listTargets.Items[ix], DragDropEffects.Copy);
        }

        private void gridFields_DragOver(object sender, DragEventArgs e) {
            var pt = gridFields.PointToClient(new Point(e.X, e.Y));
            var rowIx = gridFields.HitTest(pt.X, pt.Y).RowIndex;
            e.Effect = rowIx >= 0 ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void gridFields_DragDrop(object sender, DragEventArgs e) {
            var target = e.Data.GetData(typeof(ImportTargetField<T>)) as ImportTargetField<T>;
            if (target == null) return;
            var pt = gridFields.PointToClient(new Point(e.X, e.Y));
            var rowIx = gridFields.HitTest(pt.X, pt.Y).RowIndex;
            if (rowIx < 0) return;
            var field = (ImportField<T>)gridFields.Rows[rowIx].DataBoundItem;
            field.ImportTarget = target;
            gridFields.DataSource = _importer.ImportFields.ToList();
        }
    }
}
