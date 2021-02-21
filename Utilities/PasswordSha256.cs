using System.Text;
using System.Security.Cryptography;

namespace netart.Utilities
{
    public class PasswordSha256
    {
        public string create_sha256(string str)
        {
            StringBuilder stringBuilder = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(str));

                foreach (byte b in result)
                    stringBuilder.Append(b.ToString("x2"));
            }
            return stringBuilder.ToString();
        }
    }
}