using Geografia.Domain.Entities;

namespace Geografia.Application.Interfaces.Repositories;

public interface ICiudadRepository
{
    Task<IReadOnlyList<Ciudad>> ObtenerTodosAsync();

    Task<IReadOnlyList<Ciudad>> ObtenerPorDepartamentoAsync(
        int departamentoId);

    Task<Ciudad?> ObtenerPorIdAsync(int id);

    Task AgregarAsync(Ciudad ciudad);

    Task ActualizarAsync(Ciudad ciudad);

    Task EliminarAsync(Ciudad ciudad);

    Task<bool> ExisteNombreAsync(
    int departamentoId,
    string nombre,
    int? excluirId = null);
}