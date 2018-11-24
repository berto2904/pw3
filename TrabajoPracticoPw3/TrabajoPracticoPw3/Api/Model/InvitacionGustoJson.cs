using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrabajoPracticoPw3.Api.Model
{
    public class InvitacionGustoJson
    {
        public int IdUsuario { get; set; }
        public Guid Token { get; set; }
        public List<GustoEmpanadaJson> GustosEmpanadasCantidad { get; set; }
        //public GustoEmpanadaJson[] GustoEmpanandasCantidad { get; set; }
    }
}