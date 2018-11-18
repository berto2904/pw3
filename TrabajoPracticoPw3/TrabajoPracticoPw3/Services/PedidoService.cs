using System;
using System.Collections.Generic;
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
                NombreNegocio = form["nombre"],
                Descripcion = form["descripcion"],
                PrecioUnidad = int.Parse(form["precioUnidad"]),
                PrecioDocena = int.Parse(form["precioDocena"]),
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
            ctx.SaveChanges();

            return nuevoPedido.IdPedido;
        }

        //------------------------------Queries------------------------------

        public List<Pedido> ListarPedidosByUsuario(Usuario usuario)
        {
            var query =
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

        internal List<Usuario> DeterminarEnviosDeInvitacionDesdeFormCollection(FormCollection form)
        {
            int IdEnviarInvitacion = ObtenerEnviarInvitacion(form);

            List<Usuario> usuariosAInvitar = new List<Usuario>();

            switch (IdEnviarInvitacion)
            {
                case 1:
                    //A nadie
                    break;
                case 2:
                    //Re enviar a todos
                    usuariosAInvitar = ObtenerTodosLosUsuariosInvitados(form);
                    break;
                case 3:
                    //Enviar solo a los nuevos
                    usuariosAInvitar = ObtenerLosUsuariosQueAntesNoEstabanInvitados(form);
                    break;
                case 4:
                    //Re enviar a los que no eligieron gustos
                    usuariosAInvitar = ObtenerLosUsuariosInvitadosQueNoEligieronGustos(form);
                    break;
            }

            return usuariosAInvitar;
        }


        public List<Usuario> ObtenerTodosLosUsuariosInvitados(FormCollection form)
        {
            List<Usuario> listaUsuario = new List<Usuario>();
            string[] usuariosInvitados = form.GetValues("invitados");
            foreach (var usuario in usuariosInvitados)
            {
                Usuario usuarioEncontrado = ctx.Usuario.Find(int.Parse(usuario));
                listaUsuario.Add(usuarioEncontrado);
            }

            return listaUsuario;
        }

        public List<Usuario> ObtenerLosUsuariosQueAntesNoEstabanInvitados(FormCollection form)
        {
            List<Usuario> usuariosAInvitar = new List<Usuario>();

            Pedido pedidoAEditar = ObtenerPedidoPorId(int.Parse(form["id"]));

            foreach (Usuario usuario in form)
            {
                foreach (InvitacionPedido invitacionPedido in pedidoAEditar.InvitacionPedido)
                {
                    if (usuario.InvitacionPedido.Contains(invitacionPedido))
                    {
                        break;
                    }
                    else
                    {
                        usuariosAInvitar.Add(usuario);
                        break;
                    }
                }
            }

            return usuariosAInvitar;
        }

        private List<Usuario> ObtenerLosUsuariosInvitadosQueNoEligieronGustos(FormCollection form)
        {
            List<Usuario> usuariosAInvitar = new List<Usuario>();

            Pedido pedidoAEditar = ObtenerPedidoPorId(int.Parse(form["id"]));

            foreach (Usuario usuario in form)
            {
                if (pedidoAEditar.InvitacionPedidoGustoEmpanadaUsuario.Count() == 0)
                {
                    usuariosAInvitar.Add(usuario);
                }
                else
                {
                    foreach (InvitacionPedidoGustoEmpanadaUsuario invitacionPedidoGustoEmpanadaUsuario in pedidoAEditar.InvitacionPedidoGustoEmpanadaUsuario)
                    {
                        if (usuario.IdUsuario == invitacionPedidoGustoEmpanadaUsuario.IdUsuario)
                        {
                            break;
                        }
                        else
                        {
                            usuariosAInvitar.Add(usuario);
                        }
                    }
                }
            }

            return usuariosAInvitar;
        }

        public Pedido ObtenerPedidoDesdeFormCollection(FormCollection form)
        {
            Pedido pedidoEditado = new Pedido
            {
                NombreNegocio = form["nombre"],
                Descripcion = form["descripcion"],
                PrecioUnidad = int.Parse(form["precioUnidad"]),
                PrecioDocena = int.Parse(form["precioDocena"]),
                FechaCreacion = DateTime.Now,
                //IdUsuarioResponsable = usuarioLogueado.IdUsuario,
                //EstadoPedido = ctx.EstadoPedido.SingleOrDefault(x => x.Nombre == "Abierto")
                EstadoPedido = ctx.EstadoPedido.Where(x => x.Nombre == "Abierto").FirstOrDefault(),
            };
            return pedidoEditado;
        }

        internal void ActualizarValoresDeUnPedidoDesdeFormCollection(FormCollection form)
        {
            Pedido pedidoEditado = ObtenerPedidoDesdeFormCollection(form);

            Pedido pedidoAEditar = ObtenerPedidoPorId(int.Parse(form["id"]));

            pedidoAEditar.Descripcion = pedidoEditado.Descripcion;
            pedidoAEditar.GustoEmpanada = pedidoEditado.GustoEmpanada;
            pedidoAEditar.InvitacionPedido = pedidoEditado.InvitacionPedido;
            pedidoAEditar.InvitacionPedidoGustoEmpanadaUsuario = pedidoEditado.InvitacionPedidoGustoEmpanadaUsuario;
            pedidoAEditar.NombreNegocio = pedidoEditado.NombreNegocio;
            pedidoAEditar.PrecioDocena = pedidoEditado.PrecioDocena;
            pedidoAEditar.PrecioUnidad = pedidoEditado.PrecioUnidad;

            ctx.SaveChanges();
        }

        public int ObtenerEnviarInvitacion(FormCollection form)
        {
            int envioInvitacion = int.Parse(form["enviarInvitacion"]);

            return envioInvitacion;
        }

        public Boolean InvitacionPedidoUsuarioIsTrue(int idPedido, Usuario usuario)
        {
            var query = (from ip in ctx.InvitacionPedido
                         where ip.IdUsuario == usuario.IdUsuario && ip.IdPedido == idPedido
                         select ip).ToList();

            if (query.Count > 0)
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

        public Pedido ObtenerPedidoPorId(int id)
        {
            var query = (from p in ctx.Pedido
                         where p.IdPedido == id
                         select p).FirstOrDefault();

            return query;
        }
    }
}