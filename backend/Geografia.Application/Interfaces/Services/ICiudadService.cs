using Geografia.Application.DTOs;

namespace Geografia.Application.Interfaces.Services;

public interface ICiudadService
{
    Task<IReadOnlyList<CiudadDto>> ObtenerTodosAsync();

    Task<IReadOnlyList<CiudadDto>> ObtenerPorDepartamentoAsync(
        int departamentoId);

    Task<CiudadDto?> ObtenerPorIdAsync(int id);

    Task<CiudadDto> CrearAsync(
        GuardarCiudadDto dto);

    Task<CiudadDto?> ActualizarAsync(
        int id,
        GuardarCiudadDto dto);

    Task<bool> EliminarAsync(int id);

}