using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClientesAPI.Models;
using ClientesAPI.Data;

[Route("api/[controller]")]
[ApiController]
public class ClientesController : ControllerBase
{
    private readonly ClientesDbContext _context;

    public ClientesController(ClientesDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
    {
        var clientes = await _context.Clientes
            .AsNoTracking()
            .ToListAsync();

        return Ok(clientes);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Cliente>> GetCliente(int id)
    {
        var cliente = await _context.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cliente == null)
        {
            return NotFound(new
            {
                mensaje = "Cliente no encontrado."
            });
        }

        return Ok(cliente);
    }

    [HttpPost]
    public async Task<ActionResult<Cliente>> CrearCliente(Cliente cliente)
    {
        var emailExiste = await _context.Clientes
            .AnyAsync(c => c.Email == cliente.Email);

        if (emailExiste)
        {
            return Conflict(new
            {
                mensaje = "El correo electrónico ya existe."
            });
        }

        cliente.Id = 0;
        cliente.FechaRegistro = DateTime.UtcNow;

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetCliente),
            new { id = cliente.Id },
            cliente);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> ActualizarCliente(
        int id,
        Cliente cliente)
    {
        if (id != cliente.Id)
        {
            return BadRequest(new
            {
                mensaje = "El ID de la URL no coincide con el objeto."
            });
        }

        var existente = await _context.Clientes
            .FirstOrDefaultAsync(c => c.Id == id);

        if (existente == null)
        {
            return NotFound();
        }

        var emailExiste = await _context.Clientes
            .AnyAsync(c =>
                c.Email == cliente.Email &&
                c.Id != id);

        if (emailExiste)
        {
            return Conflict(new
            {
                mensaje = "El correo electrónico ya está registrado."
            });
        }

        existente.Nombre = cliente.Nombre;
        existente.Apellido = cliente.Apellido;
        existente.Email = cliente.Email;
        existente.Telefono = cliente.Telefono;
        existente.Direccion = cliente.Direccion;
        existente.Activo = cliente.Activo;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> EliminarCliente(int id)
    {
        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cliente == null)
        {
            return NotFound();
        }

        _context.Clientes.Remove(cliente);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
