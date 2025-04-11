using DeveloperStore.Repository;      // Namespaces hipotéticos dos projetos
using DeveloperStore.Services;
using DeveloperStore.Services.DTOs;   // Supondo que os DTOs estejam neste namespace
using DeveloperStore.Repository.Models; // Supondo que as entidades Sale e SaleItem estejam aqui
using Microsoft.EntityFrameworkCore;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// String de conexão (pode vir de appsettings.json). Exemplo para SQLite:
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=DeveloperStore.db";

// Registrando o DbContext do Entity Framework Core (usando SQLite neste exemplo).
builder.Services.AddDbContext<DeveloperStoreContext>(options =>
    options.UseSqlite(connectionString));
// Para usar SQL Server, basta trocar para UseSqlServer(connectionString)&#8203;:contentReference[oaicite:0]{index=0}.

// Registrando serviços e repositórios na DI.
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<ISaleService, SaleService>();
// (Registrar outros repositórios/serviços conforme existirem, p.ex. IProductService, IProductRepository, etc.)

// Configurando AutoMapper para mapear entidades e DTOs.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());  // Busca Profiles de mapeamento automaticamente&#8203;:contentReference[oaicite:1]{index=1}.

// Adicionando suporte a controllers (Web API).
builder.Services.AddControllers();

// (Opcional: adicionar documentação Swagger em ambiente de desenvolvimento, se desejado)
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuração do pipeline HTTP (middleware).
if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
