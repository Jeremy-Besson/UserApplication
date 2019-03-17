using System.Collections.Generic;

namespace BalticAmadeusTask.Services
{
    public class PasswordPolicyService : IPasswordPolicyService
    {
        public List<string> PasswordErrors(string password)
        {
            List<string> errors = new List<string>();
            if (string.IsNullOrEmpty(password))
            {
                errors.Add("Mandatory.");
                return errors;
            }

            if (password.Length < 10)
            {
                errors.Add("Too short.");
            }
            if (password.Contains("NO"))
            {
                errors.Add("NO is not allowed.");
            }
            return errors;
        }
    }
}
