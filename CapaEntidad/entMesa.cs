using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entMesa
    {
        public int MesaId { get; set; }
        public int NroMesa { get; set; }
        public int ZonaId { get; set; }
        public string NombreZona { get; set; }
        public string Estado { get; set; }
        public decimal PosicionX { get; set; }
        public decimal PosicionY { get; set; }
    }
}
