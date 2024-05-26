using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RestfullApi.Models;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace RestfullApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        public CookieOptions options = new CookieOptions();
        private readonly JWTSettingsModel _jwtSettings;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        public UserController(IOptions<JWTSettingsModel> options, IRefreshTokenGenerator _refreshtoken)
        {
            _jwtSettings = options.Value;
            _refreshTokenGenerator = _refreshtoken;
        }
        [NonAction]
        public TokenResponse Authenticate(string username, Claim[] claims)
        {
            var _jwtsettings = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JWTSettings");

            var authkey = _jwtsettings["SecurityKey"];
            var tokenkey = Encoding.UTF8.GetBytes(authkey);
            TokenResponse tokenResponse = new TokenResponse();
            var tokenhandler = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(20),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
                );
            tokenResponse.JWTToken = new JwtSecurityTokenHandler().WriteToken(tokenhandler);
            tokenResponse.RefreshToken = _refreshTokenGenerator.GenerateToken(username);
            string? UserName = Request.Cookies["UserName"];
            if (UserName == null)
            {
                options.Expires = DateTime.Now.AddDays(7);
                Response.Cookies.Append("UserName", username, options);
                Response.Cookies.Append("RefreshToken", tokenResponse.RefreshToken, options);
            }
            else
            {
                Response.Cookies.Delete("RefreshToken");
                options.Expires = DateTime.Now.AddDays(7);
                Response.Cookies.Append("RefreshToken", tokenResponse.RefreshToken, options);
            }
            return tokenResponse;
        }
        [Route("Authenticate")]
        [HttpPost]
        public IActionResult Authenticate([FromBody] UserModel user)
        {
            TokenResponse tokenResponse = new TokenResponse();
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@Username", SqlDbType.VarChar);
            parameters[0].Value = user.UserName.ToString();
            DataTable dataTable = ExecuteProcedure("GetAccountPassword", parameters);
            if (user.Password == dataTable.Rows[0]["Password"].ToString())
            {
                var tokenhandler = new JwtSecurityTokenHandler();
                var tokenkey = Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity
                    (
                        new Claim[]
                        {
                            new Claim(ClaimTypes.Name, user.UserName.ToString()),
                            new Claim(ClaimTypes.Role, dataTable.Rows[0]["RoleName"].ToString()),
                        }
                    ),
                    Expires = DateTime.Now.AddMinutes(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
                };
                var token = tokenhandler.CreateToken(tokenDescriptor);
                string finaltoken = tokenhandler.WriteToken(token);

                tokenResponse.JWTToken = finaltoken;
                tokenResponse.RefreshToken = _refreshTokenGenerator.GenerateToken(user.UserName);
                string? UserName = Request.Cookies["UserName"];
                if (UserName == null)
                {
                    options.Expires = DateTime.Now.AddDays(7);
                    Response.Cookies.Append("UserName", user.UserName.ToString(), options);
                    Response.Cookies.Append("RefreshToken", tokenResponse.RefreshToken, options);
                }
                else
                {
                    Response.Cookies.Delete("RefreshToken");
                    options.Expires = DateTime.Now.AddDays(7);
                    Response.Cookies.Append("RefreshToken", tokenResponse.RefreshToken, options);
                }
                return Ok(tokenResponse);
            }
            else
            {
                return Unauthorized();
            }
        }
        [Route("Refresh")]
        [HttpPost]
        public IActionResult Refresh([FromBody] TokenResponse token)
        {
            var tokenhandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var _jwtsettings = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JWTSettings");
            var authkey = _jwtsettings["SecurityKey"];
            var principal = tokenhandler.ValidateToken(token.JWTToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authkey)),
                ValidateIssuer = false,
                ValidateAudience = false
            }, out securityToken);

            var _token = securityToken as JwtSecurityToken;
            if (_token != null && !_token.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
            {
                return Unauthorized();
            }
            var username = principal.Identity.Name;
            string? UserName = Request.Cookies["UserName"];
            string? RefreshToken = Request.Cookies["RefreshToken"];
            if (RefreshToken == null)
            {
                return Unauthorized();
            }

            TokenResponse response = Authenticate(username, principal.Claims.ToArray());
            return Ok(response);
        }
    }
}
