using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entDetalleReceta
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public int InsumoId { get; set; }
        public string NombreInsumo { get; set; }
        public string UnidadMedida { get; set; }
        public decimal Cantidad { get; set; }
    }
}
