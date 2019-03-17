using System.Collections.Generic;

namespace BalticAmadeusTask.Services
{
    public interface IPasswordPolicyService
    {
        List<string> PasswordErrors(string password);
    }
}
