using DeveloperStore.Repository;      
using DeveloperStore.Services;
using Microsoft.EntityFrameworkCore;
using DeveloperStore.Domain.Interfaces;
using DeveloperStore.Services.Interfaces;
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

// Configurando AutoMapper para mapear entidades e DTOs.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());  

// Adicionando suporte a controllers (Web API).
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Aplica migrations automaticamente
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DeveloperStoreContext>();
    context.Database.Migrate();
}

// Configuração do pipeline HTTP (middleware).
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
