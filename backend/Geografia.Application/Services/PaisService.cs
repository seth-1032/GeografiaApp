
using Geografia.Application.DTOs;
using Geografia.Application.Interfaces.Repositories;
using Geografia.Application.Interfaces.Services;
using Geografia.Domain.Entities;

namespace Geografia.Application.Services;

public class PaisService : IPaisService
{
    private readonly IPaisRepository _paisRepository;

    public PaisService(IPaisRepository paisRepository)
    {
        _paisRepository = paisRepository;
    }

    public async Task<IReadOnlyList<PaisDto>> ObtenerTodosAsync()
    {
        var paises = await _paisRepository.ObtenerTodosAsync();

        return paises.Select(MapearDto).ToList();
    }

    public async Task<PaisDto?> ObtenerPorIdAsync(int id)
    {
        if (id <= 0)
            return null;

        var pais = await _paisRepository.ObtenerPorIdAsync(id);

        return pais is null ? null : MapearDto(pais);
    }

    public async Task<PaisDto> CrearAsync(GuardarPaisDto dto)
    {
        Validar(dto);

        var nombre = dto.Nombre.Trim();
        var codigo = NormalizarCodigo(dto.CodigoIso2);

        if (await _paisRepository.ExisteNombreAsync(nombre))
            throw new ArgumentException(
                "Ya existe un país con ese nombre.");

        if (codigo is not null &&
            await _paisRepository.ExisteCodigoIso2Async(codigo))
        {
            throw new ArgumentException(
                "Ya existe un país con ese código ISO.");
        }

        var pais = new Pais
        {
            Nombre = dto.Nombre.Trim(),
            CodigoIso2 = NormalizarCodigo(dto.CodigoIso2),
            Activo = true,
            FechaCreacion = DateTime.Now
        };

        await _paisRepository.AgregarAsync(pais);

        return MapearDto(pais);
    }

    public async Task<PaisDto?> ActualizarAsync(
        int id,
        GuardarPaisDto dto)
    {
        if (id <= 0)
            return null;

        Validar(dto);

        var pais = await _paisRepository.ObtenerPorIdAsync(id);

        if (pais is null)
            return null;

        var nombre = dto.Nombre.Trim();
        var codigo = NormalizarCodigo(dto.CodigoIso2);

        if (await _paisRepository.ExisteNombreAsync(nombre, id))
            throw new ArgumentException(
                "Ya existe otro país con ese nombre.");

        if (codigo is not null &&
            await _paisRepository.ExisteCodigoIso2Async(codigo, id))
        {
            throw new ArgumentException(
                "Ya existe otro país con ese código ISO.");
        }

        pais.Nombre = dto.Nombre.Trim();
        pais.CodigoIso2 = NormalizarCodigo(dto.CodigoIso2);
        pais.Activo = dto.Activo;
        pais.FechaModificacion = DateTime.Now;

        await _paisRepository.ActualizarAsync(pais);

        return MapearDto(pais);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
            return false;

        var pais = await _paisRepository.ObtenerPorIdAsync(id);

        if (pais is null)
            return false;

        await _paisRepository.EliminarAsync(pais);

        return true;
    }

    private static void Validar(GuardarPaisDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
            throw new ArgumentException(
                "El nombre del país es obligatorio.");

        if (dto.Nombre.Trim().Length > 100)
            throw new ArgumentException(
                "El nombre no puede superar los 100 caracteres.");

        if (!string.IsNullOrWhiteSpace(dto.CodigoIso2))
        {
            var codigo = dto.CodigoIso2.Trim().ToUpperInvariant();

            if (codigo.Length != 2 ||
                codigo.Any(c => c < 'A' || c > 'Z'))
            {
                throw new ArgumentException(
                    "El código ISO debe contener dos letras.");
            }
        }
    }

    private static string? NormalizarCodigo(string? codigo)
    {
        return string.IsNullOrWhiteSpace(codigo)
            ? null
            : codigo.Trim().ToUpperInvariant();
    }

    private static PaisDto MapearDto(Pais pais)
    {
        return new PaisDto
        {
            PaisId = pais.PaisId,
            Nombre = pais.Nombre,
            CodigoIso2 = pais.CodigoIso2,
            Activo = pais.Activo
        };
    }
}
