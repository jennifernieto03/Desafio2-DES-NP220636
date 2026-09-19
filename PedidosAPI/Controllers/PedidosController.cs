using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PedidosAPI.Data;
using PedidosAPI.Models;
using System.Net;

[Route("api/[controller]")]
[ApiController]
public class PedidosController : ControllerBase
{
    private readonly PedidosDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;

    public PedidosController(
        PedidosDbContext context,
        IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
    {
        var pedidos = await _context.Pedidos
            .AsNoTracking()
            .ToListAsync();

        return Ok(pedidos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Pedido>> GetPedido(int id)
    {
        var pedido = await _context.Pedidos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pedido == null)
        {
            return NotFound(new
            {
                mensaje = "Pedido no encontrado."
            });
        }

        return Ok(pedido);
    }

    [HttpGet("cliente/{clienteId:int}")]
    public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidosCliente(
        int clienteId)
    {
        var pedidos = await _context.Pedidos
            .AsNoTracking()
            .Where(p => p.ClienteId == clienteId)
            .ToListAsync();

        return Ok(pedidos);
    }

    [HttpPost]
    public async Task<ActionResult<Pedido>> CrearPedido(Pedido pedido)
    {
        var client = _httpClientFactory.CreateClient("ClientesAPI");

        var response = await client.GetAsync(
            $"/api/clientes/{pedido.ClienteId}");

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return BadRequest(new
            {
                mensaje = "El cliente especificado no existe."
            });
        }

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode(503, new
            {
                mensaje = "No fue posible validar el cliente."
            });
        }

        var numeroExiste = await _context.Pedidos
            .AnyAsync(p => p.NumeroPedido == pedido.NumeroPedido);

        if (numeroExiste)
        {
            return Conflict(new
            {
                mensaje = "El número de pedido ya existe."
            });
        }

        pedido.Id = 0;
        pedido.FechaPedido = DateTime.UtcNow;

        _context.Pedidos.Add(pedido);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetPedido),
            new { id = pedido.Id },
            pedido);
    }
}
