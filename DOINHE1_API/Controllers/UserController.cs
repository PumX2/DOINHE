using Microsoft.AspNetCore.Mvc;
using DOINHE_BusinessObject;
using DOINHE_Repository;
using System.Linq;

namespace DOINHE1_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/user
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_userRepository.GetAllUsers());
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        // POST: api/user/signup
        [HttpPost("signup")]
        public IActionResult Signup([FromBody] User user)
        {
            // Kiểm tra xem email hoặc tên người dùng đã tồn tại chưa
            var existingUserByEmail = _userRepository.GetAllUsers().FirstOrDefault(u => u.Email == user.Email);
            if (existingUserByEmail != null)
            {
                return BadRequest("Email đã được sử dụng.");
            }

            

            user.Role = 2;

            _userRepository.SaveUser(user);
            return Ok("Đăng ký thành công.");
        }


        // POST: api/user/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] User loginUser)
        {
            var user = _userRepository.GetAllUsers()
                .FirstOrDefault(u => u.Email == loginUser.Email && u.Password == loginUser.Password);
            
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }
            var userId = user.Id;
            var name = user.Name;
            var role = user.Role == 1 ? "admin" : "user";
            return Ok(new { message = "Login successful", role = role, user = user, userId = userId, name = name });
        }


        // POST: api/user/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok("Logout successful");
        }

        // POST: api/user
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            _userRepository.SaveUser(user);
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        // PUT: api/user/{id}
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] User user)
        {
            var existingUser = _userRepository.GetUserById(id);
            if (existingUser == null)
                return NotFound();

            _userRepository.UpdateUser(user);
            return NoContent();
        }

        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
                return NotFound();

            _userRepository.DeleteUser(user);
            return NoContent();
        }
    }
}
