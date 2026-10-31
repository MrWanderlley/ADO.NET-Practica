using ProductosApi.Models;
using ProductosApi.Repositories;

namespace ProductosApi.Endpoints;

public static class ProductoEndpoints
{
    public static void MapProductoEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/productos").WithTags("Productos");

        //GET /productos
        group.MapGet("/", (IProductoRepository repo) => {
            return Results.Ok(repo.ObtenerTodos());
        });

        //GET /productos/buscar?termino=monster
        group.MapGet("/buscar", (string termino, IProductoRepository repo) => {
            return Results.Ok(repo.Buscar(termino));
        });

        //GET /productos/{id}
        group.MapGet("/{id:int}", (int id, IProductoRepository repo) => {
            var producto = repo.ObtenerPorId(id);
            return producto is null
                ? Results.NotFound(new { message = "Producto no encontrado" })
                : Results.Ok(producto);
        });

        //POST /productos
        group.MapPost("/", (Producto producto, IProductoRepository repo) => {
            if (!EsValido(producto)) {
                return Results.BadRequest(new { message = "Datos del producto no validos" });
            }

            producto.Id = repo.Insertar(producto);
            return Results.Created($"/productos/{producto.Id}", producto);
        });

        //PUT /productos/{id}
        group.MapPut("/{id:int}", (int id, Producto producto, IProductoRepository repo) => {
            if (!EsValido(producto)) {
                return Results.BadRequest(new { message = "Datos del producto no validos" });
            }

            producto.Id = id;
            return repo.Actualizar(producto)
                ? Results.NoContent()
                : Results.NotFound(new { message = "Producto no encontrado" });
        });

        //DELETE /productos/{id}
        group.MapDelete("/{id:int}", (int id, IProductoRepository repo) => {
            return repo.Eliminar(id)
                ? Results.NoContent()
                : Results.NotFound(new { message = "Producto no encontrado" });
        });
    }

    // Validacion basica (los limites coinciden con las columnas de la tabla)
    private static bool EsValido(Producto p) =>
        !string.IsNullOrWhiteSpace(p.Nombre)
        && p.Nombre.Length <= 100
        && (p.Descripcion is null || p.Descripcion.Length <= 255)
        && p.Precio >= 0
        && p.Stock >= 0;
}
