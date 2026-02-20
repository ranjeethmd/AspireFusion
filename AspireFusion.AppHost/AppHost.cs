var builder = DistributedApplication.CreateBuilder(args);

var orderSubGraph = builder.AddProject<Projects.quick_start_Orders>("orderSubGraph")
    .WithHttpHealthCheck("/health");

orderSubGraph.WithUrl("/graphql");

var productSubGraph = builder.AddProject<Projects.quick_start_Products>("productSubGraph")
    .WithHttpHealthCheck("/health");

productSubGraph.WithUrl("/graphql");

builder.AddFusionGateway<Projects.quick_start_Gateway>("gateway")
   .WithUrl("/graphql")
   .WithSubgraph(orderSubGraph)
   .WithSubgraph(productSubGraph);


builder.Build().Compose().Run();
