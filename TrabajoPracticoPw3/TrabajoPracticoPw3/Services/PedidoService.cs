using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

        //------------------------------Negocio------------------------------

        public int IniciarService(FormCollection form, Usuario usuarioLogueado)
        {
            Pedido nuevoPedido = new Pedido
            {
                NombreNegocio = form["nombre"],
                Descripcion = form["descripcion"],
                PrecioUnidad = int.Parse(form["precioUnidad"]),
                PrecioDocena = int.Parse(form["precioDocena"]),
                FechaCreacion = DateTime.Now,
                IdUsuarioResponsable = usuarioLogueado.IdUsuario,
                EstadoPedido = ctx.EstadoPedido.SingleOrDefault(x => x.Nombre == "Abierto")
            };

            int[] gustosDisponibles = Array.ConvertAll(form.GetValues("gustosDisponibles"), int.Parse);
            int[] invitados = Array.ConvertAll(form.GetValues("invitados"), int.Parse);

            foreach (int gustoId in gustosDisponibles)
            {
                GustoEmpanada gustoEncontrado = ctx.GustoEmpanada.SingleOrDefault(x => x.IdGustoEmpanada == gustoId);
                nuevoPedido.GustoEmpanada.Add(gustoEncontrado);

            }

            //foreach (int invitado in invitados)
            //{
            //    Usuario usuarioEncontrado = ctx.Usuario.SingleOrDefault(x => x.IdUsuario == invitado);
            //}

            ctx.Pedido.Add(nuevoPedido);
            ctx.SaveChanges();

            return nuevoPedido.IdPedido;
        }

        //------------------------------Queries------------------------------

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

        public Boolean InvitacionPedidoUsuarioIsTrue(int idPedido, Usuario usuario)
        {
            var query = (from ip in ctx.InvitacionPedido
                         where ip.IdUsuario == usuario.IdUsuario && ip.IdPedido == idPedido
                         select ip).ToList();

            if(query.Count > 0)
            {
                return true;
            }
            return false;
        }

        public List<GustoEmpanada> ObtenerGustoDeEmpanadasList()
        {
            var query = (from ge in ctx.GustoEmpanada
                         select ge).ToList();

            return query;
        }

        public List<Usuario> ObtenerUsuarioList(Usuario usuarioLogueado)
        {
            var query = (from u in ctx.Usuario
                         where u.IdUsuario != usuarioLogueado.IdUsuario
                         select u).ToList();

            return query;
        }
    }
}