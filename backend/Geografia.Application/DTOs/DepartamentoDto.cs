namespace Geografia.Application.DTOs;

public class DepartamentoDto
{
    public int DepartamentoId { get; set; }

    public int PaisId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public bool Activo { get; set; }
}