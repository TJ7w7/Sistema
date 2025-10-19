using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class ServicioImagen
    {
        #region Singleton
        private static readonly ServicioImagen _instancia = new ServicioImagen();
        public static ServicioImagen Instancia
        {
            get { return _instancia; }
        }
        #endregion

        private readonly string _carpetaImagenes;

        private ServicioImagen()
        {
            // Carpeta fija fuera del proyecto
            _carpetaImagenes = @"C:\DatosRestaurante\Imagenes\Productos";

            // Crear carpeta si no existe
            if (!Directory.Exists(_carpetaImagenes))
            {
                try
                {
                    Directory.CreateDirectory(_carpetaImagenes);
                    System.Diagnostics.Debug.WriteLine($"Carpeta creada: {_carpetaImagenes}");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error al crear carpeta de imágenes: {ex.Message}");
                }
            }
        }

        public string GuardarImagen(byte[] bytes, string nombreOriginal)
        {
            try
            {
                // Generar nombre único para evitar duplicados
                string extension = Path.GetExtension(nombreOriginal).ToLower();
                string nombreArchivo = $"{Guid.NewGuid()}{extension}";
                string rutaCompleta = Path.Combine(_carpetaImagenes, nombreArchivo);

                // Guardar archivo
                File.WriteAllBytes(rutaCompleta, bytes);

                // Retornar solo el nombre del archivo (se guarda en BD)
                return nombreArchivo;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar imagen: {ex.Message}");
            }
        }

        public bool EliminarImagen(string nombreArchivo)
        {
            try
            {
                if (string.IsNullOrEmpty(nombreArchivo)) return false;

                string rutaCompleta = Path.Combine(_carpetaImagenes, nombreArchivo);

                if (File.Exists(rutaCompleta))
                {
                    File.Delete(rutaCompleta);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error eliminando imagen: {ex.Message}");
                return false;
            }
        }

        public string ObtenerRutaCompleta(string nombreArchivo)
        {
            if (string.IsNullOrEmpty(nombreArchivo)) return string.Empty;
            return Path.Combine(_carpetaImagenes, nombreArchivo);
        }

        public bool ExisteImagen(string nombreArchivo)
        {
            if (string.IsNullOrEmpty(nombreArchivo)) return false;
            string rutaCompleta = Path.Combine(_carpetaImagenes, nombreArchivo);
            return File.Exists(rutaCompleta);
        }
    }
}
