using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductOne.Utilitarios
{
	public static class Enumerador
	{
		public static int ESTADO_ACTIVO = 1;
		public static int ESTADO_INACTIVO = 0;

		public static int MONEDA_NACIONAL_SOLES_ID = 1;
		public static int MONEDA_EXTRANJERA_DOLARES_ID = 2;
		public static int ESTADO_PRODUCCION = 1;

		/// <summary>
		/// se ha insertado en la base de datos
		/// </summary>
		public static int RESPUESTA_INSERTADO_BD = 0;

		/// <summary>
		/// no se pudo insertar en la base de datos
		/// </summary>
		public static int RESPUESTA_ERROR_INSERTADO_BD = -1;

	}
}
