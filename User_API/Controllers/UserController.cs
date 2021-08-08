using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using User_API.Models;
using User_API.Data;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace User_API.Controllers
{
    [Route("api/user/")]
    [ApiController]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly SqliteContext _context;

        public UserController(SqliteContext context)
        {
            _context = context;
        }

        // GET: api/user
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.UserModel.ToListAsync();
            users.Reverse();
            return Ok(users);
        }
        
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<UserModel>>> Searchuser([FromQuery] string Email, [FromQuery] string Phone)
        {
            IQueryable<UserModel> query = _context.UserModel;

            if (!string.IsNullOrEmpty(Email))
            {
                query = query.Where(e => e.Email.Contains(Email)).OrderBy(x => x.Email);
            }
            if (!string.IsNullOrEmpty(Phone))
            {
                query = query.Where(e => e.Phone.Contains(Phone)).OrderBy(x => x.Phone);
            }
            var result = await query.ToListAsync();
            return Ok(result);
        }

        // GET api/user/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _context.UserModel.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST api/user/
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserModel user)
        {
            if (user == null || String.IsNullOrEmpty(user.FullName) || String.IsNullOrEmpty(user.Email))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            await _context.UserModel.AddAsync(user);
            _context.SaveChanges();

            return Ok(user.UserId);
        }

        // PUT api/user/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserModel user)
        {
            if ( user == null || (id != user.UserId || user.UserId == 0) || String.IsNullOrEmpty(user.FullName) || String.IsNullOrEmpty(user.Email))
            {
                return BadRequest();
            }

            var existingUser = _context.UserModel.SingleOrDefault(x => x.UserId == user.UserId);
            if (existingUser == null)
            {
                return NotFound();
            }
            _context.Entry(existingUser).CurrentValues.SetValues(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(user);
        }

        // DELETE api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = _context.UserModel.SingleOrDefault(p => p.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.UserModel.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        private bool UserExists(int id)
        {
            return _context.UserModel.Any(e => e.UserId == id);
        }
    }
}
