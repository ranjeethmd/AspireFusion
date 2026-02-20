var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

builder.AddGraphQL().AddTypes();

var app = builder.Build();

app.MapGraphQL();
app.MapHealthChecks("/health");

app.RunWithGraphQLCommands(args);