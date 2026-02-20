namespace quick_start.Orders.Types;

public class Order
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public List<LineItem> Items { get; set; }
}