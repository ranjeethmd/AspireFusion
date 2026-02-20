using quick_start.Orders.Types;

namespace AspireFusion.Tests;

public class OrderTests
{
    #region Order Model Tests

    [Fact]
    public void Order_Properties_CanBeSetAndRetrieved()
    {
        // Arrange & Act
        var order = new Order
        {
            Id = 1,
            Name = "Test Order",
            Description = "Test Description",
            Items = new List<LineItem>()
        };

        // Assert
        Assert.Equal(1, order.Id);
        Assert.Equal("Test Order", order.Name);
        Assert.Equal("Test Description", order.Description);
        Assert.NotNull(order.Items);
        Assert.Empty(order.Items);
    }

    [Fact]
    public void Order_WithLineItems_ContainsCorrectItems()
    {
        // Arrange
        var lineItems = new List<LineItem>
        {
            new() { Id = 1, Quantity = 2, ProductId = 100 },
            new() { Id = 2, Quantity = 3, ProductId = 200 }
        };

        // Act
        var order = new Order
        {
            Id = 1,
            Name = "Order with Items",
            Description = "Description",
            Items = lineItems
        };

        // Assert
        Assert.Equal(2, order.Items.Count);
        Assert.Equal(1, order.Items[0].Id);
        Assert.Equal(2, order.Items[1].Id);
    }

    #endregion

    #region LineItem Model Tests

    [Fact]
    public void LineItem_Properties_CanBeSetAndRetrieved()
    {
        // Arrange & Act
        var lineItem = new LineItem
        {
            Id = 1,
            Quantity = 5,
            ProductId = 100
        };

        // Assert
        Assert.Equal(1, lineItem.Id);
        Assert.Equal(5, lineItem.Quantity);
        Assert.Equal(100, lineItem.ProductId);
    }

    [Fact]
    public void LineItem_DefaultValues_AreZero()
    {
        // Arrange & Act
        var lineItem = new LineItem();

        // Assert
        Assert.Equal(0, lineItem.Id);
        Assert.Equal(0, lineItem.Quantity);
        Assert.Equal(0, lineItem.ProductId);
    }

    #endregion

    #region LineItemType Tests

    [Fact]
    public void LineItemType_GetProduct_ReturnsProductWithCorrectId()
    {
        // Arrange
        var lineItem = new LineItem
        {
            Id = 1,
            Quantity = 2,
            ProductId = 42
        };

        // Act
        var product = LineItemType.GetProduct(lineItem);

        // Assert
        Assert.NotNull(product);
        Assert.Equal(42, product.Id);
    }

    [Fact]
    public void LineItemType_GetProduct_WithDifferentProductIds_ReturnsCorrectProducts()
    {
        // Arrange
        var lineItem1 = new LineItem { Id = 1, Quantity = 1, ProductId = 1 };
        var lineItem2 = new LineItem { Id = 2, Quantity = 2, ProductId = 999 };

        // Act
        var product1 = LineItemType.GetProduct(lineItem1);
        var product2 = LineItemType.GetProduct(lineItem2);

        // Assert
        Assert.Equal(1, product1.Id);
        Assert.Equal(999, product2.Id);
    }

    #endregion

    #region Query Tests

    [Fact]
    public void Query_GetOrders_ReturnsNonEmptyArray()
    {
        // Act
        var orders = Query.GetOrders();

        // Assert
        Assert.NotNull(orders);
        Assert.NotEmpty(orders);
    }

    [Fact]
    public void Query_GetOrders_ReturnsTwoOrders()
    {
        // Act
        var orders = Query.GetOrders();

        // Assert
        Assert.Equal(2, orders.Length);
    }

    [Fact]
    public void Query_GetOrders_FirstOrderHasCorrectProperties()
    {
        // Act
        var orders = Query.GetOrders();
        var firstOrder = orders[0];

        // Assert
        Assert.Equal(1, firstOrder.Id);
        Assert.Equal("Order 1", firstOrder.Name);
        Assert.Equal("Description 1", firstOrder.Description);
    }

