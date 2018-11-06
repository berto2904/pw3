using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrabajoPracticoPw3.Models;

namespace TrabajoPracticoPw3.Services
{
    public class PedidoService
    {
        TPEntities ctx = new TPEntities();
        public Usuario BuscarUsuarioById(int idUsuario)
        {
            Usuario usuarioEncontrado = ctx.Usuario.SingleOrDefault(x => x.IdUsuario == idUsuario);
            return usuarioEncontrado;
        }
        public List<Pedido> ListarPedidosByIdUsuario(int idUsuario)
        {
     
            var query =
               (from p in ctx.Pedido
               join ep in ctx.EstadoPedido on p.IdEstadoPedido equals ep.IdEstadoPedido
               join ip in ctx.InvitacionPedido on p.IdPedido equals ip.IdPedido
               where ip.IdUsuario == idUsuario
               select
                    p).ToList();




            /*from Pedido p
                Join EstadoPedido ep on p.IdEstadoPedido = ep.IdEstadoPedido
                Join InvitacionPedido ip on ip.IdPedido = p.IdPedido
                where ip.IdUsuario = 1*/
            return query;
        }
    }
}