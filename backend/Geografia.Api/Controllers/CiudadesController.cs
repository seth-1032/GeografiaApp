
using Geografia.Application.DTOs;
using Geografia.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Geografia.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CiudadesController : ControllerBase
{
    private readonly ICiudadService _ciudadService;

    public CiudadesController(ICiudadService ciudadService)
    {
        _ciudadService = ciudadService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CiudadDto>>> ObtenerTodos()
    {
        var ciudades = await _ciudadService.ObtenerTodosAsync();
        return Ok(ciudades);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CiudadDto>> ObtenerPorId(int id)
    {
        var ciudad = await _ciudadService.ObtenerPorIdAsync(id);

        if (ciudad is null)
            return NotFound(new
            {
                mensaje = "La ciudad no existe."
            });

        return Ok(ciudad);
    }

    [HttpGet("por-departamento/{departamentoId:int}")]
    public async Task<ActionResult<IReadOnlyList<CiudadDto>>>
        ObtenerPorDepartamento(int departamentoId)
    {
        var ciudades =
            await _ciudadService.ObtenerPorDepartamentoAsync(
                departamentoId);

        return Ok(ciudades);
    }

    [HttpPost]
    public async Task<ActionResult<CiudadDto>> Crear(
        [FromBody] GuardarCiudadDto dto)
    {
        try
        {
            var ciudad = await _ciudadService.CrearAsync(dto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = ciudad.CiudadId },
                ciudad);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CiudadDto>> Actualizar(
        int id,
        [FromBody] GuardarCiudadDto dto)
    {
        try
        {
            var ciudad = await _ciudadService.ActualizarAsync(id, dto);

            if (ciudad is null)
                return NotFound(new
                {
                    mensaje = "La ciudad no existe."
                });

            return Ok(ciudad);
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
            var eliminado = await _ciudadService.EliminarAsync(id);

            if (!eliminado)
                return NotFound(new
                {
                    mensaje = "La ciudad no existe."
                });

            return NoContent();
        }
        catch (DbUpdateException)
        {
            return Conflict(new
            {
                mensaje = "No se pudo eliminar la ciudad por una restricción de la base de datos."
            });
        }
    }
}
