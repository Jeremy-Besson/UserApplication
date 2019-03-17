using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BalticAmadeusTask.Helpers
{
    public interface IHashingService
    {
        string GenerateHashString(string text);
    }

    public class HashingService: IHashingService
    {
        public string GenerateHashString(string text)
        {
            using (SHA256 algo = SHA256.Create())
            {
                // Compute hash from text parameter
                algo.ComputeHash(Encoding.UTF8.GetBytes(text));

                // Get has value in array of bytes
                var result = algo.Hash;

                // Return as hexadecimal string
                return string.Join(
                    string.Empty,
                    result.Select(x => x.ToString("x2")));
            }
        }
    }
}