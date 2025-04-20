namespace Api.Extensions
{
    public static class DecimalExtensions
    {
        public static decimal Round(this decimal value, int precision = 2)
            => Math.Round(value, precision, MidpointRounding.ToEven);
    }
}
