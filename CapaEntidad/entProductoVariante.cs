using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entProductoVariante
    {
        public int VarianteId { get; set; }
        public int ProductoId { get; set; }
        public string Tamaño { get; set; }
        public decimal Precio { get; set; }
        public bool Estado { get; set; }


    }
}
