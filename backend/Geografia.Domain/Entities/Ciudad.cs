namespace Geografia.Domain.Entities;

public class Ciudad
{
    public int CiudadId { get; set; }

    public int DepartamentoId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Departamento Departamento { get; set; } = null!;
}