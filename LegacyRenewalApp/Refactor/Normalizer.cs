namespace LegacyRenewalApp.Refactor;

public class Normalizer
{
    public static string Normalize(string input)
    {
        return input.Trim().ToUpperInvariant();
    }
}