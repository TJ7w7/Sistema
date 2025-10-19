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
        public int UsuarioId { get; set; }
        public DateFormat Fecha { get; set; }
        public decimal PrecioTotal { get; set; }
        public bool Esstado { get; set; }
    }
}
