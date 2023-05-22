using MultiTenantsExample.Core.Interfaces;
using MultiTenantsExample.Core.Settings;
using MultiTenantsExample.Infrastructure.Extensions;
using MultiTenantsExample.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ITenantService, TenantService>();
builder.Services.AddTransient<IProductService, ProductService>();

var config = builder.Configuration;
builder.Services.Configure<TenantSettings>(config.GetSection(nameof(TenantSettings)));
builder.Services.AddAndMigrateTenantDatabase(config);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
