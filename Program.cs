using PuacharApi.GraphQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>();

var app = builder.Build();

app.MapGraphQL("/graphql");

app.MapGet("/", () => "PuacharApi działa. Wejdź na /graphql");

app.Run();
