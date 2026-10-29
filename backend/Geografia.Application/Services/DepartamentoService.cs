
using Geografia.Application.DTOs;
using Geografia.Application.Interfaces.Repositories;
using Geografia.Application.Interfaces.Services;
using Geografia.Domain.Entities;

namespace Geografia.Application.Services;

public class DepartamentoService : IDepartamentoService
{
    private readonly IDepartamentoRepository _departamentoRepository;
    private readonly IPaisRepository _paisRepository;

    public DepartamentoService(
        IDepartamentoRepository departamentoRepository,
        IPaisRepository paisRepository)
    {
        _departamentoRepository = departamentoRepository;
        _paisRepository = paisRepository;
    }

    public async Task<IReadOnlyList<DepartamentoDto>> ObtenerTodosAsync()
    {
        var departamentos = await _departamentoRepository.ObtenerTodosAsync();

        return departamentos
            .Select(MapearDto)
            .ToList();
    }

    public async Task<IReadOnlyList<DepartamentoDto>> ObtenerPorPaisAsync(
        int paisId)
    {
        if (paisId <= 0)
            return Array.Empty<DepartamentoDto>();

        var departamentos =
            await _departamentoRepository.ObtenerPorPaisAsync(paisId);

        return departamentos
            .Select(MapearDto)
            .ToList();
    }

    public async Task<DepartamentoDto?> ObtenerPorIdAsync(int id)
    {
        if (id <= 0)
            return null;

        var departamento =
            await _departamentoRepository.ObtenerPorIdAsync(id);

        return departamento is null
            ? null
            : MapearDto(departamento);
    }

    public async Task<DepartamentoDto> CrearAsync(
        GuardarDepartamentoDto dto)
    {
        Validar(dto);

        var pais = await _paisRepository.ObtenerPorIdAsync(dto.PaisId);

        if (pais is null)
            throw new ArgumentException(
                "El país seleccionado no existe.");

        var nombre = dto.Nombre.Trim();

        if (await _departamentoRepository.ExisteNombreAsync(
                dto.PaisId, nombre))
        {
            throw new ArgumentException(
                "Ya existe un departamento con ese nombre en el país seleccionado.");
        }

        var departamento = new Departamento
        {
            PaisId = dto.PaisId,
            Nombre = dto.Nombre.Trim(),
            Activo = true,
            FechaCreacion = DateTime.Now
        };

        await _departamentoRepository.AgregarAsync(departamento);

        return MapearDto(departamento);
    }

    public async Task<DepartamentoDto?> ActualizarAsync(
        int id,
        GuardarDepartamentoDto dto)
    {
        if (id <= 0)
            return null;

        Validar(dto);

        var departamento =
            await _departamentoRepository.ObtenerPorIdAsync(id);

        if (departamento is null)
            return null;

        var pais = await _paisRepository.ObtenerPorIdAsync(dto.PaisId);

        if (pais is null)
            throw new ArgumentException(
                "El país seleccionado no existe.");

        var nombre = dto.Nombre.Trim();

        if (await _departamentoRepository.ExisteNombreAsync(
                dto.PaisId, nombre, id))
        {
            throw new ArgumentException(
                "Ya existe otro departamento con ese nombre en el país seleccionado.");
        }

        departamento.PaisId = dto.PaisId;
        departamento.Nombre = dto.Nombre.Trim();
        departamento.FechaModificacion = DateTime.Now;
        departamento.Activo = dto.Activo;

        await _departamentoRepository.ActualizarAsync(departamento);

        return MapearDto(departamento);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
            return false;

        var departamento =
            await _departamentoRepository.ObtenerPorIdAsync(id);

        if (departamento is null)
            return false;

        await _departamentoRepository.EliminarAsync(departamento);

        return true;
    }

    private static void Validar(GuardarDepartamentoDto dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        if (dto.PaisId <= 0)
            throw new ArgumentException(
                "Debes seleccionar un país válido.");

        if (string.IsNullOrWhiteSpace(dto.Nombre))
            throw new ArgumentException(
                "El nombre del departamento es obligatorio.");

        if (dto.Nombre.Trim().Length > 100)
            throw new ArgumentException(
                "El nombre no puede superar los 100 caracteres.");
    }

    private static DepartamentoDto MapearDto(
        Departamento departamento)
    {
        return new DepartamentoDto
        {
            DepartamentoId = departamento.DepartamentoId,
            PaisId = departamento.PaisId,
            Nombre = departamento.Nombre,
            Activo = departamento.Activo
        };
    }
}
