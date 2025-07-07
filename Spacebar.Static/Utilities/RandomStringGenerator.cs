namespace Spacebar.Static.Utilities;

public class RandomStringGenerator {
    private static readonly Random rnd = new();
    private static readonly string str_chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public static string Generate(int length) {
        var res = "";
        for (var i = 0; i < length; i++) res += str_chars[rnd.Next(str_chars.Length)];

        return res;
    }
}