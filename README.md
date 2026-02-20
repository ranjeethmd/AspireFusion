# AspireFusion

A distributed GraphQL application built with .NET Aspire and HotChocolate Fusion, demonstrating federated GraphQL architecture with multiple subgraphs.

## Overview

AspireFusion is a microservices-based GraphQL API that uses HotChocolate Fusion to compose multiple GraphQL subgraphs into a unified gateway. The solution leverages .NET Aspire for orchestration, service discovery, health checks, and observability.

## Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                    Aspire AppHost                           │
│              (Orchestration & Service Discovery)            │
└─────────────────────────────────────────────────────────────┘
                              │
              ┌───────────────┼───────────────┐
              ▼               ▼               ▼
       ┌───────────┐   ┌───────────┐   ┌───────────┐
       │  Gateway  │   │  Orders   │   │ Products  │
       │ (Fusion)  │◄──│ Subgraph  │   │ Subgraph  │
       └───────────┘   └───────────┘   └───────────┘
              │               │               │
              └───────────────┴───────────────┘
                    Service Discovery
```

## Projects

### AspireFusion.AppHost

The .NET Aspire orchestrator that manages all services in the distributed application.

**Key Features:**
- Orchestrates all microservices
- Configures HotChocolate Fusion gateway with subgraphs
- Provides service discovery for inter-service communication
- Health check monitoring for all services

**Configuration (AppHost.cs):**
```csharp
var builder = DistributedApplication.CreateBuilder(args);

var orderSubGraph = builder.AddProject<Projects.quick_start_Orders>("orderSubGraph")
    .WithHttpHealthCheck("/health");

var productSubGraph = builder.AddProject<Projects.quick_start_Products>("productSubGraph")
    .WithHttpHealthCheck("/health");

builder.AddFusionGateway<Projects.quick_start_Gateway>("gateway")
    .WithSubgraph(orderSubGraph)
    .WithSubgraph(productSubGraph);

builder.Build().Compose().Run();
```

### AspireFusion.ServiceDefaults

Shared project containing common Aspire service configurations including:
- OpenTelemetry instrumentation (tracing, metrics, logging)
- Health check endpoints (`/health`, `/alive`)
- Service discovery configuration
- HTTP client resilience policies

### quick-start.Gateway

The HotChocolate Fusion gateway that composes the Orders and Products subgraphs into a unified GraphQL API.

**Features:**
- Federated GraphQL schema composition
- Automatic service discovery via Aspire
- Query plan visualization (development only)
- Health check endpoints

**Endpoint:** `/graphql`

### quick-start.Orders

GraphQL subgraph service for order management.

**Schema Types:**

| Type | Description |
|------|-------------|
| `Order` | Order with id, name, description, and line items |
| `LineItem` | Line item with id, quantity, and product reference |

**Queries:**
- `orders: [Order!]!` - Returns all orders

**Entity Resolution:**
- `LineItem.product` resolves to Product entity from Products subgraph

### quick-start.Products

GraphQL subgraph service for product catalog.

**Schema Types:**

| Type | Description |
|------|-------------|
| `Product` | Product with id, name, sku, description, and price |

**Queries:**
- `products: [Product!]!` - Returns all products
- `productById(id: Int!): Product!` - Lookup product by ID (internal, used for federation)

### AspireFusion.Tests

Integration tests using Aspire.Hosting.Testing and xUnit.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [.NET Aspire workload](https://learn.microsoft.com/dotnet/aspire/fundamentals/setup-tooling)

```bash
dotnet workload install aspire
```

## Getting Started

### Running the Application

1. Clone the repository
2. Navigate to the solution directory
3. Run the AppHost project:

```bash
dotnet run --project AspireFusion.AppHost
```

4. Open the Aspire Dashboard (URL shown in console output)
5. Access the GraphQL Gateway at the gateway service URL + `/graphql`

### Example Queries

**Get all orders with product details:**
```graphql
query {
  orders {
    id
    name
    description
    items {
      id
      quantity
      product {
        id
        name
        sku
        price
      }
    }
  }
}
```

**Get all products:**
```graphql
query {
  products {
    id
    name
    sku
    description
    price
  }
}
```

### Running Tests

```bash
dotnet test
```

## Project Structure

```
AspireFusion/
├── AspireFusion.sln
├── README.md
├── AspireFusion.AppHost/           # Aspire orchestrator
│   ├── AppHost.cs
│   └── AspireFusion.AppHost.csproj
├── AspireFusion.ServiceDefaults/   # Shared service configurations
│   ├── Extensions.cs
│   └── AspireFusion.ServiceDefaults.csproj
├── AspireFusion.Tests/             # Integration tests
│   ├── WebTests.cs
│   └── AspireFusion.Tests.csproj
├── quick-start.Gateway/            # Fusion Gateway
│   ├── Program.cs
│   ├── gateway.fgp                 # Fusion gateway package
│   └── quick-start.Gateway.csproj
├── quick-start.Orders/             # Orders subgraph
│   ├── Program.cs
│   ├── Types/
│   │   ├── Order.cs
│   │   ├── LineItem.cs
│   │   └── Query.cs
│   └── quick-start.Orders.csproj
└── quick-start.Products/           # Products subgraph
    ├── Program.cs
    ├── Types/
    │   ├── Product.cs
    │   └── Query.cs
    └── quick-start.Products.csproj
```

## Technology Stack

| Technology | Version | Purpose |
|------------|---------|---------|
| .NET | 10.0 | Runtime |
| .NET Aspire | 13.1.1 | Orchestration & Observability |
| HotChocolate | 15.1.12 | GraphQL Server |
| HotChocolate.Fusion | 15.1.12 | GraphQL Federation |
| HotChocolate.Fusion.Aspire | 15.1.12 | Aspire Integration |
| xUnit | 3.0.1 | Testing |

## Key Concepts

### GraphQL Federation with HotChocolate Fusion

HotChocolate Fusion enables building a distributed GraphQL architecture where:
- Each subgraph owns and exposes its domain types
- The gateway composes subgraphs into a unified schema
- Entity references are resolved across subgraph boundaries

### Entity Resolution

The `LineItem` type in the Orders subgraph references `Product` from the Products subgraph:

```csharp
// Orders subgraph - LineItem.cs
public static Product GetProduct([Parent] LineItem lineItem)
    => new(lineItem.ProductId);
```

The Products subgraph provides a lookup resolver:

```csharp
// Products subgraph - Query.cs
[Query]
[Lookup]
[Internal]
public static Product GetProductById(int id) => /* ... */;
```

### Aspire Service Discovery

Services communicate using Aspire's service discovery instead of hardcoded URLs:

```csharp
// Gateway configuration
.ConfigureHttpClient("Orders", client =>
    client.BaseAddress = new Uri("http://orderSubGraph"))
.ConfigureHttpClient("Products", client =>
    client.BaseAddress = new Uri("http://productSubGraph"));
```

## Health Checks

All services expose health check endpoints:
- `/health` - Full health check
- `/alive` - Liveness probe

## Observability

The solution includes built-in observability through Aspire:
- **Distributed Tracing** - OpenTelemetry traces across all services
- **Metrics** - ASP.NET Core, HTTP client, and runtime metrics
- **Logging** - Structured logging with OpenTelemetry export
- **Dashboard** - Aspire Dashboard for monitoring all services

## License

This project is provided as a sample/demo application.
