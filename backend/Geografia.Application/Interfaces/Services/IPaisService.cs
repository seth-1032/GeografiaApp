using Geografia.Application.DTOs;

namespace Geografia.Application.Interfaces.Services;

public interface IPaisService
{
    Task<IReadOnlyList<PaisDto>> ObtenerTodosAsync();

    Task<PaisDto?> ObtenerPorIdAsync(int id);

    Task<PaisDto> CrearAsync(GuardarPaisDto dto);

    Task<PaisDto?> ActualizarAsync(
        int id,
        GuardarPaisDto dto);

    Task<bool> EliminarAsync(int id);
}