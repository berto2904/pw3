using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrabajoPracticoPw3.Models;

namespace TrabajoPracticoPw3.Services
{
    public class HomeService
    {
        TPEntities ctx = new TPEntities();

        public Usuario BuscarUsuario(Usuario usuario)
        {
            Usuario usuarioEncontrado = ctx.Usuario.SingleOrDefault(x => x.Email == usuario.Email && x.Password == usuario.Password);
            return usuarioEncontrado;
        }
    }
}