using Domain.Models;
using Domain.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UserService:IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly JWT _jwt;

        public UserService(UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager, 
            IOptions<JWT> jwt,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _jwt = jwt.Value;
        }
        public async Task<string> RegisterAsync(RegisterModel model)
        {
            var user = new AppUser
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
            if (userWithSameEmail == null)
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded && await _roleManager.RoleExistsAsync("User"))
                {
                    await _userManager.AddToRoleAsync(user, "User");

                }
                else if (result.Succeeded)
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = "User" });
                    await _userManager.AddToRoleAsync(user, "User");
                    return $"User with this {user.Email} is Successfully registered";
                }
                return $"{string.Join(' ', result.Errors.Select(x => x.Description + Environment.NewLine))}";
            }
            else
            {
                return $"Email {user.Email} is already registered.";
            }
        }
        
        public async Task<ResponseModel> AddUserToRoleAsync(ManageRoleModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (await _roleManager.RoleExistsAsync(model.NewRoleName))
            {
                if (user == null)
                {
                    return new ResponseModel { Message = "User not found!" };
                }
                else
                {
                    await ChangeUserRole(model, user);
                    return new ResponseModel { Message = "Successful", Status = true };
                }

            }
            else
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = model.NewRoleName});
                await _userManager.AddToRoleAsync(user, model.NewRoleName);
                return new ResponseModel { Message = "Role added Successfully", Status = true };
            }
            
            
        }

        private async Task ChangeUserRole(ManageRoleModel model, AppUser user)
        {
            
            if (model.ChangeRole)
            {
                await _userManager.RemoveFromRoleAsync(user, model.OldRoleName);
                await _userManager.AddToRoleAsync(user, model.NewRoleName);
            }
            else
            {
                await _userManager.AddToRoleAsync(user, model.NewRoleName);
            }
        }

        public async Task<ResponseModel> LoginAsync(LoginModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                return new ResponseModel { Status = false, Message = "Enter a valid mail" };
            }
            return await LogUserIn(model);
        }
        private async Task<ResponseModel> LogUserIn(LoginModel model)
        {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    return new ResponseModel { Status = false, Message = "Account with this email does not exist!" };
                }

                //get the user's roles
                var roles = await _userManager.GetRolesAsync(user);

                //check that the user is not null and that his password is correct
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var jwtTokenResponse = await GenerateJwtToken(user);
                    return jwtTokenResponse;
                }
                //return an authorization error if the checks fail
                return new ResponseModel { Message = "Username or password invalid, please try again with correct details.", Status = false };
        }

        private async Task<ResponseModel> GenerateJwtToken(AppUser user) //IdentityUser user
        {
            //get the user's roles
            var roles = await _userManager.GetRolesAsync(user);
            DateTime expirationDay; // NEED TO SPECIFY EXPIRATION TIME
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token;

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Key"]));
            var configExpTime = Encoding.ASCII.GetBytes(_configuration["JWT:DurationInMinutes"]);
            var correctExp = Encoding.ASCII.GetChars(configExpTime);
            expirationDay = DateTime.UtcNow.AddHours(double.Parse(correctExp));

            List<Claim> subjectClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
            subjectClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            subjectClaims.Add(new Claim(ClaimTypes.Email, user.Email));
            subjectClaims.Add(new Claim(ClaimTypes.Sid, user.Id.ToString()));
            subjectClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            subjectClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Email));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(subjectClaims.AsEnumerable()),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwt.Issuer,
                Audience = _jwt.Audience,
                Expires = expirationDay

            };

            token = tokenHandler.CreateToken(tokenDescriptor);
            //convert token to string
            var jwtToken = tokenHandler.WriteToken(token);

            var response = new
            {
                UserID = Guid.Parse(user.Id),
                Token = jwtToken,
                Email = user.Email,
                ExpiryTime = expirationDay,
                PhoneNumber = user.PhoneNumber,
                Roles = roles.ToList()
            };

            return new ResponseModel { Status = true, Returnobj = response, Message = "Successful" };

        }
    }
}
