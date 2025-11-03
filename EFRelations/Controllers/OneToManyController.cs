using EFRelations.Data;
using EFRelations.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFRelations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OneToManyController(AppDbContext context) : ControllerBase
    {

        [HttpPost("add-blog")]
        public async Task<IActionResult> CreateBlog(Blog blog)
        {
            context.Blogs.Add(blog);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("get-blogs")]
        public async Task<IActionResult> GetBlogs()
        {
            return Ok(await context.Blogs.Include(x => x.Posts).ToListAsync());
        }

        [HttpPost("add-post")]
        public async Task<IActionResult> CreatePost(Post post)
        {
            context.Posts.Add(post);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("get-posts")]
        public async Task<IActionResult> GetPosts()
        {
            return Ok(await context.Posts.Include(x => x.Blog).ToListAsync());
        }
    }
}
