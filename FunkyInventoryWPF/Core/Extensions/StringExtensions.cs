using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text;
using System.Xml.Serialization;
using FunkyInventoryWPF.Core.Providers;

public static class StringExtensions
{
    public static T? FromXml<T>(this string? xmlString)
    {
        T? retValue = default;
        if (string.IsNullOrEmpty(xmlString))
            return retValue;

        xmlString = xmlString.Trim();

        if (xmlString.StartsWith("<") && xmlString.EndsWith(">"))
            using (StringReader reader = new(xmlString))
            {
                retValue = (T?)(new XmlSerializer(typeof(T)).Deserialize(reader));
            }

        return retValue;
    }

    public static T? FromJson<T>(this string? jsonString)
    {
        T? retValue = default;
        if (string.IsNullOrEmpty(jsonString))
            return retValue;

        jsonString = jsonString.Trim();

        if (new[] { "{", "[" }.Any(jsonString.StartsWith) && new[] { "]", "}" }.Any(jsonString.EndsWith))
            retValue = JsonSerializer.Deserialize<T>(jsonString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        return retValue;
    }

    public static string? ToSplitTitle(this string? source)
    {
        if (string.IsNullOrEmpty(source))
            return source;
        return string.Join("", source.Select((c, i) => char.IsUpper(c) && i != 0 ? " " + c : "" + c));
    }

    public static string? Truncate(this string? source, int maxLength)
    {
        if (string.IsNullOrEmpty(source))
            return source;
        return source.Length <= maxLength ? source : source.Substring(0, maxLength);
    }

    public static string? ToTitleCase(this string? source)
    {
        if (string.IsNullOrEmpty(source))
            return source;
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(source.ToLower());
    }

    public static string? ToSafeFilename(this string? source)
    {
        if (string.IsNullOrEmpty(source))
            return source;

        foreach (char c in Path.GetInvalidFileNameChars())
            source = source.Replace(c.ToString(), "");

        return source;
    }

    public static string? Decrypt(this string? source)
        => source is null ? default : AesManager.DecryptString(source.Replace('-', '+').Replace('_', '/'));

    public static string? Encrypt(this string? source)
        => source is null ? default : AesManager.EncryptString(source ?? "").Replace('+', '-').Replace('/', '_');

    public static List<string>? FromIdentifier(this string? source)
    {
        if (source is null)
            return default;

        string? id = AesManager.DecryptString(source.Replace('-', '+').Replace('_', '/'));

        return id.Split('|')?.ToList();
    }

    public static string? LuceneEscape(this string? source)
    {
        if (string.IsNullOrEmpty(source))
            return source;

        char[] chars = ['+', '&', '-', '|', '!', '(', ')', '{', '}', '[', ']', '^', '"', '~', '*', '?', ':', '\\', '/'];
        var ret = $"*{string.Join("", source.Select(c => chars.Any(a => a == c) ? $"*\\{c}" : $"*{c}"))}*";

        return ret;
    }

    public static string? CalculateSHA256(this string source)
    {
        StringBuilder sb = new();
        Encoding enc = Encoding.UTF8;
        byte[] result = SHA256.HashData(enc.GetBytes(source));

        foreach (byte b in result)
            sb.Append(b.ToString("x2"));

        string? ret = sb.ToString();

        return ret;
    }

    public static string? CalculateSHA512(this string source)
    {
        StringBuilder sb = new();
        Encoding enc = Encoding.UTF32;
        byte[] result = SHA512.HashData(enc.GetBytes(source));

        var test = enc.GetString(result);

        foreach (byte b in result)
            sb.Append(b.ToString("x2"));

        string? ret = sb.ToString();

        return ret;
    }

    private const int keySize = 64;
    private const int iterations = 350000;
    private const string saltString =
        "0D1FBD58F20D6233856E454DE91583F59F979EBFBE1C80CB1CD974041F5C678FDAAAAC44DB4CF5D323A9B92D6E9305D3E8E60FC956E44B719B81F86948AAE13F";
    public static string? HashPassword(this string source)
        => Convert.ToHexString(Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(source), Convert.FromHexString(saltString), iterations,
            HashAlgorithmName.SHA512, keySize));

    public static bool IsValidEmail(this string source)
        => Regex.IsMatch(source, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
            RegexOptions.IgnoreCase);

    public static bool IsValidPassword(this string source)
        => Regex.IsMatch(source, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
}
