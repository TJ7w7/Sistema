using CapaDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class Recursos
    {
        public static bool EnviarCorreo(string destinatario, string asunto, string mensaje)
        {
            //bool resultado = false;
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(destinatario);
                mail.From = new MailAddress("juniortomasarenas24@gmail.com");
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true; // Si el mensaje es HTML

                SmtpClient smtp = new SmtpClient()
                {
                    Credentials = new NetworkCredential("juniortomasarenas24@gmail.com", "xpwy zvlz waiq fphd"),
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true
                };

                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("Error al enviar correo a " + destinatario + ": " + ex.Message);
                return false;
            }
        }

        public static string EncriptarSHA256(string texto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(texto);
                byte[] hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        public static string GenerarNombreUsuario(string nombre, string apellido)
        {
            string n = RemoverCaracteresEspeciales(nombre.ToLower().Trim());
            string a = RemoverCaracteresEspeciales(apellido.ToLower().Trim());

            if (n.Contains(" "))
                n = n.Split(' ')[0];

            if (a.Contains(" "))
                a = a.Split(' ').Last();

            string baseUsuario = $"{n[0]}{a}";

            string aleatorio = new Random().Next(10, 99).ToString();

            return $"{baseUsuario}{aleatorio}";
        }

        public static string GenerarNombreUsuarioUnico(string nombre, string apellido)
        {
            string usuario;
            int intentos = 0;
            do
            {
                usuario = GenerarNombreUsuario(nombre, apellido);
                intentos++;
                if (intentos > 10)
                    throw new Exception("No se pudo generar un nombre de usuario único.");
            }
            while (datUsuario.Instancia.ExisteNombreUsuario(usuario));

            return usuario;
        }

        private static string RemoverCaracteresEspeciales(string texto)
        {
            string normalizado = texto.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (char c in normalizado)
            {
                var cat = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (cat != System.Globalization.UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            return sb.ToString()
                     .Normalize(NormalizationForm.FormC)
                     .Replace(" ", "")
                     .Replace("'", "")
                     .Replace("-", "");
        }
    }
}
