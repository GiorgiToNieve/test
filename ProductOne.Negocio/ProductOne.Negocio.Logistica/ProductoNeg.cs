using ProductOne.Datos.ProductOne.Datos.Logistica;
using ProductOne.Entidad.ProductOne.Entidad.Logistica;
using ProductOne.Negocio.ProductOne.Negocio.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductOne.Negocio.ProductOne.Negocio.Logistica
{
	public class ProductoNeg : BaseNegocio
	{
		#region Singleton

		private static readonly Lazy<ProductoNeg> instance =
			new Lazy<ProductoNeg>(() => new ProductoNeg());

		private ProductoNeg()
		{
		}

		public static ProductoNeg Instance
		{
			get { return instance.Value; }
		}

		#endregion

		public void Guardar(Producto oProducto, bool bValidar = true)
		{
			try
			{
				if (bValidar)
					Validar(oProducto);
				ProductoAdo.Instance.Guardar(oProducto);

			}
			catch (Exception)
			{

				throw;
			}
		}

		public Task<bool> GuardarAsync(List<Producto> LstProducto)
		{
			try
			{
				return Task.Run(() => ProductoNeg.Instance.Guardar(LstProducto));
			}
			catch (Exception)
			{

				throw;
			}
		}

		public bool Guardar(List<Producto> LstProducto)
		{
			try
			{
				foreach (var item in LstProducto)
					ProductoAdo.Instance.Guardar(item);
				
				return true;
			}
			catch (Exception)
			{
				throw;
			}
		}


		/// <summary>
		/// consulta de forma asyncron
		/// </summary>
		/// <param name="parametros"></param>
		/// <returns></returns>
		public Task<List<Producto>> ConsultarAsync(Dictionary<string, object> parametros = null)
		{
			try
			{
				return Task.Run(() => ProductoAdo.Instance.Consultar(parametros,"*"));
			}
			catch (Exception)
			{
				throw;
			}
		}

		private void Validar(Producto oProducto)
		{
			StringBuilder objSb = new StringBuilder();

			if (oProducto.nProTipo <= 0)
				objSb.AppendLine("- Debe Seleccionar Tipo");

			if (oProducto.UnidadMedida_Id <= 0)
				objSb.AppendLine("- Debe Seleccionar Unidad de Medida");


			if (oProducto.sProNombre == "")
				objSb.AppendLine("- Debe ingresar Nombre");

			if (oProducto.Empresa_Id < 1)
				objSb.AppendLine("- La empresa no existe");

			if (!objSb.ToString().Equals(""))
				throw new Exception("Se han producido errores al intentar guardar : " + "\n" + objSb.ToString());
		}

		public void Eliminar(int Producto_Id)
		{
			try
			{
				ProductoAdo.Instance.Eliminar(Producto_Id);
			}

			catch (Exception)
			{
				throw;
			}
		}

		public void Restaurar(int Producto_Id)
		{
			try
			{
				ProductoAdo.Instance.Restaurar(Producto_Id);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public Producto Consultar(int Producto_Id, string columnas = "*")
		{
			try
			{
				return ProductoAdo.Instance.Consultar(Producto_Id, columnas);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public List<Producto> Consultar(Dictionary<string, object> parametros = null)
		{
			try
			{
				return ProductoAdo.Instance.Consultar(parametros);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public List<Producto> Consultar(Dictionary<string, object> dctParametros = null,
			string sColumnas = "*", string sOrderBy = "")
		{
			try
			{
				return ProductoAdo.Instance.Consultar(dctParametros, sColumnas, sOrderBy);
			}
			catch (Exception)
			{
				throw;
			}
		}

	}
}
