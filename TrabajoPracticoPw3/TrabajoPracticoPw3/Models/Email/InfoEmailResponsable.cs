using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrabajoPracticoPw3.Models.Email;

namespace TrabajoPracticoPw3.Models
{
    public class InfoEmailResponsable
    {
        public int PrecioTotal { get; set; }
        public List<InfoInvitadoEmail> Invitados { get; set; }
        public List<InfoGustosEmail> Gustos { get; set; }
    }
}