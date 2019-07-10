using ProductOne.Datos.ProductOne.Datos.Base;
using ProductOne.Entidad.ProductOne.Entidad.Comunes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductOne.Datos.ProductOne.Datos.Comunes
{
	public class EmpresaAdo : BaseAdo<Empresa>
	{
		#region "Singleton"

		private static readonly Lazy<EmpresaAdo> instance = new Lazy<EmpresaAdo>(() => new EmpresaAdo());

		private EmpresaAdo()
		{
		}

		public static EmpresaAdo Instance
		{
			get
			{
				return instance.Value;
			}
		}

		#endregion

	}
}