namespace Geografia.Domain.Entities;

public class Pais
{
    public int PaisId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? CodigoIso2 { get; set; }

    public bool Activo { get; set; } = true;

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public ICollection<Departamento> Departamentos { get; set; }
        = new List<Departamento>();
}