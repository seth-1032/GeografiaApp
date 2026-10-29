
using Geografia.Application.DTOs;
using Geografia.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Geografia.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaisesController : ControllerBase
{
    private readonly IPaisService _paisService;

    public PaisesController(IPaisService paisService)
    {
        _paisService = paisService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PaisDto>>> ObtenerTodos()
    {
        var paises = await _paisService.ObtenerTodosAsync();
        return Ok(paises);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PaisDto>> ObtenerPorId(int id)
    {
        var pais = await _paisService.ObtenerPorIdAsync(id);

        if (pais is null)
            return NotFound(new { mensaje = "El país no existe." });

        return Ok(pais);
    }

    [HttpPost]
    public async Task<ActionResult<PaisDto>> Crear(
        [FromBody] GuardarPaisDto dto)
    {
        try
        {
            var pais = await _paisService.CrearAsync(dto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = pais.PaisId },
                pais);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PaisDto>> Actualizar(
        int id,
        [FromBody] GuardarPaisDto dto)
    {
        try
        {
            var pais = await _paisService.ActualizarAsync(id, dto);

            if (pais is null)
                return NotFound(new { mensaje = "El país no existe." });

            return Ok(pais);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminado = await _paisService.EliminarAsync(id);

        if (!eliminado)
            return NotFound(new { mensaje = "El país no existe." });

        return NoContent();
    }
}