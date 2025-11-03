namespace EFRelations.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string? name { get; set; }
        public ICollection<CourseStudent>? CourseStudents { get; set; }
    }
}
