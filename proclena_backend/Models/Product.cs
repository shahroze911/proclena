//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace proclena_backend.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.ProductProperties = new HashSet<ProductProperty>();
        }
    
        public int ProductID { get; set; }
        [Required(ErrorMessage = "Product Name is required.")]
        [StringLength(100, ErrorMessage = "Product Name cannot be more than 100 characters.")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Product Category is required.")]
        public string ProductCategory { get; set; }
        [Required(ErrorMessage = "Product Image is required.")]
        public byte[] ProductImage { get; set; }
        [Required(ErrorMessage = "Product Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Product Price must be a positive value.")]
        public decimal ProductPrice { get; set; }
        [Required(ErrorMessage = "Product Description is required.")]
        public string ProductDescription { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductProperty> ProductProperties { get; set; }
    }
}