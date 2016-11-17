using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using CsvXlsImport;
using System.Linq;

namespace CsvXlsImportUnitTests {
    [TestClass]
    public class BasicCsvTests {
        [TestMethod]
        public void CsvImport1() {
            List<CsvImport1Model> data;
            using (var ifile = new CsvImportFile("Data\\CsvImport1.csv")) {
                var imp = new Importer<CsvImport1Model>(ifile);
                data = imp.Import().ToList();
            }

            var expectedData = new CsvImport1Model[] {
                new CsvImport1Model { SVal = "abcde", IVal = 1, FVal = 1.5f, DVal = 2.0000001d, DecVal = 3.4m },
                new CsvImport1Model { SVal = "xyz,abc", IVal = 2, FVal = 3, DVal = 2, DecVal = 5 },
                new CsvImport1Model { SVal = "", IVal = 0, FVal = 0, DVal = 0, DecVal = 0 },
                new CsvImport1Model { SVal = "a\r\nb", IVal = 0, FVal = 1, DVal = 2, DecVal = 3 }
            }.ToList();
            CollectionAssert.AreEqual(expectedData, data);
        }

        [TestMethod]
        public void CsvImport2() {
            List<CsvImport2Model> data;
            using (var ifile = new CsvImportFile("Data\\CsvImport1.csv")) {
                var imp = new Importer<CsvImport2Model>(ifile);
                data = imp.Import().ToList();
            }

            var expectedData = new CsvImport2Model[] {
                new CsvImport2Model { SVal = "abcde", IVal = 1, FVal = 1.5f, DVal = 2.0000001d, DecVal = 3.4m },
                new CsvImport2Model { SVal = "xyz,abc", IVal = 2, FVal = 3, DVal = 2, DecVal = 5 },
                new CsvImport2Model { SVal = "", IVal = null, FVal = null, DVal = null, DecVal = null },
                new CsvImport2Model { SVal = "a\r\nb", IVal = null, FVal = 1, DVal = 2, DecVal = 3 }
            }.ToList();
            CollectionAssert.AreEqual(expectedData, data);
        }

        class CsvImport1Model {
            public string SVal { get; set; }
            public int IVal { get; set; }
            public float FVal { get; set; }
            public double DVal { get; set; }
            public decimal DecVal { get; set; }

            public override bool Equals(object obj) {
                return obj is CsvImport1Model ? Equals((CsvImport1Model)obj) : base.Equals(obj);
            }

            public bool Equals(CsvImport1Model other) {
                return SVal == other.SVal && IVal == other.IVal && FVal == other.FVal && DVal == other.DVal && DecVal == other.DecVal;
            }

            public override int GetHashCode() {
                return base.GetHashCode();
            }
        }

        class CsvImport2Model {
            public string SVal { get; set; }
            public int? IVal { get; set; }
            public float? FVal { get; set; }
            public double? DVal { get; set; }
            public decimal? DecVal { get; set; }

            public override bool Equals(object obj) {
                return obj is CsvImport2Model ? Equals((CsvImport2Model)obj) : base.Equals(obj);
            }

            public bool Equals(CsvImport2Model other) {
                return SVal == other.SVal && IVal == other.IVal && FVal == other.FVal && DVal == other.DVal && DecVal == other.DecVal;
            }

            public override int GetHashCode() {
                return base.GetHashCode();
            }
        }



    }
}
