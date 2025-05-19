using STARTBOOTSTRAP.Models;

namespace STARTBOOTSTRAP.ViewModels
{
    public class CreateEmployeeVM
    {
        public string Name { get; set; }
        public IFormFile Photo { get; set; }
        public string X { get; set; }
        public string Facebook { get; set; }
        public string Linkedin { get; set; }

        //relational
        public int PositionId { get; set; }
        public List<Position>? Positions { get; set; }
    }
}
