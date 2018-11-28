using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using TrabajoPracticoPw3.Models;

namespace TrabajoPracticoPw3.Services
{
    public class EmailService
    {
        //public string output { get; private set; }

        public void EnviarEmailInvitados(InvitacionPedido invitacion)
        {
                MailMessage email = new MailMessage("3empanadaspw3@gmail.com", "berto2904@gmail.com");
                email.Subject = "Te invitó "+ invitacion.Pedido.Usuario.Email+" para pedir empanadas";
                email.Body = "Has sido invitado para realizar un pedido de empanadas http://localhost:50846/pedidos/elegir/" + invitacion.Token;

                SmtpClient smtp = new SmtpClient();
                smtp.Send(email);

        }

        public void EnviarEmailPrueba(String mensaje)
        {
            MailMessage email = new MailMessage("3empanadaspw3@gmail.com","berto2904@gmail.com");
            email.Subject = "Asunto de prueba";
            email.Body = "Prueba de contenido";

            SmtpClient smtp = new SmtpClient();
            smtp.Send(email);

            //try
            //{
            //    email.Dispose();
            //    output = "Corre electrónico fue enviado satisfactoriamente.";
            //}
            //catch (Exception ex)
            //{
            //    output = "Error enviando correo electrónico: " + ex.Message;
            //}

            //Console.WriteLine(output);
        }
    }
}