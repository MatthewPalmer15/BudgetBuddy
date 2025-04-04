using System.Text;
using BudgetBuddy.Internal.Encryption.Providers.Abstract;
using BudgetBuddy.Internal.Encryption.Providers.Concrete;

namespace BudgetBuddy.Services.Encryption;

internal class EncryptionService : IEncryptionService
{
    private byte[] _Key = [218, 67, 67, 63, 204, 244, 241, 114, 106, 200, 253, 68, 254, 170, 233, 174, 241, 127, 130, 233, 16, 17, 217, 204, 18, 174, 7, 247, 196, 98, 133, 163];
    private byte[] _Iv = [58, 191, 153, 193, 2, 157, 167, 89, 225, 55, 84, 168, 83, 75, 77, 242];


    /// <summary>
    ///     Gets an encryption provider that uses the AES algorithm.
    /// </summary>
    /// <value>
    ///     An <see cref="IEncryptionProvider" /> that uses the AES algorithm.
    /// </value>
    public IEncryptionProvider EncryptionProvider =>
        new AesProvider(_Key, _Iv);

    /// <summary>
    ///     Encrypts the specified text.
    /// </summary>
    /// <param name="text">The text to encrypt.</param>
    /// <returns>A string that represents the encrypted text, in Base64 format.</returns>
    public string Encrypt(string text)
    {
        var textBytes = Encoding.UTF8.GetBytes(text);
        var encryptedBytes = EncryptionProvider.Encrypt(textBytes);
        return Convert.ToBase64String(encryptedBytes);
    }

    /// <summary>
    ///     Decrypts the specified text.
    /// </summary>
    /// <param name="encryptedText">The text to decrypt, in Base64 format.</param>
    /// <returns>The decrypted text.</returns>
    public string Decrypt(string encryptedText)
    {
        var encryptedBytes = Convert.FromBase64String(encryptedText);
        var decryptedBytes = EncryptionProvider.Decrypt(encryptedBytes);
        return Encoding.UTF8.GetString(decryptedBytes);
    }
}