using System.Security.Cryptography;
using System.Text;

namespace FunkyInventoryWPF.Core.Providers;

public static class AesManager
{
    private static Aes? AesCrypto { get; set; }

    private static Aes GetAesCrypto()
    {
        if (AesCrypto != null)
            return AesCrypto;

        var aesCsp = Aes.Create();
        var keySize = aesCsp.Key.Length;
        var ivSize = aesCsp.IV.Length;
        List<byte> byteCluster = [];
        var clusterName = typeof(AesManager).FullName;
        while ((keySize > ivSize ? keySize : ivSize) > byteCluster.Count)
            byteCluster.AddRange(Encoding.UTF8.GetBytes(clusterName ?? AppDomain.CurrentDomain.FriendlyName));

        aesCsp.Key = byteCluster.Take(keySize).ToArray();
        aesCsp.IV = byteCluster.Take(ivSize).ToArray();

        AesCrypto = aesCsp;

        return AesCrypto;
    }

    public static string EncryptString(string encryptString)
    {
        var retValue = string.Empty;
        if (string.IsNullOrEmpty(encryptString))
            return retValue;

        try
        {
            var inBlock = Encoding.UTF8.GetBytes(encryptString);
            retValue = Convert.ToBase64String(GetAesCrypto().CreateEncryptor().TransformFinalBlock(inBlock, 0, inBlock.Length));
        }
        catch (Exception ex)
        {
        }

        return retValue;
    }

    public static string DecryptString(string decryptString)
    {
        var retValue = string.Empty;
        if (string.IsNullOrEmpty(decryptString))
            return retValue;

        try
        {
            var inBlock = Convert.FromBase64String(decryptString);
            retValue = Encoding.UTF8.GetString(GetAesCrypto().CreateDecryptor().TransformFinalBlock(inBlock, 0, inBlock.Length));
        }
        catch (Exception ex)
        {
        }

        return retValue;
    }
}
