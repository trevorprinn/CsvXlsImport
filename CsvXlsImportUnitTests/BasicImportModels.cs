using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvXlsImportUnitTests {
    class Import1Model {
        public string SVal { get; set; }
        public int IVal { get; set; }
        public float FVal { get; set; }
        public double DVal { get; set; }
        public decimal DecVal { get; set; }

        public override bool Equals(object obj) {
            return obj is Import1Model ? Equals((Import1Model)obj) : base.Equals(obj);
        }

        public bool Equals(Import1Model other) {
            return ((string.IsNullOrEmpty(SVal) && string.IsNullOrEmpty(other.SVal)) || SVal == other.SVal) && IVal == other.IVal && FVal == other.FVal && DVal == other.DVal && DecVal == other.DecVal;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }

    class Import2Model {
        public string SVal { get; set; }
        public int? IVal { get; set; }
        public float? FVal { get; set; }
        public double? DVal { get; set; }
        public decimal? DecVal { get; set; }

        public override bool Equals(object obj) {
            return obj is Import2Model ? Equals((Import2Model)obj) : base.Equals(obj);
        }

        public bool Equals(Import2Model other) {
            if (string.IsNullOrEmpty(SVal) && string.IsNullOrEmpty(other.SVal)) return true;
            return ((string.IsNullOrEmpty(SVal) && string.IsNullOrEmpty(other.SVal)) || SVal == other.SVal) && IVal == other.IVal && FVal == other.FVal && DVal == other.DVal && DecVal == other.DecVal;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
