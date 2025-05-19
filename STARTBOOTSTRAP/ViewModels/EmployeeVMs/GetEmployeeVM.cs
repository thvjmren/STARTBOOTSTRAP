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
        public string PositionName { get; set; }
        public string Image { get; set; }

    }
}
