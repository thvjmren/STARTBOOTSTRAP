using STARTBOOTSTRAP.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace STARTBOOTSTRAP.ViewModels
{
    public class HomeVM
    {
        public List<Project> Projects { get; set; }
        public List<Category> Categories { get; set; }
        public List<Position> Positions { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
