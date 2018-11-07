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

        public List<Pedido> ListarPedidosByUsuario(Usuario usuario)
        {  var query =
               (from p in ctx.Pedido
               join ep in ctx.EstadoPedido on p.IdEstadoPedido equals ep.IdEstadoPedido
               join ip in ctx.InvitacionPedido on p.IdPedido equals ip.IdPedido
               where ip.IdUsuario == usuario.IdUsuario
                orderby p.FechaCreacion
                select
                    p).ToList();
            return query;
        }

        public Boolean PedidoUsuarioResponsableIsTrue(int idPedido, Usuario usuario)
        {
            var query = (from p in ctx.Pedido
                         where p.IdUsuarioResponsable == usuario.IdUsuario &&
                                p.IdPedido == idPedido
                         select p).ToList();

            if (query.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}