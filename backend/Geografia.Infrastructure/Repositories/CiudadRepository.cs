
using Geografia.Application.Interfaces.Repositories;
using Geografia.Domain.Entities;
using Geografia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Geografia.Infrastructure.Repositories;

public class CiudadRepository : ICiudadRepository
{
    private readonly GeografiaDbContext _context;

    public CiudadRepository(GeografiaDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Ciudad>> ObtenerTodosAsync()
    {
        return await _context.Ciudades
            .AsNoTracking()
            .OrderBy(c => c.Nombre)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Ciudad>> ObtenerPorDepartamentoAsync(
        int departamentoId)
    {
        return await _context.Ciudades
            .AsNoTracking()
            .Where(c => c.DepartamentoId == departamentoId)
            .OrderBy(c => c.Nombre)
            .ToListAsync();
    }

    public async Task<Ciudad?> ObtenerPorIdAsync(int id)
    {
        return await _context.Ciudades
            .FirstOrDefaultAsync(c => c.CiudadId == id);
    }

    public async Task AgregarAsync(Ciudad ciudad)
    {
        await _context.Ciudades.AddAsync(ciudad);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Ciudad ciudad)
    {
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Ciudad ciudad)
    {
        _context.Ciudades.Remove(ciudad);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExisteNombreAsync(
    int departamentoId,
    string nombre,
    int? excluirId = null)
    {
        var consulta = _context.Ciudades
            .AsNoTracking()
            .Where(c =>
                c.DepartamentoId == departamentoId &&
                c.Nombre == nombre);

        if (excluirId.HasValue)
        {
            consulta = consulta.Where(
                c => c.CiudadId != excluirId.Value);
        }

        return await consulta.AnyAsync();
    }
}
