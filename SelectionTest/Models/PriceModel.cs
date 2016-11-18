using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectionTest.Models {
    class PriceModel {
        public string Sku { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime ActiveDate { get; set; }
    }
}
