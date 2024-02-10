using Core.Settings;
using System.Security.Cryptography;
using System.Text;

namespace Cerebellum.Api.Auth;

public interface IPasswordService
{
    string Hash(string password, string salt);
    bool IsMatch(string expectedPassword, string password, string salt);
}

public sealed class PasswordService : IPasswordService
{
    private readonly string _pepper;

    public PasswordService(AppSecrets appSecrets)
    {
        _pepper = appSecrets.Auth.Pepper;
    }

    public string Hash(string password, string salt)
    {
        var toHash = password + salt + _pepper;

        return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(toHash)));
    }

    public bool IsMatch(string expectedPassword, string password, string salt)
    {
        var hashed = Hash(password, salt);

        return hashed == expectedPassword;
    }
}