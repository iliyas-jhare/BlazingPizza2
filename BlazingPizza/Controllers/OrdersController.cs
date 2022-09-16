using BlazingPizza.Data;
using BlazingPizza.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazingPizza.Controllers;

[Route("orders")]
[ApiController]
public class OrdersController : Controller
{
	private readonly PizzaStoreContext db;

	public OrdersController(PizzaStoreContext db)
	{
		this.db = db;
	}

	[HttpGet]
	public async Task<ActionResult<List<OrderWithStatus>>> GetOrders()
	{
		var orders = await db.Orders
			.Include(o => o.Pizzas).ThenInclude(p => p.Special)
			.Include(o => o.Pizzas).ThenInclude(p => p.Toppings).ThenInclude(t => t.Topping)
			.OrderByDescending(o => o.CreatedTime)
			.ToListAsync();

		return orders.Select(o => OrderWithStatus.FromOrder(o)).ToList();
	}

	[HttpPost]
	public async Task<ActionResult<int>> PlaceOrder(Order order)
	{
		order.CreatedTime = DateTime.Now;
		order.Pizzas.ForEach(UpdateSpecialAndSpecialId);
		db.Orders.Attach(order);
		await db.SaveChangesAsync();
		return order.OrderId;
	}

	private void UpdateSpecialAndSpecialId(Pizza pizza)
	{
		pizza.SpecialId = pizza.Special.Id;
		pizza.Special = default;
	}
}
