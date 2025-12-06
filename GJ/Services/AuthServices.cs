using GJ.Models;
using Microsoft.Data.SqlClient;

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
                    id = reader.GetInt32("id"),
                    username = reader.GetString("username"),
                    cliente = reader.GetInt32("cliente"),
                    role = reader.GetInt32("role"),
                    Passe = reader.IsDBNull("Passe") ? null : reader.GetString("Passe"),
                    nome = reader.IsDBNull("nome") ? null : reader.GetString("nome"),
                    email = reader.IsDBNull("email") ? null : reader.GetString("email"),
                    telefoneMovel = reader.IsDBNull("telefoneMovel") ? null : reader.GetString("telefoneMovel"),
                    IPusual = reader.IsDBNull("IPusual") ? null : reader.GetString("IPusual")
                };
            }
        }
    }

    return null;
}
