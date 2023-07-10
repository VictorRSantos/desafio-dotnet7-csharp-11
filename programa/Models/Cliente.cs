namespace Programa.Models;

public record Cliente
{
    public required string Id { get; set; }
    public string Nome { get; set; } = default!;
    public string Telefone { get; set; } = default!;
    public string Email { get; set; } = default!;

}
