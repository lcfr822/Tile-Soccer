public static class ExtensionMethods 
{
    /// <summary>
    /// Linearly translate a value from one number range to another.
    /// </summary>
    /// <param name="value">Initial value.</param>
    /// <param name="rangeAMin">Minimum value of initial number range.</param>
    /// <param name="rangeAMax">Maximum value of initial number range.</param>
    /// <param name="rangeBMin">Minimum value of target number range.</param>
    /// <param name="rangeBMax">Maximum value of target number range.</param>
    /// <returns>Value mapped to target number range.</returns>
    public static float Map(this float value, float rangeAMin, float rangeAMax, float rangeBMin, float rangeBMax)
    {
        return (value - rangeAMin) / (rangeAMax - rangeAMin) * (rangeBMax - rangeBMin) + rangeBMin;
    }
}
