using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using TrabajoPracticoPw3.Models;
using TrabajoPracticoPw3.Services;

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
                return RedirectToAction("Login", "Home");
            }
            //ViewBag.ListaDeGustos = new MultiSelectList(ps.ObtenerGustoDeEmpanadasList(), "IdGustoEmpanada", "Nombre");
            //ViewBag.ListaDeUsuarios = new MultiSelectList(ps.ObtenerUsuarioList(), "IdUsuario", "Email");

            ViewBag.ListaDeGustos = ps.ObtenerGustoDeEmpanadasList();
            ViewBag.ListaDeUsuarios = ps.ObtenerUsuarioList(usuarioLoguedado);
            return View();
        }

        [HttpPost]
        public ActionResult Iniciar(FormCollection form)
        {
            int idPedido = ps.IniciarService(form,usuarioLoguedado);
            if (idPedido != 0)
            {
                TempData["mensaje"] = "El pedido se inició";
            }
            return RedirectToAction("Iniciado", new {id = idPedido });
        }

        public ActionResult Iniciar(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
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
            return View();
        }

        public ActionResult Lista()
        {
            try
            {
                usuarioLoguedado = ps.BuscarUsuarioById(Convert.ToInt32(Session["usuario"]));
                ViewBag.ListaPedidos = ps.ListarPedidosByUsuario(usuarioLoguedado);
                return View(usuarioLoguedado);
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

            return View();
        }

        public ActionResult Eliminar(int id)
        {
            if (!ps.PedidoUsuarioResponsableIsTrue(id, usuarioLoguedado))
            {
                TempData["mensaje"] = "Acceso invalido";
                return RedirectToAction("Error", "Home");
            }
            ps.EliminarService(id);
            return View();
        }

       public ActionResult Elegir(int id)
        {
            if (!ps.InvitacionPedidoUsuarioIsTrue(id, usuarioLoguedado))
            {
                TempData["mensaje"] = "Acceso invalido";
                return RedirectToAction("Error","Home");
            }

            return View();
        }

        public ActionResult Detalle()
        {
            return View();
        }

        public ActionResult ValidarUsuarioSesion()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return null;
        }

    }
}