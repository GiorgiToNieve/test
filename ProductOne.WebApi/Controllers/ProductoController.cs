using ProductOne.Entidad.ProductOne.Entidad.Logistica;
using ProductOne.Negocio.ProductOne.Negocio.Logistica;
using ProductOne.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ProductOne.WebApi.Controllers
{

	[RoutePrefix("servicio/Producto")]
	public class ProductoController : ApiController
    {
		[HttpGet]
		public async Task<List<Producto>> Get()
		{
			try
			{
				var Parametros = new Dictionary<string, object>();

				return await ProductoNeg.Instance.ConsultarAsync(Parametros);
			}
			catch (Exception)
			{

				throw;
			}
		}

		// GET: api/Producto/5
		public string Get(int id)
        {
            return "value";
        }

		// POST: api/Producto
		[ResponseType(typeof(List<Producto>))]
		public async Task<int> Post(List<Producto> LstProducto)
        {

			try
			{
				if (LstProducto != null && LstProducto.Count > 0)
				{
					bool result = true;

					result = await ProductoNeg.Instance.GuardarAsync(LstProducto);

					if (result)
					{
						return Enumerador.RESPUESTA_INSERTADO_BD;
					}
				}

				return Enumerador.RESPUESTA_ERROR_INSERTADO_BD;
			}
			catch (Exception ex)
			{
				return Enumerador.RESPUESTA_ERROR_INSERTADO_BD;
			}
		}

        // PUT: api/Producto/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Producto/5
        public void Delete(int id)
        {
        }
    }
}
