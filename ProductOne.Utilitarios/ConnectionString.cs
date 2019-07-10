using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductOne.Utilitarios
{
	public static class ConnectionString
	{
		public static string Conn = ConfigurationManager.ConnectionStrings["conexion_BD"].ConnectionString;
	}
}
