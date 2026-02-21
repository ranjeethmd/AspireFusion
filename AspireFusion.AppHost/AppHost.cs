var builder = DistributedApplication.CreateBuilder(args);

var orderSubGraph = builder.AddProject<Projects.quick_start_Orders>("orderSubGraph")
    .WithHttpHealthCheck("/health");


var productSubGraph = builder.AddProject<Projects.quick_start_Products>("productSubGraph")
    .WithHttpHealthCheck("/health");



builder.AddFusionGateway<Projects.quick_start_Gateway>("gateway")
   .WithExternalHttpEndpoints()
   .WithSubgraph(orderSubGraph)
   .WithSubgraph(productSubGraph);


builder.Build().Compose().Run();
