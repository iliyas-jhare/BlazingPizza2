using BlazingPizza.Extensions;

namespace BlazingPizza.Models;

/// <summary>
/// Represents a customized pizza as part of an order
/// </summary>
public record Pizza
{
    public const int DefaultSize = 12;
    public const int MinimumSize = 9;
    public const int MaximumSize = 17;

    public int Id { get; set; }

    public int OrderId { get; set; }

    public PizzaSpecial Special { get; set; }

    public int SpecialId { get; set; }

    public int Size { get; set; }

    public List<PizzaTopping> Toppings { get; set; }

    public decimal GetBasePrice() => Special.BasePrice * Size / DefaultSize;

    public decimal GetTotalPrice() => GetBasePrice();

    public string GetFormattedTotalPrice() => GetTotalPrice().ToFormattedPrice();
}
