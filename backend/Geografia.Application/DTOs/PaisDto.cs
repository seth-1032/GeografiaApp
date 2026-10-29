namespace Geografia.Application.DTOs;

public class PaisDto
{
    public int PaisId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? CodigoIso2 { get; set; }

    public bool Activo { get; set; }
}