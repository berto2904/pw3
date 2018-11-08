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
            ValidarUsuarioSesion();
            return View();
        }
        [HttpPost]
        public ActionResult Iniciar(Pedido pedido)
        {
            ValidarUsuarioSesion();

            return View();
        }

        public ActionResult Iniciar(int id)
        {
            ValidarUsuarioSesion();
            return View();
        }

        public ActionResult Iniciado(int id)
        {
            return View();
        }

        public ActionResult Lista()
        {
            ValidarUsuarioSesion();
            try
            {
                usuarioLoguedado = ps.BuscarUsuarioById(Convert.ToInt32(Session["usuario"]));
                ViewBag.ListaPedidos = ps.ListarPedidosByUsuario(usuarioLoguedado);
                return View(usuarioLoguedado);
            }
            catch (TargetException)
            {

                TempData["mensaje"] = "Debes <a href='/Home/Login'>Iniciar Sesion</a>";
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult Editar(int id)
        {
            ValidarUsuarioSesion();
            try
            {
                if (!ps.PedidoUsuarioResponsableIsTrue(id, usuarioLoguedado))
                {
                    TempData["mensaje"] = "Acceso invalido";
                    return RedirectToAction("Error", "Home");
                }

                return View();

            }
            catch (TargetException)
            {
                TempData["mensaje"] = "Debes Iniciar Sesion";
                return RedirectToAction("Error", "Home");
            }

        }

        public ActionResult Eliminar(int id)
        {
            ValidarUsuarioSesion();
            if (!ps.PedidoUsuarioResponsableIsTrue(id, usuarioLoguedado))
            {
                TempData["mensaje"] = "Acceso invalido";
                return RedirectToAction("Error", "Home");
            }
            return View();
        }

       public ActionResult Elegir(int id)
        {
            ValidarUsuarioSesion();
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