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
        static string redirectActionDefault;
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string redirigir)
        {
            redirectActionDefault = redirigir;

            if (Session["usuario"] != null)
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
                if (redirectActionDefault != null)
                {
                    try
                    {
                        string[] splited = redirectActionDefault.Split('/');
                        if (splited[3] == null)
                        {
                            return RedirectToAction(splited[2], splited[1]);
                        }
                        return RedirectToAction(splited[2], splited[1], new {id= splited[3]});
                    }
                    catch (IndexOutOfRangeException ex)
                    {
                        throw new Exception("El indice está fuera de rango", ex);
                    }
                }
                return RedirectToAction("Lista","Pedidos");
                //return RedirectToAction("Lista","Pedidos",new { idUsuario = Session["usuario"] });
            }
            else
            {
                ViewBag.mensaje = "Usuario y/o Contraseña inválidos";
            }
            return View();
        }

        // GET: Error
        public ActionResult Error(int error = 0)
        {
            switch (error)
            {
                case 505:
                    ViewBag.Title = "Ocurrio un error inesperado";
                    ViewBag.Description = "Esto es muy vergonzoso, esperemos que no vuelva a pasar ..";
                    break;

                case 404:
                    ViewBag.Title = "Página no encontrada";
                    ViewBag.Description = "La URL que está intentando ingresar no existe";
                    break;

                default:
                    ViewBag.Title = "Página no encontrada";
                    ViewBag.Description = "Algo salio muy mal :( ..";
                    break;
            }

            return View("~/views/Home/Error.cshtml");
        }

    public ActionResult Logout(Usuario usuario)
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}