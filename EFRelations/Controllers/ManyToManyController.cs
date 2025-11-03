using EFRelations.Data;
using EFRelations.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFRelations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ManyToManyController(AppDbContext context) : ControllerBase
    {

        [HttpPost("add-student")]
        public async Task<IActionResult> CreateStudent(Student student)
        {
            context.Students.Add(student);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("get-students")]
        public async Task<IActionResult> GetStudents()
        {
            return Ok(await context.Students.Include(x => x.CourseStudents).ToListAsync());
        }

        [HttpPost("add-course")]
        public async Task<IActionResult> CreateCourse(Course course)
        {
            if (course == null) { 
            return BadRequest("You have to write the course details");
            }
            context.Add(course);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("get-courses")]
        public async Task<IActionResult> GetCourses()
        {
            return Ok(await context.Courses.Include(x => x.CourseStudents).ToListAsync());
        }

        [HttpPost("add-course-student")]
        public async Task<IActionResult> CreateCourseStudent(CourseStudent courseStudent)
        {
            context.CourseStudents.Add(courseStudent);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("get-course-students")]
        public async Task<IActionResult> GetCourseStudents()
        {
            return Ok(await context.CourseStudents
                .Include(x => x.Student)
                .Include(x => x.Course)
                .ToListAsync());
        }
    }
}
