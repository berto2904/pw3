//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrabajoPracticoPw3.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class InvitacionPedido
    {
        public int IdInvitacionPedido { get; set; }
        public int IdPedido { get; set; }
        public int IdUsuario { get; set; }
        public System.Guid Token { get; set; }
        public bool Completado { get; set; }
    
        public virtual Pedido Pedido { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
