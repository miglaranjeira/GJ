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
                        id = reader.GetInt32(reader.GetOrdinal("id")),
                        username = reader.GetString(reader.GetOrdinal("username")),
                        cliente = reader.GetInt32(reader.GetOrdinal("cliente")),
                        role = reader.GetInt32(reader.GetOrdinal("role")),
                        Passe = reader.IsDBNull(reader.GetOrdinal("Passe")) ? null : reader.GetString(reader.GetOrdinal("Passe")),
                        nome = reader.IsDBNull(reader.GetOrdinal("nome")) ? null : reader.GetString(reader.GetOrdinal("nome")),
                        email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email")),
                        telefoneMovel = reader.IsDBNull(reader.GetOrdinal("telefoneMovel")) ? null : reader.GetString(reader.GetOrdinal("telefoneMovel")),
                        IPusual = reader.IsDBNull(reader.GetOrdinal("IPusual")) ? null : reader.GetString(reader.GetOrdinal("IPusual"))
                    };
                }
            }
        }

        return null;
    }
}