    [Fact]
    public void Query_GetOrders_SecondOrderHasCorrectProperties()
    {
        // Act
        var orders = Query.GetOrders();
        var secondOrder = orders[1];

        // Assert
        Assert.Equal(2, secondOrder.Id);
        Assert.Equal("Order 2", secondOrder.Name);
        Assert.Equal("Description 2", secondOrder.Description);
    }

    [Fact]
    public void Query_GetOrders_FirstOrderHasTwoLineItems()
    {
        // Act
        var orders = Query.GetOrders();
        var firstOrder = orders[0];

        // Assert
        Assert.NotNull(firstOrder.Items);
        Assert.Equal(2, firstOrder.Items.Count);
    }

    [Fact]
    public void Query_GetOrders_SecondOrderHasTwoLineItems()
    {
        // Act
        var orders = Query.GetOrders();
        var secondOrder = orders[1];

        // Assert
        Assert.NotNull(secondOrder.Items);
        Assert.Equal(2, secondOrder.Items.Count);
    }

    [Fact]
    public void Query_GetOrders_FirstOrderLineItemsHaveCorrectData()
    {
        // Act
        var orders = Query.GetOrders();
        var items = orders[0].Items;

        // Assert
        Assert.Equal(1, items[0].Id);
        Assert.Equal(1, items[0].Quantity);
        Assert.Equal(1, items[0].ProductId);

        Assert.Equal(2, items[1].Id);
        Assert.Equal(2, items[1].Quantity);
        Assert.Equal(2, items[1].ProductId);
    }

    [Fact]
    public void Query_GetOrders_SecondOrderLineItemsHaveCorrectData()
    {
        // Act
        var orders = Query.GetOrders();
        var items = orders[1].Items;

        // Assert
        Assert.Equal(3, items[0].Id);
        Assert.Equal(3, items[0].Quantity);
        Assert.Equal(3, items[0].ProductId);

        Assert.Equal(4, items[1].Id);
        Assert.Equal(4, items[1].Quantity);
        Assert.Equal(4, items[1].ProductId);
    }

    [Fact]
    public void Query_GetOrders_ReturnsNewArrayEachCall()
    {
        // Act
        var orders1 = Query.GetOrders();
        var orders2 = Query.GetOrders();

        // Assert
        Assert.NotSame(orders1, orders2);
    }

    [Fact]
    public void Query_GetOrders_AllOrdersHaveUniqueIds()
    {
        // Act
        var orders = Query.GetOrders();
        var ids = orders.Select(o => o.Id).ToList();

        // Assert
        Assert.Equal(ids.Count, ids.Distinct().Count());
    }

    [Fact]
    public void Query_GetOrders_AllLineItemsAcrossOrdersHaveUniqueIds()
    {
        // Act
        var orders = Query.GetOrders();
        var allLineItemIds = orders.SelectMany(o => o.Items).Select(i => i.Id).ToList();

        // Assert
        Assert.Equal(allLineItemIds.Count, allLineItemIds.Distinct().Count());
    }

    #endregion

    #region Product Record Tests (Orders namespace)

    [Fact]
    public void Product_Record_CanBeCreatedWithId()
    {
        // Act
        var product = new Product(42);

        // Assert
        Assert.Equal(42, product.Id);
    }

    [Fact]
    public void Product_Record_EqualityByValue()
    {
        // Arrange
        var product1 = new Product(1);
        var product2 = new Product(1);
        var product3 = new Product(2);

        // Assert
        Assert.Equal(product1, product2);
        Assert.NotEqual(product1, product3);
    }

    [Fact]
    public void Product_Record_HashCodeConsistentWithEquality()
    {
        // Arrange
        var product1 = new Product(1);
        var product2 = new Product(1);

        // Assert
        Assert.Equal(product1.GetHashCode(), product2.GetHashCode());
    }

    #endregion
}
