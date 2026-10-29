
using Geografia.Application.Interfaces.Repositories;
using Geografia.Domain.Entities;
using Geografia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Geografia.Infrastructure.Repositories;

public class DepartamentoRepository : IDepartamentoRepository
{
    private readonly GeografiaDbContext _context;

    public DepartamentoRepository(GeografiaDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Departamento>> ObtenerTodosAsync()
    {
        return await _context.Departamentos
            .AsNoTracking()
            .OrderBy(d => d.Nombre)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Departamento>> ObtenerPorPaisAsync(
        int paisId)
    {
        return await _context.Departamentos
            .AsNoTracking()
            .Where(d => d.PaisId == paisId)
            .OrderBy(d => d.Nombre)
            .ToListAsync();
    }

    public async Task<Departamento?> ObtenerPorIdAsync(int id)
    {
        return await _context.Departamentos
            .FirstOrDefaultAsync(d => d.DepartamentoId == id);
    }

    public async Task AgregarAsync(Departamento departamento)
    {
        await _context.Departamentos.AddAsync(departamento);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Departamento departamento)
    {
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Departamento departamento)
    {
        _context.Departamentos.Remove(departamento);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExisteNombreAsync(
    int paisId,
    string nombre,
    int? excluirId = null)
    {
        var consulta = _context.Departamentos
            .AsNoTracking()
            .Where(d =>
                d.PaisId == paisId &&
                d.Nombre == nombre);

        if (excluirId.HasValue)
        {
            consulta = consulta.Where(
                d => d.DepartamentoId != excluirId.Value);
        }

        return await consulta.AnyAsync();
    }
}
