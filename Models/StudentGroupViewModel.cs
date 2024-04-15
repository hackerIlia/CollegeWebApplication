using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace CollegeWebApplication.Models
{
    public class StudentGroupViewModel
    {
        public IPagedList<Student>? Students { get; set; }
        public SelectList? Groups { get; set; }
        public string? StudentGroup {  get; set; }
        public string? SearchString { get; set; }
        public int? Page {  get; set; }
        public int? PageSize { get; set; }
    }
}
