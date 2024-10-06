namespace Task_Manager.Models
{
    public class DepartmentEmployee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Employee> Employees { get; set; }

    }
}
