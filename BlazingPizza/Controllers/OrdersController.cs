using BlazingPizza.Data;
using BlazingPizza.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

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
		=> (await GetOrdersQuery(db.Orders)
		.OrderByDescending(o => o.CreatedTime)
		.ToListAsync())
		.Select(o => OrderWithStatus.FromOrder(o))
		.ToList();

	[HttpPost]
	public async Task<ActionResult<int>> PlaceOrder(Order order)
	{
		order.CreatedTime = DateTime.Now;
		order.Pizzas.ForEach(UpdateSpecialAndSpecialId);
		db.Orders.Attach(order);
		await db.SaveChangesAsync();
		return order.OrderId;
	}

	[HttpGet("{orderId}")]
	public async Task<ActionResult<OrderWithStatus>> GetOrderWithStatus(int orderId)
		=> (await GetOrdersQuery(db.Orders.Where(o => o.OrderId == orderId)).SingleOrDefaultAsync()) is Order order
		? OrderWithStatus.FromOrder(order)
		: NotFound();

	private IIncludableQueryable<Order, Topping> GetOrdersQuery(IQueryable<Order> orders)
		=> orders
		.Include(o => o.Pizzas).ThenInclude(p => p.Special)
		.Include(o => o.Pizzas).ThenInclude(p => p.Toppings).ThenInclude(t => t.Topping);

	private void UpdateSpecialAndSpecialId(Pizza pizza)
	{
		pizza.SpecialId = pizza.Special.Id;
		pizza.Special = default;
	}
}
