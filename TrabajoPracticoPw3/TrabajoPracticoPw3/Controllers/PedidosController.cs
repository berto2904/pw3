using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrabajoPracticoPw3.Controllers
{
    public class PedidosController : Controller
    {
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
            return View();
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