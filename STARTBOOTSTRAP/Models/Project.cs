using STARTBOOTSTRAP.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace STARTBOOTSTRAP.Models
{
    public class Project:BaseEntity
    {
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "only letters can be used")]
        public string Name { get; set; }
        public string Image { get; set; }

        //relational
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
