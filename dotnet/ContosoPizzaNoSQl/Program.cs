using ContosoPizzaNoSQl.GraphQL;
using ContosoPizzaNoSQl.Services;
using ContosoPizzaNoSQl.Repositories;
using ContosoPizzaNoSQl.Configuration;
using ContosoPizzaNoSQl.GraphQL.Pizzas;
using ContosoPizzaNoSQl.Services.Interfaces;
using ContosoPizzaNoSQl.Repositories.Interfaces;
using ContosoPizzaNoSQl.GraphQL.Customers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//MongoDb Configuration;
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddScoped<IPizzaRepository, PizzaRepository>();
builder.Services.AddScoped<IPizzaService, PizzaService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();


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
    .AddErrorFilter<ExceptionFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGraphQL();  // This sets up /graphql endpoint

app.Run();
