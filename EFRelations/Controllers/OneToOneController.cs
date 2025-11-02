using EFRelations.Data;
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
            return Ok(await context.Users.Include(x => x.Profile).ToListAsync());
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
