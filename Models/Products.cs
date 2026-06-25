using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mini_store.Models
{
    public class Product
    {
        [Key]
        public int Id {get ; set;}

        public string Name { get; set; }

        public Decimal Price { get; set; }
        
        public string Images { get; set; }


        public int CategoryId {get ; set ;}  // Foreign Key 
     
     

       [ForeignKey("CategoryId")]
       public virtual Categories Categories{get;set;}

       public virtual ICollection<Image> Image {get;set;}=new List<Image>();

    }


    
}