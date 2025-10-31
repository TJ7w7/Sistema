using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entPedido
    {
        public int PedidoId { get; set; }
        public int MesaId { get; set; }
        public int NroMesa { get; set; }
        public int UsuarioId { get; set; }
        public string NombreMozo { get; set; }
        public DateTime Fecha { get; set; }
        public decimal PrecioTotal { get; set; }
        public bool Estado { get; set; }
        public int CantidadItems { get; set; }
        public List<entDetallePedido> Detalles { get; set; }

        public entPedido()
        {
            Detalles = new List<entDetallePedido>();
        }
    }
}
