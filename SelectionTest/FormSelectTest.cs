using CsvXlsImport;
using SelectionTest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SelectionTest {
    public partial class FormSelectTest : Form {
        public FormSelectTest() {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e) {
            using (var impFile = sender == btnOpenXls ? (ImportFile)new XlsImportFile("Data\\PriceData.xlsx") : new CsvImportFile("Data\\PriceData.csv")) {
                var importer = new Importer<PriceModel>(impFile);
                if (importer.DisplaySelectionForm(this)) {
                    gridData.DataSource = importer.Import().ToList();
                }
            }
        }
    }
}
