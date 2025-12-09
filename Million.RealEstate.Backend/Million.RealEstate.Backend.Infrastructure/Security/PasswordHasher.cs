using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Million.RealEstate.Backend.Application.Abstractions;
using System.Security.Cryptography;

namespace Million.RealEstate.Backend.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        // salt aleatoria
        byte[] salt = RandomNumberGenerator.GetBytes(16);

        byte[] subkey = KeyDerivation.Pbkdf2(
            password,
            salt,
            KeyDerivationPrf.HMACSHA256,
            iterationCount: 100_000,
            numBytesRequested: 32);

        var resultBytes = new byte[1 + salt.Length + subkey.Length];
        resultBytes[0] = 0x01; // versión
        Buffer.BlockCopy(salt, 0, resultBytes, 1, salt.Length);
        Buffer.BlockCopy(subkey, 0, resultBytes, 1 + salt.Length, subkey.Length);

        return Convert.ToBase64String(resultBytes);
    }

    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        var decoded = Convert.FromBase64String(hashedPassword);

        if (decoded[0] != 0x01) return false;

        byte[] salt = new byte[16];
        Buffer.BlockCopy(decoded, 1, salt, 0, salt.Length);

        byte[] storedSubkey = new byte[32];
        Buffer.BlockCopy(decoded, 1 + salt.Length, storedSubkey, 0, storedSubkey.Length);

        byte[] generatedSubkey = KeyDerivation.Pbkdf2(
            providedPassword,
            salt,
            KeyDerivationPrf.HMACSHA256,
            100_000,
            32);

        return CryptographicOperations.FixedTimeEquals(storedSubkey, generatedSubkey);
    }
}
