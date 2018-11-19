using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrabajoPracticoPw3.Helper;
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
            EmailService es = new EmailService();
            Usuario uLogueado = ctx.Usuario.Find(usuarioLogueado.IdUsuario);
            Pedido nuevoPedido = new Pedido
            {
                NombreNegocio = form["NombreNegocio"],
                Descripcion = form["Descripcion"],
                PrecioUnidad = int.Parse(form["PrecioUnidad"]),
                PrecioDocena = int.Parse(form["PrecioDocena"]),
                FechaCreacion = DateTime.Now,
                Usuario = uLogueado,
                //IdUsuarioResponsable = usuarioLogueado.IdUsuario,
                //EstadoPedido = ctx.EstadoPedido.SingleOrDefault(x => x.Nombre == "Abierto")
                EstadoPedido = ctx.EstadoPedido.Where(x => x.Nombre == "Abierto").FirstOrDefault(),
            };


            int[] gustosDisponibles = Array.ConvertAll(form.GetValues("gustosDisponibles"), int.Parse);
            int[] invitados = Array.ConvertAll(form.GetValues("invitados"), int.Parse);

            foreach (int gustoId in gustosDisponibles)
            {
                GustoEmpanada gustoEncontrado = ctx.GustoEmpanada.SingleOrDefault(x => x.IdGustoEmpanada == gustoId);
                nuevoPedido.GustoEmpanada.Add(gustoEncontrado);

            }

            List<int> Listainvitados = invitados.OfType<int>().ToList();
            Listainvitados.Add(usuarioLogueado.IdUsuario);

            foreach (int invitadoId in Listainvitados)
            {
                Usuario usuarioEncontrado = ctx.Usuario.SingleOrDefault(x => x.IdUsuario == invitadoId);
                InvitacionPedido invitacionPedido = new InvitacionPedido
                {
                    Pedido = nuevoPedido,
                    Usuario = usuarioEncontrado,
                    Token = new Guid(new Md5Hash().GetMD5((usuarioEncontrado.Email + nuevoPedido.FechaCreacion))),
                    Completado = false,
                };
                ctx.InvitacionPedido.Add(invitacionPedido);
                es.EnviarEmailInvitados(invitacionPedido);
            }

            ctx.Pedido.Add(nuevoPedido);

            try { ctx.SaveChanges(); }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                            validationError.PropertyName,
                            validationError.ErrorMessage);
                    }
                }
            }

            return nuevoPedido.IdPedido;
        }


        public int EliminarService(int id)
        {
            Pedido pedido = ObtenerPedidoById(id);
            List<int> gustoEmpanadasIds = new List<int>();
            List<int> invitacionPedidoIds = new List<int>();
            List<int> invitacionPedidoGustoEmpanadaUsuarioIds = new List<int>();

            //Agregacion
            foreach (var gusto in pedido.GustoEmpanada)
            {
                gustoEmpanadasIds.Add(gusto.IdGustoEmpanada);
            }

            foreach (var invitacion in pedido.InvitacionPedido)
            {
                invitacionPedidoIds.Add(invitacion.IdInvitacionPedido);
            }
            foreach (var invitacionPedidoGustoEmpanadaUsuario in pedido.InvitacionPedidoGustoEmpanadaUsuario)
            {
                invitacionPedidoGustoEmpanadaUsuarioIds.Add(invitacionPedidoGustoEmpanadaUsuario.IdInvitacionPedidoGustoEmpanadaUsuario);
            }

            //Eliminacion 
            foreach (var idGustos in gustoEmpanadasIds)
            {
                var gustoEliminar = ctx.GustoEmpanada.FirstOrDefault(g => g.IdGustoEmpanada == idGustos);
                pedido.GustoEmpanada.Remove(gustoEliminar);
            }

            foreach (var idInvitacion in invitacionPedidoIds)
            {
                var invitacionEliminar = ctx.InvitacionPedido.FirstOrDefault(i => i.IdInvitacionPedido == idInvitacion);
                ctx.InvitacionPedido.Remove(invitacionEliminar);
            }
            
            foreach(var idInvitacionPedidoGustoEmpanadaUsuario in invitacionPedidoGustoEmpanadaUsuarioIds)
            {
                var invitacionPedidoGustoEmpanadaUsuarioEliminar = ctx.InvitacionPedidoGustoEmpanadaUsuario.FirstOrDefault(i => i.IdInvitacionPedidoGustoEmpanadaUsuario == idInvitacionPedidoGustoEmpanadaUsuario);
                ctx.InvitacionPedidoGustoEmpanadaUsuario.Remove(invitacionPedidoGustoEmpanadaUsuarioEliminar);
            }
            ctx.Pedido.Remove(pedido);
            ctx.SaveChanges();

            return pedido.IdPedido;
        }

        public int ElegirService(FormCollection form, Usuario usuarioLoguedado)
        {
            Pedido pedido = ObtenerPedidoById(int.Parse(form["idPedido"]));

            

            foreach (var gusto in pedido.GustoEmpanada)
            {
                pedido.InvitacionPedidoGustoEmpanadaUsuario.Remove(ctx.InvitacionPedidoGustoEmpanadaUsuario.Where(i=> i.GustoEmpanada.IdGustoEmpanada == gusto.IdGustoEmpanada && i.IdUsuario == usuarioLoguedado.IdUsuario).FirstOrDefault());
                try
                {
                    var cantidadEmpanada = int.Parse(form["gustoEmpanada_" + gusto.IdGustoEmpanada]);
                    InvitacionPedidoGustoEmpanadaUsuario ipgeu = new InvitacionPedidoGustoEmpanadaUsuario
                    {
                        Cantidad = cantidadEmpanada,
                        GustoEmpanada = gusto,
                        IdUsuario = usuarioLoguedado.IdUsuario
                    };

                    pedido.InvitacionPedidoGustoEmpanadaUsuario.Add(ipgeu);
                

                }
                catch (Exception)
                {

                    
                }
                
            }
                    ctx.SaveChanges();
            return pedido.IdPedido;
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

        public Pedido ObtenerPedidoById(int id)
        {
            Pedido pedido = ctx.Pedido.Find(id);
            return pedido;
        }

        public GustoEmpanada ObtenerGustoEmpanadaById(int id)
        {
            GustoEmpanada gusto = ctx.GustoEmpanada.Find(id);
            return gusto;
        }

        public List<InvitacionPedidoGustoEmpanadaUsuario> ObtenerIPGEUByIdPedido(int id, Usuario usuarioLogueado)
        {
            var query = (from i in ctx.InvitacionPedidoGustoEmpanadaUsuario
                         where i.IdPedido == id && i.IdUsuario == usuarioLogueado.IdUsuario
                         select i).ToList();

            return query;
        }
    }
}