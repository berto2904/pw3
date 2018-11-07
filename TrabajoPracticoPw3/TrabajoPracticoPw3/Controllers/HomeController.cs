using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TrabajoPracticoPw3.Models;
using TrabajoPracticoPw3.Services;

namespace TrabajoPracticoPw3.Controllers
{
    public class HomeController : Controller
    {
        HomeService hs = new HomeService();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            if(Session["usuario"] != null)
            {
                return RedirectToAction("Lista", "Pedidos");
            }
            return View();

        }

        [HttpPost]
        public ActionResult Login(Usuario usuario)
        {
            Usuario usuarioEncontrado = hs.BuscarUsuario(usuario);

            if (usuarioEncontrado != null)
            {
                Session["usuario"] = usuarioEncontrado.IdUsuario;
                return RedirectToAction("Lista","Pedidos");
                //return RedirectToAction("Lista","Pedidos",new { idUsuario = Session["usuario"] });
            }
            else
            {
                ViewBag.mensaje = "Usuario invalido";
            }
            return View();
        }

        public ActionResult Error()
        {
            ViewBag.Mensaje = TempData["mensaje"];
            return View();
        }

        public ActionResult Logout(Usuario usuario)
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}