using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entDetallePedido
    {
        public int DetalleId { get; set; }
        public int PedidoId { get; set; }
        public int VarianteId { get; set; }

        // Propiedades adicionales para mostrar
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public string Tamaño { get; set; }
        public int? EstacionId { get; set; }
        public string NombreEstacion { get; set; }

        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal SubTotal { get; set; }
        public string EstadoDetalle { get; set; }
    }
}
