using EFRelations.Data;
using EFRelations.DTOs;
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
        public async Task<IActionResult> CreateBlog(BlogDto blogDto)
        {
            //context.Blogs.Add(blog);
            //await context.SaveChangesAsync();
            //return Ok();

            if (blogDto == null)
                return BadRequest("Invalid Data");

            var blog = new Blog
            {
                Title = blogDto.Title
              
            };

            context.Blogs.Add(blog);
            await context.SaveChangesAsync();

            return Ok();
        }



        [HttpGet("get-blogs")]
        public async Task<IActionResult> GetBlogs()
        {
            //return Ok(await context.Blogs.Include(x => x.Posts).ToListAsync());

            //update it using the mapping DTO

            var blogs = await context.Blogs.Include(b => b.Posts).Select(b => new BlogDto
            {
                Id = b.Id,
                Title = b.Title,
                Post = b.Posts.Select(p => new PostDto
                {
                    Id = p.Id,
                    Content = p.Content,
                    BlogId = p.BlogId
                }).ToList()
            })
            .ToListAsync();

            return Ok(blogs);
        }

        [HttpPost("add-post")]
        public async Task<IActionResult> CreatePost(PostDto postDto)
        {
            //context.Posts.Add(post);
            //await context.SaveChangesAsync();
            //return Ok();

            var blog = await context.Blogs.FindAsync(postDto.BlogId);
            if (blog == null)
                return BadRequest("Blog not found");

            var post = new Post
            {
                Content = postDto.Content,
                BlogId = postDto.BlogId
            };

            context.Posts.Add(post);
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("get-posts")]
        public async Task<IActionResult> GetPosts()
        {
            //return Ok(await context.Posts.Include(x => x.Blog).ToListAsync());

            var post = await context.Posts.Include(p => p.Blog).Select(p => new PostDto
            {
                Id = p.Id,
                Content = p.Content,
                BlogId = p.BlogId
            }).ToListAsync();

            return Ok(post);
        }
    }
}
