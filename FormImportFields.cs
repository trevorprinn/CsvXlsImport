#region Licence
/*
The MIT License (MIT)

Copyright (c) 2016 Babbacombe Computers Ltd

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion
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

            showSample();
        }

        private void showSample() {
            gridData.DataSource = _importer.GetSample().ToList();
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

            showSample();
        }
    }
}
