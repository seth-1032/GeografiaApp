using Geografia.Application.DTOs;

namespace Geografia.Application.Interfaces.Services;

public interface IDepartamentoService
{
    Task<IReadOnlyList<DepartamentoDto>> ObtenerTodosAsync();

    Task<IReadOnlyList<DepartamentoDto>> ObtenerPorPaisAsync(
        int paisId);

    Task<DepartamentoDto?> ObtenerPorIdAsync(int id);

    Task<DepartamentoDto> CrearAsync(
        GuardarDepartamentoDto dto);

    Task<DepartamentoDto?> ActualizarAsync(
        int id,
        GuardarDepartamentoDto dto);

    Task<bool> EliminarAsync(int id);
}