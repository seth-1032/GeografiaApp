namespace Geografia.Application.DTOs;

public class GuardarPaisDto
{
    public string Nombre { get; set; } = string.Empty;

    public string? CodigoIso2 { get; set; }

    public bool Activo { get; set; }
}