namespace Spacebar.Static.Utilities;

public class RandomStringGenerator
{
    private readonly static Random rnd = new();
    private readonly static string str_chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public static string Generate(int length)
    {
        string res = "";
        for (int i = 0; i < length; i++)
        {
            res += str_chars[rnd.Next(str_chars.Length)];
        }

        return res;
    }
}