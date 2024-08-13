using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clothing_boutique_web.Areas.Admin.Models
{
    public partial class Photo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Featured { get; set; }
        public bool Status { get; set; }
        public int ProductId { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }

        public virtual Product Product { get; set; }

    }
}
