using Geografia.Domain.Entities;

namespace Geografia.Application.Interfaces.Repositories;

public interface IDepartamentoRepository
{
    Task<IReadOnlyList<Departamento>> ObtenerTodosAsync();

    Task<IReadOnlyList<Departamento>> ObtenerPorPaisAsync(
        int paisId);

    Task<Departamento?> ObtenerPorIdAsync(int id);

    Task AgregarAsync(Departamento departamento);

    Task ActualizarAsync(Departamento departamento);

    Task EliminarAsync(Departamento departamento);

    Task<bool> ExisteNombreAsync(
    int paisId,
    string nombre,
    int? excluirId = null);
}