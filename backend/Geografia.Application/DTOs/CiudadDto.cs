namespace Geografia.Application.DTOs;

public class CiudadDto
{
    public int CiudadId { get; set; }

    public int DepartamentoId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public bool Activo { get; set; }
}