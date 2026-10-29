using Geografia.Application.Interfaces.Repositories;
using Geografia.Application.Interfaces.Services;
using Geografia.Application.Services;
using Geografia.Infrastructure.Data;
using Geografia.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<GeografiaDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("GeografiaDb")));

builder.Services.AddScoped<IPaisRepository, PaisRepository>();
builder.Services.AddScoped<IDepartamentoRepository, DepartamentoRepository>();
builder.Services.AddScoped<ICiudadRepository, CiudadRepository>();

builder.Services.AddScoped<IPaisService, PaisService>();
builder.Services.AddScoped<IDepartamentoService, DepartamentoService>();
builder.Services.AddScoped<ICiudadService, CiudadService>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.UseCors("AngularFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();