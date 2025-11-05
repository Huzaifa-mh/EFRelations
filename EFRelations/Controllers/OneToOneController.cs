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
        //optimize add-user using DTO 
        [HttpPost("add-userDto")]
        public async Task<IActionResult> CreateUserDto(UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("Invalid user data");
            }

            var user = new User
            {
                Username = userDto.Username,

            };
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
                    Bio = x.Profile.Bio
                }
            };
        }

        [HttpPost("add-profile")]
        public async Task<IActionResult> CreateProfile(ProfileDto profileDto)
        {
            //context.Profiles.Add(profile);
            //await context.SaveChangesAsync();
            //return Ok();

            var user = await context.Users.FindAsync(profileDto.UserId);
            if (user == null)
                return BadRequest("User not found");

            var profile = new Profile
            {
                Id = profileDto.Id,
                Bio = profileDto.Bio
            };

            context.Profiles.Add(profile);
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("get-profiles")]
        public async Task<IActionResult> GetProfile()
        {
            //return Ok(await context.Profiles.Include(x => x.User).ToListAsync());

            var profile = await context.Profiles.Include(p => p.User).Select(p => MapProfile(p)).ToListAsync();

            return Ok(profile);
        }

        [HttpGet("get-profile/{id}")]
        public async Task<IActionResult> GetProfileById(int id)
        {
            //return Ok(await context.Profiles.Include(x => x.User).ToListAsync());

            var profile = await context.Profiles.Include(p => p.User).Where(x => id == x.Id).Select(p => MapProfile(p)).FirstOrDefaultAsync();

            if (profile == null)
                return BadRequest("profile not found");

            return Ok(profile);
        }

        private static ProfileDto MapProfile(Profile p)
        {
            return new ProfileDto
            {
                Id = p.Id,
                Bio = p.Bio
            };
        }

    }
}
