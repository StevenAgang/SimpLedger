using SimpLedger.Repository.Configurations.Exception_Extender;
using SimpLedger.Repository.ViewModels.Account;
using System.Text.RegularExpressions;

namespace SimpLedger.Repository.Configurations.Validation.Account
{
    public class UserAccountValidation()
    {
        public void NotNullEmailAndPassword(UserAccountLogin user) 
        {
            string emailPattern = @"^[\w.-]+@[\w.-]+\.\w{2,}$";
            if (string.IsNullOrWhiteSpace(user.Email)) throw new BadRequest("Email is required");
            if (!Regex.IsMatch(user.Email, emailPattern)) throw new BadRequest("Email is invalid");
            if (string.IsNullOrWhiteSpace(user.Password)) throw new BadRequest("Password is required");
        }
        public void ValidEmailAndPassword(string email, string password)
        {
            string emailPattern = @"^[\w.-]+@[\w.-]+\.\w{2,}$";
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*]).{8,}$";

            if (string.IsNullOrWhiteSpace(email)) throw new BadRequest("Email is required");
            if (string.IsNullOrWhiteSpace(password)) throw new BadRequest("Password is required");
            if (!Regex.IsMatch(email, emailPattern)) throw new BadRequest("Email is invalid");
            if (!Regex.IsMatch(password, passwordPattern)) throw new BadRequest("Password must contain at least one uppercase, lowercase, number, and special character, and at least 8 characters long.");
        }

        public void ValidInformation(UserAccountCreation user)
        {
            if (string.IsNullOrWhiteSpace(user.Email)) throw new BadRequest("Email is required");
            if (string.IsNullOrWhiteSpace(user.Password)) throw new BadRequest("Password is required");
            if (string.IsNullOrWhiteSpace(user.FirstName)) throw new BadRequest("Firstname is required");
            if (string.IsNullOrWhiteSpace(user.LastName)) throw new BadRequest("Lastname is required");
            if (Convert.ToInt32(user.UserType) < 1 || Convert.ToInt32(user.UserType) > 2) throw new BadRequest("Account type is required");
            if (Convert.ToInt32(user.UserType) == 1)
            {
                if (string.IsNullOrWhiteSpace(user.BusinessName)) throw new BadRequest("Business name is required");
                if (string.IsNullOrWhiteSpace(user.BusinessAddress)) throw new BadRequest("Business address is required");
                if (string.IsNullOrWhiteSpace(user.BusinessEmail)) throw new BadRequest("Business email is required");
            }
        }
    }
}
