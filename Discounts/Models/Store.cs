using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Discounts.Models
{
    public class Store
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ImageId { get; set; }
        public int Id { get; set; }
        public string File { get; set; }
        public int BrandId { get; set; }
        public string UserId { get; set; }
    }
}