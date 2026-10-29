namespace Geografia.Domain.Entities;

public class Departamento
{
    public int DepartamentoId { get; set; }

    public int PaisId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Pais Pais { get; set; } = null!;

    public ICollection<Ciudad> Ciudades { get; set; }
        = new List<Ciudad>();
}