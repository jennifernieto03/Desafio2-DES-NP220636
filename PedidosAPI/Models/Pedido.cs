using System.ComponentModel.DataAnnotations;

namespace PedidosAPI.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        [MaxLength(30)]
        public string NumeroPedido { get; set; } = string.Empty;

        public DateTime FechaPedido { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(30)]
        public string Estado { get; set; } = "Pendiente";

        [Range(0.01, 99999999)]
        public decimal Total { get; set; }

        [MaxLength(250)]
        public string? Descripcion { get; set; }
    }
}
