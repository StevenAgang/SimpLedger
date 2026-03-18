using SimpLedger.Repository.Configurations;

namespace SimpLedger.Repository.Interfaces.Account
{
    public interface ISetTokenSevice
    {
        /// <summary>
        ///  Generating a JWT cookie token
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        string GenerateJwtToken(int id, string name);
        string Hashed<T>(T value, string salt);
        int GenerateCode();
        string GenerateSalt();
        string GenerateGenericToken(int code);
        //string GenerateRandomString(int numberOfCharacters);
        HttpContext SetCookie(string tokenName, string actualToken, HttpContextSettings httpContextSettings, HttpContext context);
    }
}
