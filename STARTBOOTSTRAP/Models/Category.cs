using STARTBOOTSTRAP.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace STARTBOOTSTRAP.Models
{
    public class Category:BaseEntity
    {
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "only letters can be used")]
        public string Name { get; set; }

        //relational
        public List<Project> Projects { get; set; }
    }
}
