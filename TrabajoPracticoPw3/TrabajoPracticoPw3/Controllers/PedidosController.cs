using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using TrabajoPracticoPw3.Models;
using TrabajoPracticoPw3.Services;
using PagedList;

namespace TrabajoPracticoPw3.Controllers
{
    public class PedidosController : Controller
    {
        PedidoService ps = new PedidoService();
        static Usuario usuarioLoguedado = new Usuario();

        // GET: Pedidos
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Iniciar()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home", new { redirigir = "/Pedidos/Iniciar/" });
            }
            //ViewBag.ListaDeGustos = new MultiSelectList(ps.ObtenerGustoDeEmpanadasList(), "IdGustoEmpanada", "Nombre");
            //ViewBag.ListaDeUsuarios = new MultiSelectList(ps.ObtenerUsuarioList(), "IdUsuario", "Email");

            ViewBag.ListaDeGustos = ps.ObtenerGustoDeEmpanadasList();
            ViewBag.ListaDeUsuarios = ps.ObtenerUsuarioList(usuarioLoguedado);
            return View();
        }

        [HttpGet]
        public ActionResult IniciarDesde(int id)
        {
            //ValidarUsuarioSesion();

            Pedido PedidoBase = ps.ObtenerPedidoPorId(id);

            ViewBag.ListaDeGustos = ps.ObtenerGustoDeEmpanadasList();
            ViewBag.ListaDeUsuarios = ps.ObtenerUsuarioList(usuarioLoguedado);

            return View(PedidoBase);
        }

        [HttpPost]
        public ActionResult Iniciar(FormCollection form)
        {
            int idPedido = ps.IniciarService(form, usuarioLoguedado);
            if (idPedido != 0)
            {
                TempData["mensaje"] = "El pedido se inició";
            }
            return RedirectToAction("Iniciado", new { id = idPedido });
        }

        public ActionResult Iniciar(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home", new { redirigir = "/Pedidos/Iniciar/" + id });
            }
            return View();
        }

        public ActionResult Iniciado(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (!ps.PedidoUsuarioResponsableIsTrue(id, usuarioLoguedado))
            {
                TempData["mensaje"] = "Acceso invalido";
                return RedirectToAction("Error", "Home");
            }
            ViewBag.Mensaje = TempData["mensaje"];
            return View(ps.ObtenerPedidoById(id));
        }

        public ActionResult Lista(int? pagePos)
        {
            int pageNumber = (pagePos ?? 1);
            try
            {
                usuarioLoguedado = ps.BuscarUsuarioById(Convert.ToInt32(Session["usuario"]));
                List<Pedido> listaPedidos = ps.ListarPedidosByUsuario(usuarioLoguedado);
                ViewBag.Usuario = usuarioLoguedado;
                return View(listaPedidos.ToPagedList(pageNumber, 5));
            }
            catch (TargetException)
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult Editar(int id)
        {
            //ValidarUsuarioSesion();
            if (!ps.PedidoUsuarioResponsableIsTrue(id, usuarioLoguedado))
            {
                TempData["mensaje"] = "Acceso invalido";
                return RedirectToAction("Error", "Home");
            }

            Pedido PedidoAEditar = ps.ObtenerPedidoPorId(id);

            if (PedidoAEditar.EstadoPedido.Nombre == "Cerrado")
            {
                TempData["mensaje"] = "El pedido se encuentra Cerrado";
                //TODO: Crear pantalla detalle
                return RedirectToAction("Detalle");
            }

            ViewBag.ListaDeGustos = ps.ObtenerGustoDeEmpanadasList();
            ViewBag.ListaDeUsuarios = ps.ObtenerUsuarioList(usuarioLoguedado);

            return View(PedidoAEditar);
        }

        [HttpPost]
        public ActionResult Editar(FormCollection form)
        {
            int envioInvitacion = ps.ObtenerEnviarInvitacion(form);

            //Pedido pedidoEditado = ps.ObtenerPedidoDesdeFormCollection(form);

            //Pedido pedidoAEditar = ps.ObtenerPedidoPorId(pedidoEditado.IdPedido);

            if (envioInvitacion == 1)
            {
                // no se envia invitacion
            }
            else
            {
                // se envia invitacion a usuariosAEnviarInvitacion
                Pedido pedido = ps.ObtenerPedidoDesdeFormCollection(form);
                List<Usuario> usuariosAEnviarInvitacion = ps.DeterminarEnviosDeInvitacionDesdeFormCollection(form);

                ps.EnviarInvitacionesDesdeUnaListaDeUsuarios(usuariosAEnviarInvitacion, pedido);
            }

            ps.ActualizarValoresDeUnPedidoDesdeFormCollection(form);
            usuarioLoguedado = ps.BuscarUsuarioById(Convert.ToInt32(Session["usuario"]));
            ViewBag.Usuario = usuarioLoguedado;
            //return View("Lista", listaPedidos.ToPagedList(pageNumber, 5));
            return RedirectToAction("Lista");
        }

        public ActionResult Eliminar(int id)
        {
            if (!ps.PedidoUsuarioResponsableIsTrue(id, usuarioLoguedado))
            {
                TempData["mensaje"] = "Acceso invalido";
                return RedirectToAction("Error", "Home");
            }
            ps.EliminarService(id);
            return RedirectToAction("Lista");
        }

        public ActionResult Elegir(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home", new { redirigir = "/Pedidos/Elegir/" + id });
            }
            usuarioLoguedado = ps.BuscarUsuarioById(Convert.ToInt32(Session["usuario"]));
            if (!ps.InvitacionPedidoUsuarioIsTrue(id, usuarioLoguedado))
            {
                TempData["mensaje"] = "Acceso invalido";
                return RedirectToAction("Error", "Home");
            }
            Pedido pedido = ps.ObtenerPedidoById(id);
            ViewBag.IdUsuario = usuarioLoguedado.IdUsuario;
            ViewBag.TokenInvitacion = pedido.InvitacionPedido.Where(i => i.IdPedido == pedido.IdPedido && i.IdUsuario == usuarioLoguedado.IdUsuario).FirstOrDefault().Token.ToString();
            return View(pedido);
        }

        [HttpPost]
        public ActionResult Elegir(FormCollection form)
        {
            int idPedido = ps.ElegirService(form, usuarioLoguedado);
            if (idPedido != 0)
            {
                TempData["mensaje"] = "has elegido gustos";
            }

            return RedirectToAction("Lista", new { id = idPedido });
        }

        public ActionResult ValidarUsuarioSesion()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return null;
        }

        public ActionResult Finalizar(int id)
        {
            ps.FinalizarPedidoPorId(id);

            return RedirectToAction("Lista");
        }

        public ActionResult Detalle(int id)
        {
            Pedido pedido = ps.ObtenerPedidoById(id);

            ViewBag.ListaDeGustos = ps.ObtenerGustoDeEmpanadasList();
            ViewBag.ListaDeUsuarios = ps.ObtenerUsuarioList(usuarioLoguedado);

            return View(pedido);
        }
    }
}