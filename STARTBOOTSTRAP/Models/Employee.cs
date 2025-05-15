using STARTBOOTSTRAP.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace STARTBOOTSTRAP.Models
{
    public class Employee:BaseEntity
    {
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "only letters can be used")]
        public string Name {  get; set; }
        public string Image { get; set; }
        public string X { get; set; }
        public string Facebook { get; set; }
        public string Linkedin { get; set; }
        
        //relational
        public int PositionId { get; set; }
        public Position Position { get; set; }
    }
}
