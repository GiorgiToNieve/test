using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProductOne.WebApi.Controllers
{
    public class EmpresaController : ApiController
    {
        // GET: api/Empresa
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Empresa/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Empresa
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Empresa/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Empresa/5
        public void Delete(int id)
        {
        }
    }
}
