namespace Geografia.Application.DTOs;

public class GuardarDepartamentoDto
{
    public int PaisId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public bool Activo { get; set; }
}