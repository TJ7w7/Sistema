using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entInsumoEstacion
    {
        public int InsumoEstacionId { get; set; }
        public int InsumoId { get; set; }
        public string Nombre { get; set; }
        public string UnidadMedida { get; set; }
        public int EstacionId { get; set; }
        public string NombreEstacion { get; set; }
        public decimal StockActual { get; set; }
        public decimal StockMinimo { get; set; }
        public bool Estado { get; set; }
        public bool EsBajoStock { get; set; }
    }
}
