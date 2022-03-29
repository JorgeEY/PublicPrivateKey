using System.IO;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text.Json;

class Program
{
    static void Main()
    {
        RSA rsa = RSA.Create(2048);

        File.WriteAllText(@"./Docs/publicKey.key", Base64UrlEncoder.Encode(rsa.ExportRSAPublicKey()));
        File.WriteAllText(@"./Docs/privateKey.key", Base64UrlEncoder.Encode(rsa.ExportRSAPrivateKey()));

        string privateKey = File.ReadAllText(@"./Docs/privateKey.key");
        string publicKey = File.ReadAllText(@"./Docs/publicKey.key");

        rsa.ImportRSAPublicKey(Base64UrlEncoder.DecodeBytes(publicKey), out _);
        rsa.ImportRSAPrivateKey(Base64UrlEncoder.DecodeBytes(privateKey), out _);

        RsaSecurityKey rsaSecurityKey = new RsaSecurityKey(rsa);

        JsonWebKey jsonWebKey = JsonWebKeyConverter.ConvertFromRSASecurityKey(rsaSecurityKey);

        File.WriteAllText(@"./Docs/jwk.json", JsonSerializer.Serialize(jsonWebKey));
    }
}