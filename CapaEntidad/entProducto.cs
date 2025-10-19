using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entProducto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public int CategoriaId { get; set; }
        public string NombreCategoria { get; set; }
        public int? EstacionId { get; set; }
        public string NombreEstacion { get; set; }
        public string Imagen { get; set; }
        public bool Estado { get; set; }

        public List<entProductoVariante> Variantes { get; set; }

        public entProducto()
        {
            Variantes = new List<entProductoVariante>();
        }
    }
}
