using GJ.Models;

namespace GJ.Services;

public interface ITerceiroService
{
    Task<List<Terceiro>> GetTerceirosAsync();
    Task<List<Terceiro>> SearchTerceirosAsync(string searchTerm);
    Task<Terceiro?> GetTerceiroByIdAsync(int id);
    Task<bool> CreateTerceiroAsync(Terceiro terceiro);
    Task<bool> UpdateTerceiroAsync(Terceiro terceiro);
    Task<bool> DeleteTerceiroAsync(int id);
}