using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entUsuario
    {
        public int UsuarioId { get; set; }
        public string Rol { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string UserName { get; set; }
        public string Pass { get; set; }
        public int? EstacionId { get; set; }
        public string EstacionNombre { get; set; }
        public bool Estado { get; set; }
    }
}
