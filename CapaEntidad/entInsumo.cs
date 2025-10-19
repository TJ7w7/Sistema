using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entInsumo
    {
        public int InsumoId { get; set; }
        public string Nombre { get; set; }
        public string UnidadMedida { get; set; }
        public bool Estado { get; set; }

        public List<entInsumoEstacion> Estaciones { get; set; }

        public entInsumo()
        {
            Estaciones = new List<entInsumoEstacion>();
        }
    }
}
