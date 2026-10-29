
using Geografia.Application.DTOs;
using Geografia.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Geografia.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartamentosController : ControllerBase
{
    private readonly IDepartamentoService _departamentoService;

    public DepartamentosController(
        IDepartamentoService departamentoService)
    {
        _departamentoService = departamentoService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DepartamentoDto>>>
        ObtenerTodos()
    {
        var departamentos =
            await _departamentoService.ObtenerTodosAsync();

        return Ok(departamentos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DepartamentoDto>> ObtenerPorId(int id)
    {
        var departamento =
            await _departamentoService.ObtenerPorIdAsync(id);

        if (departamento is null)
            return NotFound(new
            {
                mensaje = "El departamento no existe."
            });

        return Ok(departamento);
    }

    [HttpGet("por-pais/{paisId:int}")]
    public async Task<ActionResult<IReadOnlyList<DepartamentoDto>>>
        ObtenerPorPais(int paisId)
    {
        var departamentos =
            await _departamentoService.ObtenerPorPaisAsync(paisId);

        return Ok(departamentos);
    }

    [HttpPost]
    public async Task<ActionResult<DepartamentoDto>> Crear(
        [FromBody] GuardarDepartamentoDto dto)
    {
        try
        {
            var departamento =
                await _departamentoService.CrearAsync(dto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = departamento.DepartamentoId },
                departamento);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<DepartamentoDto>> Actualizar(
        int id,
        [FromBody] GuardarDepartamentoDto dto)
    {
        try
        {
            var departamento =
                await _departamentoService.ActualizarAsync(id, dto);

            if (departamento is null)
                return NotFound(new
                {
                    mensaje = "El departamento no existe."
                });

            return Ok(departamento);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            var eliminado =
                await _departamentoService.EliminarAsync(id);

            if (!eliminado)
                return NotFound(new
                {
                    mensaje = "El departamento no existe."
                });

            return NoContent();
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException)
        {
            return Conflict(new
            {
                mensaje = "No se puede eliminar el departamento porque tiene ciudades asociadas."
            });
        }
    }
}
