using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrabajoPracticoPw3.Models;
using TrabajoPracticoPw3.Services;

namespace TrabajoPracticoPw3.Controllers
{
    public class PedidosController : Controller
    {
        PedidoService ps = new PedidoService();
        // GET: Pedidos
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Iniciar()
        {
            return View();
        }

        public ActionResult Iniciar(int id)
        {
            return View();
        }

        public ActionResult Iniciado(int id)
        {
            return View();
        }

        public ActionResult Lista()
        {
            if(Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int idUsuario = Convert.ToInt32(Session["usuario"]);
            Usuario usuario = ps.BuscarUsuarioById(idUsuario);
            ViewBag.ListaPedidos = ps.ListarPedidosByIdUsuario(idUsuario);
            return View(usuario);
        }

        public ActionResult Editar()
        {
            return View();
        }

        public ActionResult Eliminar()
        {
            return View();
        }

        public ActionResult Elegir()
        {
            return View();
        }

        public ActionResult Detalle()
        {
            return View();
        }

    }
}