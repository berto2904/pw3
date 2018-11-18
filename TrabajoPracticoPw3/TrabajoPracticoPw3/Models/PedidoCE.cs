using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TrabajoPracticoPw3.Models
{
    public class PedidoCE
    {
        [StringLength(200), Required(ErrorMessage = "El Nombre es obligatorio y hasta 200 caracteres")]
        public string NombreNegocio { get; set; }
        //[Required(ErrorMessage ="Agregar una descripción")]
        public string Descripcion { get; set; }
        [Range(0, 9999), Required(ErrorMessage = "El Precio es obligatorio y hasta 4 caracteres")]
        public int PrecioUnidad { get; set; }
        [Range(0, 9999), Required(ErrorMessage = "El Precio es obligatorio y hasta 4 caracteres")]
        public int PrecioDocena { get; set; }
    }

    [MetadataType(typeof(PedidoCE))]

    public partial class Pedido
    {

    }
}