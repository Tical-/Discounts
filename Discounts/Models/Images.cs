//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Discounts.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Images
    {
        public int Id { get; set; }
        public System.Guid Guid { get; set; }
        public string Extension { get; set; }
        public Nullable<int> BrandId { get; set; }
        public Nullable<int> StoreId { get; set; }
        public Nullable<int> ProductId { get; set; }
    
        public virtual Brands Brands { get; set; }
        public virtual Products Products { get; set; }
        public virtual Stores Stores { get; set; }
    }
}
