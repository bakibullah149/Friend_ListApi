using Friend_List.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Friend_List.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : Controller
    {
        private readonly AppDbContext _context;
        public FriendController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Friend>>> Get()

        {
            return Ok(await _context.Friends.ToListAsync());
        }


        [HttpGet("Search")]
        public async Task<ActionResult<Friend>> SearchByName([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name parameter is required.");
            }
            var friend = await _context.Friends.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToListAsync();
            if (friend.Count == 0)
                return BadRequest("Friend doesn't found");
            else
                return Ok(friend);
        }

        [HttpPost("AddFriend")]
        public async Task<ActionResult<List<Friend>>> AddFriend(Friend newFriend)
        {
            _context.Friends.Add(newFriend);
            await _context.SaveChangesAsync();
            return Ok(await _context.Friends.ToListAsync());
        }

        [HttpPut("Update")]
        public async Task<ActionResult<List<Friend>>> UpdateFriend([FromQuery] String name, Friend updateFriend)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name parameter is required.");
            }
            if (updateFriend == null)
            {
                return BadRequest("Invalid Data for update");
            }
            var friend = await _context.Friends.FirstOrDefaultAsync(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (friend == null)
                return BadRequest("Friend doesn't found");
            friend.Name = updateFriend.Name;
            friend.UniversityName = updateFriend.UniversityName;
            friend.Department = updateFriend.Department;
            friend.HomeDistrict = updateFriend.HomeDistrict;
            friend.PhoneNumber = updateFriend.PhoneNumber;
            await _context.SaveChangesAsync();
            return Ok(await _context.Friends.ToListAsync());
            
        }

        [HttpDelete]
        public async Task<ActionResult<List<Friend>>> DeleteFriend([FromQuery] string name, Friend deleteFriend)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name parameter is required.");
            }
            var friend = await _context.Friends.FirstOrDefaultAsync(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (friend == null)
                return BadRequest("Friend doesn't found");
            _context.Friends.Remove(friend);
            await _context.SaveChangesAsync();
            return Ok(await _context.Friends.ToListAsync());
        }
    }
}
