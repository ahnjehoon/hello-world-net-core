using System.Security.Cryptography;
using System.Text;

namespace API.Utils
{
	public static class EncryptHelper
	{
		public static string EncryptSHA512(this string data, string salt = "")
		{
			var hashValue = SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(data + salt));
			return Convert.ToBase64String(hashValue);
		}
	}
}
