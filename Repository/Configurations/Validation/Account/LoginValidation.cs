using SimpLedger.Repository.ViewModels.Account;
using System.Text.RegularExpressions;

namespace SimpLedger.Repository.Configurations.Validation.Account
{
    public class LoginValidation
    {
        public bool ValidInput(UserAccountLogin user)
        {
            string emailPattern = @"^[\w.-]+@[\w.-]+\.\w{2,}$";
            string passwordPattern = @"/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.!@#$%^&*).{8,}$";

            if (string.IsNullOrEmpty(user.Email)) throw new Exception("Email is required");
            if (string.IsNullOrEmpty(user.Password)) throw new Exception("Password is required");
            if (!Regex.IsMatch(emailPattern, user.Email, RegexOptions.IgnoreCase)) throw new Exception("Email is invalid");
            if (!Regex.IsMatch(passwordPattern, user.Password)) throw new Exception("Password must contain at least one uppercase, lowercase, number, and special character, and at least 8 characters long.");

            return true;
        }
    }
}
