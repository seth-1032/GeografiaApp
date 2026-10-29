
using Geografia.Application.Interfaces.Repositories;
using Geografia.Domain.Entities;
using Geografia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Geografia.Infrastructure.Repositories;

public class PaisRepository : IPaisRepository
{
    private readonly GeografiaDbContext _context;

    public PaisRepository(GeografiaDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Pais>> ObtenerTodosAsync()
    {
        return await _context.Paises
            .AsNoTracking()
            .OrderBy(p => p.Nombre)
            .ToListAsync();
    }

    public async Task<Pais?> ObtenerPorIdAsync(int id)
    {
        return await _context.Paises
            .FirstOrDefaultAsync(p => p.PaisId == id);
    }

    public async Task AgregarAsync(Pais pais)
    {
        await _context.Paises.AddAsync(pais);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Pais pais)
    {
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Pais pais)
    {
        _context.Paises.Remove(pais);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExisteNombreAsync(
    string nombre,
    int? excluirId = null)
    {
        var consulta = _context.Paises
            .AsNoTracking()
            .Where(p => p.Nombre == nombre);

        if (excluirId.HasValue)
            consulta = consulta.Where(p => p.PaisId != excluirId.Value);

        return await consulta.AnyAsync();
    }

    public async Task<bool> ExisteCodigoIso2Async(
        string codigoIso2,
        int? excluirId = null)
    {
        var consulta = _context.Paises
            .AsNoTracking()
            .Where(p => p.CodigoIso2 == codigoIso2);

        if (excluirId.HasValue)
            consulta = consulta.Where(p => p.PaisId != excluirId.Value);

        return await consulta.AnyAsync();
    }
}
