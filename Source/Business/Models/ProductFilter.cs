using System.Collections.Generic;

namespace Business.Models
{
    public class ProductFilter
    {
        public float MinimumPrice { get; set; }
        public float MaximumPrice { get; set; }
        public IEnumerable<string> Sizes { get; set; } = new List<string>();
        public IEnumerable<string> CommonWords { get; set; } = new List<string>();
    }
}