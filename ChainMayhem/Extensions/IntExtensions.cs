namespace ChainMayhem.Extensions;

public static class IntExtensions
{
    /// <summary>Returns true if value is between two numbers.</summary>
    public static bool Between(this int value, int min, int max)
    {
        return value >= min && value <= max;
    }
}
