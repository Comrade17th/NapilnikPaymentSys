using System.Security.Cryptography;
using System.Text;

public class MD5Hasher : IHash
{
    public string Hash(string input) =>
        Convert.ToHexString(SHA1.HashData(Encoding.UTF8.GetBytes(input)));
}