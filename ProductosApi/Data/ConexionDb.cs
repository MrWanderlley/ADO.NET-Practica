using MySqlConnector;

namespace ProductosApi.Data;

public class ConexionDb
{
    private readonly string _cadena;

    // Los datos salen de las variables del archivo .env (ver Program.cs)
    public ConexionDb()
    {
        var cadena = new MySqlConnectionStringBuilder {
            Server = Leer("DB_HOST"),
            Port = uint.Parse(Leer("DB_PORT")),
            Database = Leer("DB_NAME"),
            UserID = Leer("DB_USER"),
            Password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? string.Empty
        };
        
        _cadena = cadena.ConnectionString;
    }

    private static string Leer(string nombre) =>
        Environment.GetEnvironmentVariable(nombre)
        ?? throw new InvalidOperationException($"Falta la variable {nombre} en el archivo .env");

    public MySqlConnection CrearConexion() => new MySqlConnection(_cadena);
}
