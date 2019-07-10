using ProductOne.Datos.ProductOne.Datos.Base;
using ProductOne.Entidad.ProductOne.Entidad.Logistica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductOne.Datos.ProductOne.Datos.Logistica
{
	public class ProductoAdo : BaseAdo<Producto>
	{
		#region "Singleton"

		private static readonly Lazy<ProductoAdo> instance =
			new Lazy<ProductoAdo>(() => new ProductoAdo());

		private ProductoAdo()
		{
		}

		public static ProductoAdo Instance
		{
			get
			{
				return instance.Value;
			}
		}

		#endregion

	}
}
