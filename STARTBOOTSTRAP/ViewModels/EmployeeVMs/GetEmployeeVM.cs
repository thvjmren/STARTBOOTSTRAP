
using STARTBOOTSTRAP.Models;

namespace STARTBOOTSTRAP.ViewModels
{
    public class GetEmployeeVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string X { get; set; }
        public string Facebook { get; set; }
        public string Linkedin { get; set; }

        //relational
        public int? PositionId { get; set; }
        public IFormFile Photo { get; set; }
        public List<Position> Positions { get; set; }
        
    }
}
