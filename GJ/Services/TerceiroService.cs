using GJ.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GJ.Services;

public class TerceiroService : ITerceiroService
{
    private readonly string _connectionString;

    public TerceiroService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("GJBase")
            ?? throw new InvalidOperationException("Connection string 'GJBase' not found.");
    }

    public async Task<List<Terceiro>> GetTerceirosAsync()
    {
        var terceiros = new List<Terceiro>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new SqlCommand(
                "SELECT Id, Nome, Nif, Morada, CPostal, Telefone, Email, DataCriacao " +
                "FROM Terceiros ORDER BY Nome",
                connection);

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    terceiros.Add(new Terceiro
                    {
                        Id = reader.GetInt32("Id"),
                        Nome = reader.GetString("Nome"),
                        Nif = reader.IsDBNull("Nif") ? null : reader.GetString("Nif"),
                        Morada = reader.IsDBNull("Morada") ? null : reader.GetString("Morada"),
                        CPostal = reader.IsDBNull("CPostal") ? null : reader.GetString("CPostal"),
                        Telefone = reader.IsDBNull("Telefone") ? null : reader.GetString("Telefone"),
                        Email = reader.IsDBNull("Email") ? null : reader.GetString("Email"),
                        DataCriacao = reader.GetDateTime("DataCriacao")
                    });
                }
            }
        }

        return terceiros;
    }

    public async Task<Terceiro?> GetTerceiroByIdAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new SqlCommand(
                "SELECT Id, Nome, Nif, Morada, CPostal, Telefone, Email, DataCriacao " +
                "FROM Terceiros WHERE Id = @Id",
                connection);

            command.Parameters.AddWithValue("@Id", id);

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return new Terceiro
                    {
                        Id = reader.GetInt32("Id"),
                        Nome = reader.GetString("Nome"),
                        Nif = reader.IsDBNull("Nif") ? null : reader.GetString("Nif"),
                        Morada = reader.IsDBNull("Morada") ? null : reader.GetString("Morada"),
                        CPostal = reader.IsDBNull("CPostal") ? null : reader.GetString("CPostal"),
                        Telefone = reader.IsDBNull("Telefone") ? null : reader.GetString("Telefone"),
                        Email = reader.IsDBNull("Email") ? null : reader.GetString("Email"),
                        DataCriacao = reader.GetDateTime("DataCriacao")
                    };
                }
            }
        }

        return null;
    }

    public async Task<bool> CreateTerceiroAsync(Terceiro terceiro)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO Terceiros (Nome, Nif, Morada, CPostal, Telefone, Email, DataCriacao) " +
                "VALUES (@Nome, @Nif, @Morada, @CPostal, @Telefone, @Email, @DataCriacao)",
                connection);

            command.Parameters.AddWithValue("@Nome", terceiro.Nome);
            command.Parameters.AddWithValue("@Nif", (object?)terceiro.Nif ?? DBNull.Value);
            command.Parameters.AddWithValue("@Morada", (object?)terceiro.Morada ?? DBNull.Value);
            command.Parameters.AddWithValue("@CPostal", (object?)terceiro.CPostal ?? DBNull.Value);
            command.Parameters.AddWithValue("@Telefone", (object?)terceiro.Telefone ?? DBNull.Value);
            command.Parameters.AddWithValue("@Email", (object?)terceiro.Email ?? DBNull.Value);
            command.Parameters.AddWithValue("@DataCriacao", terceiro.DataCriacao);

            var result = await command.ExecuteNonQueryAsync();
            return result > 0;
        }
    }

    public async Task<bool> UpdateTerceiroAsync(Terceiro terceiro)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new SqlCommand(
                "UPDATE Terceiros SET Nome = @Nome, Nif = @Nif, Morada = @Morada, " +
                "CPostal = @CPostal, Telefone = @Telefone, Email = @Email " +
                "WHERE Id = @Id",
                connection);

            command.Parameters.AddWithValue("@Id", terceiro.Id);
            command.Parameters.AddWithValue("@Nome", terceiro.Nome);
            command.Parameters.AddWithValue("@Nif", (object?)terceiro.Nif ?? DBNull.Value);
            command.Parameters.AddWithValue("@Morada", (object?)terceiro.Morada ?? DBNull.Value);
            command.Parameters.AddWithValue("@CPostal", (object?)terceiro.CPostal ?? DBNull.Value);
            command.Parameters.AddWithValue("@Telefone", (object?)terceiro.Telefone ?? DBNull.Value);
            command.Parameters.AddWithValue("@Email", (object?)terceiro.Email ?? DBNull.Value);

            var result = await command.ExecuteNonQueryAsync();
            return result > 0;
        }
    }

    public async Task<bool> DeleteTerceiroAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new SqlCommand(
                "DELETE FROM Terceiros WHERE Id = @Id",
                connection);

            command.Parameters.AddWithValue("@Id", id);

            var result = await command.ExecuteNonQueryAsync();
            return result > 0;
        }
    }

    public async Task<List<Terceiro>> SearchTerceirosAsync(string searchTerm)
    {
        var terceiros = new List<Terceiro>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new SqlCommand(
                "SELECT Id, Nome, Nif, Morada, CPostal, Telefone, Email, DataCriacao " +
                "FROM Terceiros WHERE " +
                "(Nome LIKE @SearchTerm OR Nif LIKE @SearchTerm OR Email LIKE @SearchTerm) " +
                "ORDER BY Nome",
                connection);

            command.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    terceiros.Add(new Terceiro
                    {
                        Id = reader.GetInt32("Id"),
                        Nome = reader.GetString("Nome"),
                        Nif = reader.IsDBNull("Nif") ? null : reader.GetString("Nif"),
                        Morada = reader.IsDBNull("Morada") ? null : reader.GetString("Morada"),
                        CPostal = reader.IsDBNull("CPostal") ? null : reader.GetString("CPostal"),
                        Telefone = reader.IsDBNull("Telefone") ? null : reader.GetString("Telefone"),
                        Email = reader.IsDBNull("Email") ? null : reader.GetString("Email"),
                        DataCriacao = reader.GetDateTime("DataCriacao")
                    });
                }
            }
        }

        return terceiros;
    }
}