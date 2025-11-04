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

    //we are doing the relations with navigation and conversion property
    public class OneToOneController(AppDbContext context) : ControllerBase
    {
        [HttpPost("add-user")]
        public async Task<IActionResult> CreateUser(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return Ok();
        }


        [HttpGet("get-user")]
        public async Task<IActionResult> GetUser()
        {
            //return Ok(await context.Users.Include(x => x.Profile).ToListAsync()); //fixing this line with DTO

            //return Ok(await context.Users.Include(x => x.Profile).Select(x =>  new UserDto
            //{
            //    Id = x.Id,
            //    Username = x.Username,
            //    Profile = x.Profile == null ? null : new ProfileDto
            //    {
            //        Id = x.Profile.Id,
            //        Bio = x.Profile.Bio,
            //        Username = x.Username
            //    }
            //})
            //.ToListAsync());

            var user = await context.Users.Include(x => x.Profile).Select(x => MapUser(x)).ToListAsync();

            return Ok(user);
        }

        [HttpGet("get-user/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            if(id == null)
            {
                return BadRequest("The id is not defined properly");
            }
            //var user = await context.Users.Include(x => x.Profile).Where(x => x.Id == id).Select(x => new UserDto
            //{
            //    Id = x.Id,
            //    Username = x.Username,
            //    Profile = x.Profile == null ? null : new ProfileDto
            //    {
            //        Id = x.Profile.Id,
            //        Bio = x.Profile.Bio,
            //        Username = x.Username
            //    }

            //}).FirstOrDefaultAsync();

            //we can do another this and make this code much better (b/c is been used by 2 request getuser and getuserbyid

            var user = await context.Users.Include(x => x.Profile)
                .Where(x => x.Id == id)
                .Select(x => MapUser(x))
                .FirstOrDefaultAsync();

            return Ok(user);
        }

        private static UserDto MapUser(User x)
        {
            return new UserDto
            {
                Id = x.Id,
                Username = x.Username,
                Profile = x.Profile == null ? null : new ProfileDto
                {
                    Id = x.Profile.Id,
                    Bio = x.Profile.Bio,
                    Username = x.Username

                }
            };
        }

        [HttpPost("add-profile")]
        public async Task<IActionResult> CreateProfile(Profile profile)
        {
            context.Profiles.Add(profile);
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("get-profiles")]
        public async Task<IActionResult> GetProfile()
        {
            return Ok(await context.Profiles.Include(x => x.User).ToListAsync());
        }

    }
}
