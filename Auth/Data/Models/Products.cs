using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class Products
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
