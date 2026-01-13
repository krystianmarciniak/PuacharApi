using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PucharApi.Application.Auth;
using PucharApi.GraphQL;
using PucharApi.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DB
var cs = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<PucharDbContext>(opt => opt.UseSqlite(cs));

// JWT
var jwt = builder.Configuration.GetSection("Jwt");
var key = jwt["Key"]!;

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ClockSkew = TimeSpan.FromSeconds(30),
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddScoped<JwtTokenService>();

// GraphQL
builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();
//.ModifyRequestOptions(o => o.IncludeExceptionDetails = true)

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL("/graphql");
app.MapGet("/", () => "PucharApi działa. Wejdź na /graphql");

app.Run();
