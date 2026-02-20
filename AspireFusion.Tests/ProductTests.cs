using quick_start.Products.Types;

namespace AspireFusion.Tests;

public class ProductTests
{
    #region Product Record Tests

    [Fact]
    public void Product_Properties_CanBeInitialized()
    {
        // Arrange & Act
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Sku = "TEST-SKU",
            Description = "Test Description",
            Price = 19.99m
        };

        // Assert
        Assert.Equal(1, product.Id);
        Assert.Equal("Test Product", product.Name);
        Assert.Equal("TEST-SKU", product.Sku);
        Assert.Equal("Test Description", product.Description);
        Assert.Equal(19.99m, product.Price);
    }

    [Fact]
    public void Product_DefaultValues_AreDefault()
    {
        // Arrange & Act
        var product = new Product();

        // Assert
        Assert.Equal(0, product.Id);
        Assert.Null(product.Name);
        Assert.Null(product.Sku);
        Assert.Null(product.Description);
        Assert.Equal(0m, product.Price);
    }

    [Fact]
    public void Product_RecordEquality_ByValue()
    {
        // Arrange
        var product1 = new Product
        {
            Id = 1,
            Name = "Product",
            Sku = "SKU",
            Description = "Desc",
            Price = 10m
        };

        var product2 = new Product
        {
            Id = 1,
            Name = "Product",
            Sku = "SKU",
            Description = "Desc",
            Price = 10m
        };

        var product3 = new Product
        {
            Id = 2,
            Name = "Product",
            Sku = "SKU",
            Description = "Desc",
            Price = 10m
        };

        // Assert
        Assert.Equal(product1, product2);
        Assert.NotEqual(product1, product3);
    }

    [Fact]
    public void Product_HashCode_ConsistentWithEquality()
    {
        // Arrange
        var product1 = new Product
        {
            Id = 1,
            Name = "Product",
            Sku = "SKU",
            Description = "Desc",
            Price = 10m
        };

        var product2 = new Product
        {
            Id = 1,
            Name = "Product",
            Sku = "SKU",
            Description = "Desc",
            Price = 10m
        };

        // Assert
        Assert.Equal(product1.GetHashCode(), product2.GetHashCode());
    }

    [Fact]
    public void Product_Price_SupportsDecimalPrecision()
    {
        // Arrange & Act
        var product = new Product
        {
            Id = 1,
            Name = "Precise Product",
            Sku = "PREC",
            Description = "Precise",
            Price = 123.456789m
        };

        // Assert
        Assert.Equal(123.456789m, product.Price);
    }

    [Fact]
    public void Product_Price_CanBeZero()
    {
        // Arrange & Act
        var product = new Product
        {
            Id = 1,
            Name = "Free Product",
            Sku = "FREE",
            Description = "Free",
            Price = 0m
        };

        // Assert
        Assert.Equal(0m, product.Price);
    }

    [Fact]
    public void Product_Price_CanBeNegative()
    {
        // Arrange & Act (negative price for discounts/refunds)
        var product = new Product
        {
            Id = 1,
            Name = "Refund",
            Sku = "REFUND",
            Description = "Refund item",
            Price = -10m
        };

        // Assert
        Assert.Equal(-10m, product.Price);
    }

    #endregion

    #region Query.GetProducts Tests

    [Fact]
    public void Query_GetProducts_ReturnsNonEmptyArray()
    {
        // Act
        var products = Query.GetProducts();

        // Assert
        Assert.NotNull(products);
        Assert.NotEmpty(products);
    }

    [Fact]
    public void Query_GetProducts_ReturnsFourProducts()
    {
        // Act
        var products = Query.GetProducts();

        // Assert
        Assert.Equal(4, products.Length);
    }

    [Fact]
    public void Query_GetProducts_FirstProductHasCorrectProperties()
    {
        // Act
        var products = Query.GetProducts();
        var first = products[0];

        // Assert
        Assert.Equal(1, first.Id);
        Assert.Equal("Product 1", first.Name);
        Assert.Equal("SKU1", first.Sku);
        Assert.Equal("Description 1", first.Description);
        Assert.Equal(1.0m, first.Price);
    }

    [Fact]
    public void Query_GetProducts_SecondProductHasCorrectProperties()
    {
        // Act
        var products = Query.GetProducts();
        var second = products[1];

        // Assert
        Assert.Equal(2, second.Id);
        Assert.Equal("Product 2", second.Name);
        Assert.Equal("SKU2", second.Sku);
        Assert.Equal("Description 2", second.Description);
        Assert.Equal(2.0m, second.Price);
    }

    [Fact]
    public void Query_GetProducts_ThirdProductHasCorrectProperties()
    {
        // Act
        var products = Query.GetProducts();
        var third = products[2];

        // Assert
        Assert.Equal(3, third.Id);
        Assert.Equal("Product 3", third.Name);
        Assert.Equal("SKU3", third.Sku);
        Assert.Equal("Description 3", third.Description);
        Assert.Equal(3.0m, third.Price);
    }

    [Fact]
    public void Query_GetProducts_FourthProductHasCorrectProperties()
    {
        // Act
        var products = Query.GetProducts();
        var fourth = products[3];

        // Assert
        Assert.Equal(4, fourth.Id);
        Assert.Equal("Product 4", fourth.Name);
        Assert.Equal("SKU4", fourth.Sku);
        Assert.Equal("Description 4", fourth.Description);
        Assert.Equal(4.0m, fourth.Price);
    }

    [Fact]
    public void Query_GetProducts_AllProductsHaveUniqueIds()
    {
        // Act
        var products = Query.GetProducts();
        var ids = products.Select(p => p.Id).ToList();

        // Assert
        Assert.Equal(ids.Count, ids.Distinct().Count());
    }

    [Fact]
    public void Query_GetProducts_AllProductsHaveUniqueSkus()
    {
        // Act
        var products = Query.GetProducts();
        var skus = products.Select(p => p.Sku).ToList();

        // Assert
        Assert.Equal(skus.Count, skus.Distinct().Count());
    }

    [Fact]
    public void Query_GetProducts_AllProductsHavePositivePrices()
    {
        // Act
        var products = Query.GetProducts();

        // Assert
        Assert.All(products, p => Assert.True(p.Price > 0));
    }

    [Fact]
    public void Query_GetProducts_AllProductsHaveNonNullNames()
    {
        // Act
        var products = Query.GetProducts();

        // Assert
        Assert.All(products, p => Assert.NotNull(p.Name));
    }

    [Fact]
    public void Query_GetProducts_AllProductsHaveNonNullDescriptions()
    {
        // Act
        var products = Query.GetProducts();

        // Assert
        Assert.All(products, p => Assert.NotNull(p.Description));
    }

    [Fact]
    public void Query_GetProducts_AllProductsHaveNonNullSkus()
    {
        // Act
        var products = Query.GetProducts();

        // Assert
        Assert.All(products, p => Assert.NotNull(p.Sku));
    }

    [Fact]
    public void Query_GetProducts_ReturnsNewArrayEachCall()
    {
        // Act
        var products1 = Query.GetProducts();
        var products2 = Query.GetProducts();

        // Assert
        Assert.NotSame(products1, products2);
    }

    [Fact]
    public void Query_GetProducts_IdsAreSequential()
    {
        // Act
        var products = Query.GetProducts();

        // Assert
        for (int i = 0; i < products.Length; i++)
        {
            Assert.Equal(i + 1, products[i].Id);
        }
    }

    [Fact]
    public void Query_GetProducts_PricesMatchIds()
    {
        // Act
        var products = Query.GetProducts();

        // Assert
        foreach (var product in products)
        {
            Assert.Equal((decimal)product.Id, product.Price);
        }
    }

    #endregion

    #region ProductOperations.GetProductById Tests

    [Fact]
    public void ProductOperations_GetProductById_ReturnsProductWithCorrectId()
    {
        // Act
        var product = ProductOperations.GetProductById(1);

        // Assert
        Assert.Equal(1, product.Id);
    }

    [Fact]
    public void ProductOperations_GetProductById_ReturnsProductWithGeneratedName()
    {
        // Act
        var product = ProductOperations.GetProductById(5);

        // Assert
        Assert.Equal("Product 5", product.Name);
    }

    [Fact]
    public void ProductOperations_GetProductById_ReturnsProductWithGeneratedSku()
    {
        // Act
        var product = ProductOperations.GetProductById(10);

        // Assert
        Assert.Equal("SKU10", product.Sku);
    }

    [Fact]
    public void ProductOperations_GetProductById_ReturnsProductWithGeneratedDescription()
    {
        // Act
        var product = ProductOperations.GetProductById(7);

        // Assert
        Assert.Equal("Description 7", product.Description);
    }

    [Fact]
    public void ProductOperations_GetProductById_PriceMatchesId()
    {
        // Act
        var product = ProductOperations.GetProductById(15);

        // Assert
        Assert.Equal(15m, product.Price);
    }

    [Fact]
    public void ProductOperations_GetProductById_WorksWithZeroId()
    {
        // Act
        var product = ProductOperations.GetProductById(0);

        // Assert
        Assert.Equal(0, product.Id);
        Assert.Equal("Product 0", product.Name);
        Assert.Equal("SKU0", product.Sku);
        Assert.Equal("Description 0", product.Description);
        Assert.Equal(0m, product.Price);
    }

    [Fact]
    public void ProductOperations_GetProductById_WorksWithNegativeId()
    {
        // Act
        var product = ProductOperations.GetProductById(-1);

        // Assert
        Assert.Equal(-1, product.Id);
        Assert.Equal("Product -1", product.Name);
        Assert.Equal("SKU-1", product.Sku);
        Assert.Equal("Description -1", product.Description);
        Assert.Equal(-1m, product.Price);
    }

    [Fact]
    public void ProductOperations_GetProductById_WorksWithLargeId()
    {
        // Act
        var product = ProductOperations.GetProductById(999999);

        // Assert
        Assert.Equal(999999, product.Id);
        Assert.Equal("Product 999999", product.Name);
        Assert.Equal("SKU999999", product.Sku);
    }

    [Fact]
    public void ProductOperations_GetProductById_ReturnsNewProductEachCall()
    {
        // Act
        var product1 = ProductOperations.GetProductById(1);
        var product2 = ProductOperations.GetProductById(1);

        // Assert - records are value-equal but different instances
        Assert.Equal(product1, product2);
    }

    [Fact]
    public void ProductOperations_GetProductById_DifferentIdsReturnDifferentProducts()
    {
        // Act
        var product1 = ProductOperations.GetProductById(1);
        var product2 = ProductOperations.GetProductById(2);

        // Assert
        Assert.NotEqual(product1, product2);
        Assert.NotEqual(product1.Id, product2.Id);
        Assert.NotEqual(product1.Name, product2.Name);
    }

    [Theory]
    [InlineData(1, "Product 1", "SKU1", "Description 1", 1)]
    [InlineData(2, "Product 2", "SKU2", "Description 2", 2)]
    [InlineData(100, "Product 100", "SKU100", "Description 100", 100)]
    public void ProductOperations_GetProductById_ReturnsExpectedProduct(
        int id, string expectedName, string expectedSku, string expectedDescription, decimal expectedPrice)
    {
        // Act
        var product = ProductOperations.GetProductById(id);

        // Assert
        Assert.Equal(id, product.Id);
        Assert.Equal(expectedName, product.Name);
        Assert.Equal(expectedSku, product.Sku);
        Assert.Equal(expectedDescription, product.Description);
        Assert.Equal(expectedPrice, product.Price);
    }

    #endregion

    #region Integration between Query and ProductOperations

    [Fact]
    public void GetProductById_CanLookupAllProductsFromGetProducts()
    {
        // Arrange
        var products = Query.GetProducts();

        // Act & Assert
        foreach (var product in products)
        {
            var lookedUpProduct = ProductOperations.GetProductById(product.Id);
            Assert.Equal(product.Id, lookedUpProduct.Id);
            Assert.Equal(product.Name, lookedUpProduct.Name);
            Assert.Equal(product.Sku, lookedUpProduct.Sku);
            Assert.Equal(product.Description, lookedUpProduct.Description);
            Assert.Equal(product.Price, lookedUpProduct.Price);
        }
    }

    #endregion
}
