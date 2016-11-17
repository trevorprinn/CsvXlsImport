using CsvXlsImport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvXlsImportUnitTests {
    [TestClass]
    public class BasicXlsTests {
        [TestMethod]
        public void XlsImport1() {
            List<Import1Model> data;
            using (var ifile = new XlsImportFile("Data\\XlsImport1.xlsx")) {
                var imp = new Importer<Import1Model>(ifile);
                data = imp.Import().ToList();
            }

            var expectedData = new Import1Model[] {
                new Import1Model { SVal = "abcde", IVal = 1, FVal = 1.5f, DVal = 2.0000001d, DecVal = 3.4m },
                new Import1Model { SVal = "xyz,abc", IVal = 2, FVal = 3, DVal = 2, DecVal = 5 },
                new Import1Model { SVal = "", IVal = 0, FVal = 0, DVal = 0, DecVal = 0 },
                new Import1Model { SVal = "a\r\nb", IVal = 0, FVal = 1, DVal = 2, DecVal = 3 }
            }.ToList();
            CollectionAssert.AreEqual(expectedData, data);
        }

        [TestMethod]
        public void XlsImport2() {
            List<Import2Model> data;
            using (var ifile = new CsvImportFile("Data\\CsvImport1.csv")) {
                var imp = new Importer<Import2Model>(ifile);
                data = imp.Import().ToList();
            }

            var expectedData = new Import2Model[] {
                new Import2Model { SVal = "abcde", IVal = 1, FVal = 1.5f, DVal = 2.0000001d, DecVal = 3.4m },
                new Import2Model { SVal = "xyz,abc", IVal = 2, FVal = 3, DVal = 2, DecVal = 5 },
                new Import2Model { SVal = null, IVal = null, FVal = null, DVal = null, DecVal = null },
                new Import2Model { SVal = "a\r\nb", IVal = null, FVal = 1, DVal = 2, DecVal = 3 }
            }.ToList();
            CollectionAssert.AreEqual(expectedData, data);
        }
    }
}
