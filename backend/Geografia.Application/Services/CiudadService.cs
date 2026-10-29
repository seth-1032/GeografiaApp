
using Geografia.Application.DTOs;
using Geografia.Application.Interfaces.Repositories;
using Geografia.Application.Interfaces.Services;
using Geografia.Domain.Entities;

namespace Geografia.Application.Services;

public class CiudadService : ICiudadService
{
    private readonly ICiudadRepository _ciudadRepository;
    private readonly IDepartamentoRepository _departamentoRepository;

    public CiudadService(
        ICiudadRepository ciudadRepository,
        IDepartamentoRepository departamentoRepository)
    {
        _ciudadRepository = ciudadRepository;
        _departamentoRepository = departamentoRepository;
    }

    public async Task<IReadOnlyList<CiudadDto>> ObtenerTodosAsync()
    {
        var ciudades = await _ciudadRepository.ObtenerTodosAsync();

        return ciudades
            .Select(MapearDto)
            .ToList();
    }

    public async Task<IReadOnlyList<CiudadDto>> ObtenerPorDepartamentoAsync(
        int departamentoId)
    {
        if (departamentoId <= 0)
            return Array.Empty<CiudadDto>();

        var ciudades =
            await _ciudadRepository.ObtenerPorDepartamentoAsync(
                departamentoId);

        return ciudades
            .Select(MapearDto)
            .ToList();
    }

    public async Task<CiudadDto?> ObtenerPorIdAsync(int id)
    {
        if (id <= 0)
            return null;

        var ciudad = await _ciudadRepository.ObtenerPorIdAsync(id);

        return ciudad is null
            ? null
            : MapearDto(ciudad);
    }

    public async Task<CiudadDto> CrearAsync(GuardarCiudadDto dto)
    {
        Validar(dto);

        var departamento =
            await _departamentoRepository.ObtenerPorIdAsync(
                dto.DepartamentoId);

        if (departamento is null)
            throw new ArgumentException(
                "El departamento seleccionado no existe.");

        var nombre = dto.Nombre.Trim();

        if (await _ciudadRepository.ExisteNombreAsync(
                dto.DepartamentoId, nombre))
        {
            throw new ArgumentException(
                "Ya existe una ciudad con ese nombre en el departamento seleccionado.");
        }

        var ciudad = new Ciudad
        {
            DepartamentoId = dto.DepartamentoId,
            Nombre = dto.Nombre.Trim(),
            Activo = true,
            FechaCreacion = DateTime.Now
        };

        await _ciudadRepository.AgregarAsync(ciudad);

        return MapearDto(ciudad);
    }

    public async Task<CiudadDto?> ActualizarAsync(
        int id,
        GuardarCiudadDto dto)
    {
        if (id <= 0)
            return null;

        Validar(dto);

        var ciudad = await _ciudadRepository.ObtenerPorIdAsync(id);

        if (ciudad is null)
            return null;

        var departamento =
            await _departamentoRepository.ObtenerPorIdAsync(
                dto.DepartamentoId);

        if (departamento is null)
            throw new ArgumentException(
                "El departamento seleccionado no existe.");

        ciudad.DepartamentoId = dto.DepartamentoId;
        ciudad.Nombre = dto.Nombre.Trim();
        ciudad.Activo = dto.Activo;
        ciudad.FechaModificacion = DateTime.Now;

        await _ciudadRepository.ActualizarAsync(ciudad);

        return MapearDto(ciudad);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
            return false;

        var ciudad = await _ciudadRepository.ObtenerPorIdAsync(id);

        if (ciudad is null)
            return false;

        await _ciudadRepository.EliminarAsync(ciudad);

        return true;
    }

    private static void Validar(GuardarCiudadDto dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        if (dto.DepartamentoId <= 0)
            throw new ArgumentException(
                "Debes seleccionar un departamento válido.");

        if (string.IsNullOrWhiteSpace(dto.Nombre))
            throw new ArgumentException(
                "El nombre de la ciudad es obligatorio.");

        if (dto.Nombre.Trim().Length > 100)
            throw new ArgumentException(
                "El nombre no puede superar los 100 caracteres.");
    }

    private static CiudadDto MapearDto(Ciudad ciudad)
    {
        return new CiudadDto
        {
            CiudadId = ciudad.CiudadId,
            DepartamentoId = ciudad.DepartamentoId,
            Nombre = ciudad.Nombre,
            Activo = ciudad.Activo
        };
    }
}