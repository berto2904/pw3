using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TrabajoPracticoPw3.Models;
using Newtonsoft.Json;
using System.Web.Http.Cors;

namespace TrabajoPracticoPw3.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PedidoController : ApiController
    {
        TPEntities ctx = new TPEntities();
        // GET api/values
        public string Get()
        {
            List<Pedido> listaPedidos = ctx.Pedido.ToList();
            string json = JsonConvert.SerializeObject(listaPedidos);
            return json;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
