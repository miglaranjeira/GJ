using GJ.Models;
using Microsoft.Data.SqlClient;

namespace GJ.Services;

public class AuthService
{
    private readonly string _connectionString;

    public AuthService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("GJBase")
            ?? throw new InvalidOperationException("Connection string 'GJBase' not found.");
    }

    public async Task<Utilizador?> AuthenticateAsync(string username, string password)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new SqlCommand(
                "SELECT id, username, cliente, role, Passe, nome, email, telefoneMovel, IPusual " +
                "FROM Utilizadores WHERE username = @Username AND Passe = @Password",
                connection);

            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return new Utilizador
                    {
                        id = reader.GetInt32(0),
                        username = reader.GetString(1),
                        cliente = reader.GetInt32(2),
                        role = reader.GetInt32(3),
                        Passe = reader.IsDBNull(4) ? null : reader.GetString(4),
                        nome = reader.IsDBNull(5) ? null : reader.GetString(5),
                        email = reader.IsDBNull(6) ? null : reader.GetString(6),
                        telefoneMovel = reader.IsDBNull(7) ? null : reader.GetString(7),
                        IPusual = reader.IsDBNull(8) ? null : reader.GetString(8)
                    };
                }
            }
        }

        return null;
    }
}
