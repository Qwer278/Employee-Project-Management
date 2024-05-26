using System.Data.SqlClient;
using RestfullApi.Controllers;
using System.Data;
using System.Security.Cryptography;

namespace RestfullApi
{
    public class RefreshTokenGenerator : BaseController,IRefreshTokenGenerator
    {
        public string GenerateToken(string username)
        {
            var randomnumber=new byte[32];
            using(var randomnumbergenerator = RandomNumberGenerator.Create())
            {
                randomnumbergenerator.GetBytes(randomnumber);
                string RefreshToken = Convert.ToBase64String(randomnumber);
                return RefreshToken;
            }
        }
    }
}
