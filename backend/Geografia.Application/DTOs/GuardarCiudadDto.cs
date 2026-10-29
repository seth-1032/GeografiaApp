namespace Geografia.Application.DTOs;

public class GuardarCiudadDto
{
    public int DepartamentoId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public bool Activo { get; set; }
}