namespace GJ.Models;

public class Utilizador
{
    public int id { get; set; }
    public string username { get; set; } = string.Empty;
    public int cliente { get; set; }
    public int role { get; set; }
    public string? Passe { get; set; }
    public string? nome { get; set; }
    public string? email { get; set; }
    public string? telefoneMovel { get; set; }
    public string? IPusual { get; set; }
}