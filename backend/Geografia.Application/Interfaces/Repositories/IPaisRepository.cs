using Geografia.Domain.Entities;

namespace Geografia.Application.Interfaces.Repositories;

public interface IPaisRepository
{
    Task<IReadOnlyList<Pais>> ObtenerTodosAsync();

    Task<Pais?> ObtenerPorIdAsync(int id);

    Task AgregarAsync(Pais pais);

    Task ActualizarAsync(Pais pais);

    Task EliminarAsync(Pais pais);

    Task<bool> ExisteNombreAsync(string nombre, int? excluirId = null);
    Task<bool> ExisteCodigoIso2Async(string codigoIso2, int? excluirId = null);
}