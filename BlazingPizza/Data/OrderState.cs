using BlazingPizza.Models;

namespace BlazingPizza.Data;

public class OrderState
{
    public bool ShowingConfigureDialog { get; private set; }
    public Pizza ConfiguringPizza { get; private set; }
    public Order Order { get; private set; } = new();

    public void ShowConfigurePizzaDialog(PizzaSpecial special)
    {
        ConfiguringPizza = new Pizza()
        {
            Special = special,
            SpecialId = special.Id,
            Toppings = new List<PizzaTopping>()
        };

        ConfiguringPizza.Size = ConfiguringPizza.DefaultSize;

        ShowingConfigureDialog = true;
    }

    public void CancelConfigurePizzaDialog() => ResetConfigurePizza();

    public void ConfirmConfigurePizzaDialog()
    {
        Order.Pizzas.Add(ConfiguringPizza);
        ResetConfigurePizza();
    }

    public void RemoveConfiguredPizza(Pizza pizza) => Order.Pizzas.Remove(pizza);

    public void ResetOrder() => Order = new();

    private void ResetConfigurePizza()
    {
        ConfiguringPizza = default;
        ShowingConfigureDialog = default;
    }
}
