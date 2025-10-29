using ApiPedidos.Application.Interfaces;
using ApiPedidos.Application.Services;
using ApiPedidos.Domain.Entities;
using ApiPedidos.Domain.Enums;
using ApiPedidos.Infrastructure.Persistence;
using ApiPedidos.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de Pedidos",
        Version = "v1",
        Description = "API para controle de pedidos e produtos",
        Contact = new OpenApiContact
        {
            Name = "Wyllian Alison Martinez",
        }
    });

   
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("PedidosDB"));


builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Pedidos v1");
        options.RoutePrefix = string.Empty; 
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    SeedDatabase(context);
}

app.Run();

static void SeedDatabase(AppDbContext context)
{
    if (!context.Produtos.Any())
    {
        context.Produtos.AddRange(
    new Produto("Arroz 5kg", 25m, 40m, 100m,
        UnidadeMedida.Quilograma, true, DateTime.UtcNow),

    new Produto("Feijão 1kg", 8m, 15m, 200m,
        UnidadeMedida.Quilograma, true, DateTime.UtcNow),

    new Produto("Refrigerante 2L", 4m, 8m, 150m,
        UnidadeMedida.Litro, true, DateTime.UtcNow)
    );

        context.SaveChanges();

    }
}

