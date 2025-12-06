namespace GJ.Models;

public class Terceiro
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Nif { get; set; }
    public string? Morada { get; set; }
    public string? CPostal { get; set; }
    public string? Telefone { get; set; }
    public string? Email { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.Now;
}