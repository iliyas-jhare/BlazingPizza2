namespace BlazingPizza.Extensions;

public static class DecimalEx
{
    public static string ToFormattedPrice(this decimal price) 
        => price.ToString("0.00");
}
