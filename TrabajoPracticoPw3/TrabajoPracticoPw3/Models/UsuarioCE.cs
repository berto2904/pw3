using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrabajoPracticoPw3.Models
{
    public class UsuarioCE
    {   
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    [MetadataType(typeof(UsuarioCE))]

    public partial class Usuario
    {

    }
}