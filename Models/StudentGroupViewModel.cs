using Microsoft.AspNetCore.Mvc.Rendering;

namespace CollegeWebApplication.Models
{
    public class StudentGroupViewModel
    {
        public List<Student>? Students { get; set; }
        public SelectList? Groups { get; set; }
        public string? StudentGroup {  get; set; }
        public string? SearchString { get; set; }
    }
}
