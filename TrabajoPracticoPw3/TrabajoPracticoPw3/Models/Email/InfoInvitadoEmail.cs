using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrabajoPracticoPw3.Models.Email
{
    public class InfoInvitadoEmail
    {
        public string Email { get; set; }
        public float Precio { get; set; }
        public List<InfoGustosEmail> Empanadas { get; set; }
        public int CantidadTotal { get; set; }
    }
}