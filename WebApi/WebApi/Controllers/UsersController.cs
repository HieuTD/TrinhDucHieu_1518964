using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.DTOs.Users;
using WebApi.EF;
using WebApi.Helper.JwtConfigure;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DBcontext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        static string id;


        public UsersController(DBcontext context, UserManager<AppUser> userManager, IMapper mapper, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetAllUSers()
        {
            return await _userManager.Users.ToListAsync();
        }

        //register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] UserCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userIdentity = _mapper.Map<AppUser>(request);
            var result = await _userManager.CreateAsync(userIdentity, request.Password);
            _context.AppUsers.Update(userIdentity);
            //if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginrequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var identity = await GetClaimsIdentity(request.UserName, request.Password);
            if (identity == null)
            {
                return BadRequest();
            }

            var response = new
            {
                id = identity.Claims.Single(c => c.Type == "id").Value,
                role = _context.AppUsers.FirstOrDefault(s => s.Id == id).Role,
                fullname = _context.AppUsers.FirstOrDefault(s => s.Id == id).FirstName + " " + _context.AppUsers.FirstOrDefault(s => s.Id == id).LastName,
                email = _context.AppUsers.FirstOrDefault(s => s.Id == id).Email,
                auth_token = await _jwtFactory.GenerateEncodedToken(request.UserName, identity),
                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds
            };
            var json = JsonConvert.SerializeObject(response, _serializerSettings);
            return new OkObjectResult(json);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                // get the user to verifty
                var userToVerify = await _userManager.FindByNameAsync(userName);
                if (userToVerify != null)
                {
                    // check the credentials  
                    if (await _userManager.CheckPasswordAsync(userToVerify, password))
                    {
                        AuthHistory auth = new AuthHistory();
                        auth.UserId = userToVerify.Id;
                        auth.CreatedAt = DateTime.Now;
                        _context.AuthHistories.Add(auth);
                        await _context.SaveChangesAsync();
                        id = userToVerify.Id;
                        return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id));
                    }
                }
            }
            return await Task.FromResult<ClaimsIdentity>(null);
        }

        [HttpGet("getUserAddress/{id}")]
        public async Task<IActionResult> GetUerAddress(string id)
        {
            //var id = json.GetValue("id_user").ToString();
            var result = await _context.AppUsers.Where(d => d.Id == id).Select(d => d.Address).SingleOrDefaultAsync();
            return Ok(result);
        }


        [HttpGet("authHistory")]
        public async Task<ActionResult<AppUser>> GetAuthHistory()
        {
            var appUser = await _context.AppUsers.FindAsync(id);
            return appUser;
        }

        [HttpPost("logout")]
        public IActionResult logout()
        {
            id = null;
            return Ok();
        }


        [HttpPut("updateUser/{id}")]
        public async Task<IActionResult> UpdateUser(string id, UserCreateRequest request)
        {
            //var id = json.GetValue("id_user").ToString();
            var result = await _context.AppUsers.Where(d => d.Id == id).SingleOrDefaultAsync();
            request.FirstName = request.LastName;
            request.LastName = request.LastName;
            request.PhoneNumber = request.PhoneNumber;
            request.Address = request.Address;
            request.Email = request.Email;
            request.Password = request.Password;
            return Ok(result);
        }
    }
}
