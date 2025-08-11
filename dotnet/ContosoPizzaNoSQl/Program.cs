using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ContosoPizzaNoSQl.GraphQL;
using ContosoPizzaNoSQl.Services;
using ContosoPizzaNoSQl.Repositories;
using ContosoPizzaNoSQl.Configuration;
using ContosoPizzaNoSQl.GraphQL.Pizzas;
using ContosoPizzaNoSQl.Services.Interfaces;
using ContosoPizzaNoSQl.Repositories.Interfaces;
using ContosoPizzaNoSQl.GraphQL.Customers;
using ContosoPizzaNoSQl.GraphQL.Orders;
using ContosoPizzaNoSQl.GraphQL.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//MongoDb Configuration;
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddScoped<IPizzaRepository, PizzaRepository>();
builder.Services.AddScoped<IPizzaService, PizzaService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtService>();


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
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
        };
    });


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// GraphQL Configuration
builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddTypeExtension<PizzaQueries>()
    .AddTypeExtension<PizzaMutations>()
    .AddTypeExtension<CustomerQueries>()
    .AddTypeExtension<CustomerMutations>()
    .AddTypeExtension<OrderMutations>()
    .AddTypeExtension<OrderQueries>()
    .AddTypeExtension<AuthMutations>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();  // This sets up /graphql endpoint

app.Run();
