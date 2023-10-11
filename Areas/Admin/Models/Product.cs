using System.ComponentModel.DataAnnotations;

namespace Clothing_boutique_web.Areas.Admin.Models
{
    public partial class Product
    {
        public Product()
        {
            Photos = new HashSet<Photo>();
        }
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Detail { get; set; }
        public double? Price { get; set; }

        public int CategoryId { get; set; }
        public int Quaity { get; set; }
        public bool Featured { get; set; }
        public DateTime DateofInsert { get; set; }


        public virtual Category Categories { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }


    }
}
