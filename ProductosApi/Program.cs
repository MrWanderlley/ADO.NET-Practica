using DotNetEnv;
using MySqlConnector;
using ProductosApi.Data;
using ProductosApi.Endpoints;
using ProductosApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Carga las variables del archivo .env (datos de conexion a MySQL)
var archivoEnv = Path.Combine(builder.Environment.ContentRootPath, ".env");
if (File.Exists(archivoEnv))
{
    Env.Load(archivoEnv);
}

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Conexion y repositorio de productos
builder.Services.AddSingleton<ConexionDb>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Si MySQL falla, responde un 503 sencillo en lugar de un error largo
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (MySqlException)
    {
        context.Response.StatusCode = 503;
        await context.Response.WriteAsJsonAsync(new { message = "No se pudo comunicar con la base de datos" });
    }
});

// Activar CORS
app.UseCors("AllowAll");

// app.UseHttpsRedirection();

//Endpoints
app.MapProductoEndpoints();

app.Run();
