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

        public void EnviarEmailResponsable(Pedido pedidoAFinalizar, InfoEmailResponsable info)
        {
                MailMessage email = new MailMessage("3empanadaspw3@gmail.com", "berto2904@gmail.com");
                //MailMessage email = new MailMessage("3empanadaspw3@gmail.com", pedidoAFinalizar.Usuario.Email);
                email.Subject = "Detalle de Recaudacion de "+pedidoAFinalizar.Descripcion.ToString();

                string listaInvitados = "";
                string listaGustos = "";

                foreach (var invitado in info.Invitados)
                {
                    string inv = invitado.Email + " : $" + invitado.Precio.ToString()+" <br>";
                    listaInvitados += inv;
                }

                foreach (var gusto in info.Gustos)
                {
                    string gus = gusto.Gusto + " : " + gusto.Cantidad.ToString() + "<br>";
                    listaGustos += gus;
                }
                
                email.Body =    "<h2>Precio Total: $"+info.PrecioTotal.ToString()+"</h2>" +
                            //"<br>" +
                            "<h2>Detalle Recaudacion:</h2>" +
                            //"<br>" +
                            "<div>" +
                            "  <h3>Invitados:</h3>" +
                            "  <div>" +
                            listaInvitados+
                            "  </div>" +
                            "</div>" +
                            "<br>" +
                            "<div >" +
                            "  <h3>Detalle Pedido:</h3>" +
                            "  <div>" +
                            listaGustos+
                             " </div>" +
                            "</div>";

            email.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Send(email);


        }

        public void EnviarEmailInvitados(InvitacionPedido invitacion)
        {
                MailMessage email = new MailMessage("3empanadaspw3@gmail.com", "berto2904@gmail.com");
                //MailMessage email = new MailMessage("3empanadaspw3@gmail.com", invitacion.Usuario.Email);

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