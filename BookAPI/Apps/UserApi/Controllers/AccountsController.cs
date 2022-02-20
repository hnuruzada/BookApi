using AutoMapper;
using BookAPI.Apps.UserApi.DTOs.AccountDtos;
using BookAPI.Data.Entity;
using BookAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace BookAPI.Apps.UserApi.Controllers
{
    [ApiExplorerSettings(GroupName = "user_v1")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;

        public AccountsController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IJwtService jwtService,IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _jwtService = jwtService;
            _configuration = configuration;
        }

        //[HttpGet("roles")]
        //public async Task<IActionResult> CreateRoles()
        //{
        //    var result = await _roleManager.CreateAsync(new IdentityRole("Member"));
        //    result = await _roleManager.CreateAsync(new IdentityRole("Admin"));
        //    result = await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));

        //    AppUser admin = new AppUser
        //    {
        //        FullName = "Super Admin",
        //        UserName = "SuperAdmin",
        //    };

        //    var resultAdmin = await _userManager.CreateAsync(admin, "Admin123");

        //    var resultRole = await _userManager.AddToRoleAsync(admin, "SuperAdmin");

        //    return Ok();
        //}


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            AppUser user =await _userManager.FindByNameAsync(registerDto.UserName);

            if (user != null) return StatusCode(409);
            user = new AppUser
            {
                UserName = registerDto.UserName,
                FullName = registerDto.FullName,
            };
            var result=await _userManager.CreateAsync(user,registerDto.Password);


            if (!result.Succeeded)
                return BadRequest(result.Errors);


            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded)
                return BadRequest(result.Errors);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            AppUser user=await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null) return NotFound();
            if(!await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return NotFound();
            var roles = await _userManager.GetRolesAsync(user);

            string tokenStr = _jwtService.Genereate(user, roles, _configuration);
            return Ok(new {token=tokenStr});
        }
        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            AccountGetDto accountDto = _mapper.Map<AccountGetDto>(user);

            return Ok(accountDto);
        }
    }
}
