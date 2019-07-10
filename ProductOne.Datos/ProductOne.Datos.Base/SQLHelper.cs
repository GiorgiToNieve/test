using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductOne.Datos.ProductOne.Datos.Base
{
	public static class SQLHelper
	{

		private static string sDatabase = string.Empty;
		private static SqlParameterCollection parameters = null;

		public static int ExecuteUpdate(string sSql, string sCadenaConexion)
		{
			try
			{
				using (SqlConnection oConnection = new SqlConnection(sCadenaConexion))
				{
					oConnection.Open();
					using (SqlTransaction oTransaction = oConnection.BeginTransaction())
					{
						using (SqlCommand oCommand = new SqlCommand(sSql, oConnection, oTransaction))
						{
							sDatabase = oCommand.Connection.Database;
							oCommand.CommandType = CommandType.Text;

							int nFilasAfectadas = oCommand.ExecuteNonQuery();
							oTransaction.Commit();
							return nFilasAfectadas;
						}
					}
				}
			}
			catch (SqlException ex)
			{
				throw new Exception(ex.Message);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static int ExecuteUpdateProcedure(string sProcedimiento, Dictionary<string, object> dctParametros = null, string sCadenaConexion = "")
		{
			try
			{
				using (SqlConnection oConnection = new SqlConnection(sCadenaConexion))
				{
					oConnection.Open();
					using (SqlTransaction oTransaction = oConnection.BeginTransaction())
					{
						using (SqlCommand oCommand = new SqlCommand(sProcedimiento, oConnection, oTransaction))
						{
							sDatabase = oCommand.Connection.Database;
							oCommand.CommandType = CommandType.StoredProcedure;
							oCommand.CommandTimeout = 500;
							if (dctParametros != null)
							{
								foreach (var oParametro in dctParametros)
								{
									var oSqlParametro = new SqlParameter("@" + oParametro.Key, oParametro.Value);
									if (!(oParametro.Value is DateTime))
									{
										oSqlParametro.SqlDbType = SqlDbType.VarChar;
									}
									oSqlParametro.Direction = ParameterDirection.Input;
									oCommand.Parameters.Add(oSqlParametro);
								}
								parameters = oCommand.Parameters;
							}

							int nFilasAfectadas = oCommand.ExecuteNonQuery();
							oTransaction.Commit();
							return nFilasAfectadas;
						}
					}
				}
			}
			catch (SqlException ex)
			{
				throw new Exception(ex.Message);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static int ExecuteUpdateProcedure(string sProcedimiento, List<SqlParameter> lstParametros = null, string sCadenaConexion = "")
		{
			try
			{
				using (SqlConnection oConnection = new SqlConnection(sCadenaConexion))
				{
					oConnection.Open();
					using (SqlTransaction oTransaction = oConnection.BeginTransaction())
					{
						using (SqlCommand oCommand = new SqlCommand(sProcedimiento, oConnection, oTransaction))
						{
							sDatabase = oCommand.Connection.Database;
							oCommand.CommandType = CommandType.StoredProcedure;
							oCommand.CommandTimeout = 300;
							if (lstParametros != null)
							{
								foreach (var oSqlParametro in lstParametros)
								{
									oCommand.Parameters.Add(oSqlParametro);
								}
								parameters = oCommand.Parameters;
							}

							int nFilasAfectadas = oCommand.ExecuteNonQuery();
							oTransaction.Commit();
							return nFilasAfectadas;
						}
					}
				}
			}
			catch (SqlException ex)
			{
					throw new Exception(ex.Message);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static object ExecuteEscalar(string sSql, string sCadenaConexion)
		{
			try
			{
				using (SqlConnection oConnection = new SqlConnection(sCadenaConexion))
				{
					oConnection.Open();
					using (SqlTransaction oTransaction = oConnection.BeginTransaction())
					{
						using (SqlCommand oCommand = new SqlCommand(sSql, oConnection, oTransaction))
						{
							sDatabase = oCommand.Connection.Database;
							oCommand.CommandType = CommandType.Text;

							var oEscalar = oCommand.ExecuteScalar();

							oTransaction.Commit();
							return oEscalar;
						}
					}
				}
			}
			catch (SqlException ex)
			{
					throw new Exception(ex.Message);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static object ExecuteEscalar(string sProcedimiento, Dictionary<string, object> dctParametros = null, string sCadenaConexion = "")
		{
			try
			{
				using (SqlConnection oConnection = new SqlConnection(sCadenaConexion))
				{
					oConnection.Open();
					using (SqlTransaction oTransaction = oConnection.BeginTransaction())
					{
						using (SqlCommand oCommand = new SqlCommand(sProcedimiento, oConnection, oTransaction))
						{
							sDatabase = oCommand.Connection.Database;
							oCommand.CommandType = CommandType.StoredProcedure;
							oCommand.CommandTimeout = 1000;
							if (dctParametros != null)
							{
								foreach (var oParametro in dctParametros)
								{
									var oSqlParametro = new SqlParameter("@" + oParametro.Key, oParametro.Value);
									oSqlParametro.Direction = ParameterDirection.Input;
									if (!(oParametro.Value is DateTime))
									{
										oSqlParametro.SqlDbType = SqlDbType.VarChar;
									}
									oCommand.Parameters.Add(oSqlParametro);
								}
								parameters = oCommand.Parameters;
							}

							var oEscalar = oCommand.ExecuteScalar();
							oTransaction.Commit();
							return oEscalar;
						}
					}
				}
			}
			catch (SqlException ex)
			{
					throw new Exception(ex.Message);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static object ExecuteEscalarProcedure(string sProcedimiento, List<SqlParameter> lstParametros = null, string sCadenaConexion = "")
		{
			try
			{
				using (SqlConnection oConnection = new SqlConnection(sCadenaConexion))
				{
					oConnection.Open();
					using (SqlTransaction oTransaction = oConnection.BeginTransaction())
					{
						using (SqlCommand oCommand = new SqlCommand(sProcedimiento, oConnection, oTransaction))
						{
							sDatabase = oCommand.Connection.Database;
							oCommand.CommandType = CommandType.StoredProcedure;
							oCommand.CommandTimeout = 900;
							if (lstParametros != null)
							{
								foreach (var oParam in lstParametros)
								{
									oCommand.Parameters.Add(oParam);
								}
								parameters = oCommand.Parameters;
							}

							var escalar = oCommand.ExecuteScalar();
							oTransaction.Commit();
							return escalar;
						}
					}
				}
			}
			catch (SqlException ex)
			{
				if ((int)ex.State != 2)
					throw new Exception(ex.Message);
				else
					throw;
			}
			catch (Exception)
			{
				throw;
			}
		}


		/// <summary>
		/// [vnieve] Ejecuta el procedimiento con los parametros y conexion pasados y devuelve un objeto para convertir segun necesidad
		/// No utiliza transacciones
		/// </summary>
		/// <param name="sProcedimiento"></param>
		/// <param name="dctParametros"></param>
		/// <param name="sCadenaConexion"></param>
		/// <returns></returns>
		public static object EjecutarProcedimiento(string sProcedimiento, Dictionary<string, object> dctParametros = null, string sCadenaConexion = "")
		{
			try
			{
				using (var oConnection = new SqlConnection(sCadenaConexion))
				{
					oConnection.Open();

					using (var oCommand = new SqlCommand(sProcedimiento, oConnection))
					{
						sDatabase = oCommand.Connection.Database;
						oCommand.CommandType = CommandType.StoredProcedure;
						oCommand.CommandTimeout = 1000;
						if (dctParametros != null)
						{
							foreach (var oParametro in dctParametros)
							{
								var oSqlParametro = new SqlParameter("@" + oParametro.Key, oParametro.Value);
								oSqlParametro.Direction = ParameterDirection.Input;
								if (!(oParametro.Value is DateTime))
								{
									oSqlParametro.SqlDbType = SqlDbType.VarChar;
								}
								oCommand.Parameters.Add(oSqlParametro);
							}
							parameters = oCommand.Parameters;
						}

						return oCommand.ExecuteScalar();

					}

				}
			}
			catch (SqlException ex)
			{
				if ((int)ex.State != 2)
					throw new Exception(ex.Message);
				else
					throw;
			}
			catch (Exception)
			{
				throw;
			}
		}





		public static List<T> ExecuteQuery<T>(string sSql, string sCadenaConexion)
		{
			try
			{
				using (SqlConnection oConnection = new SqlConnection(sCadenaConexion))
				{
					using (SqlCommand oCommand = new SqlCommand(sSql, oConnection))
					{
						sDatabase = oCommand.Connection.Database;
						oCommand.CommandType = CommandType.Text;
						oConnection.Open();

						return GenericHelper.GetAsList<T>(oCommand.ExecuteReader());
					}
				}
			}
			catch (SqlException ex)
			{
				if (ex.Number == 2601 || ex.Number == 2627 || ex.Number == 547)
				{
					ex.Data.Add("Tipo", "DuplicidadData");
					throw;
				}
				else
				{
					throw new Exception(ex.Message);
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public static List<T> ExecuteQueryProcedureListParam<T>(string sProcedimiento, List<SqlParameter> lstParametros = null, string sCadenaConexion = "")
		{
			try
			{
				using (SqlConnection oConnection = new SqlConnection(sCadenaConexion))
				{
					using (SqlCommand oCommand = new SqlCommand(sProcedimiento, oConnection))
					{
						sDatabase = oCommand.Connection.Database;
						oCommand.CommandType = CommandType.StoredProcedure;
						oCommand.CommandTimeout = 90;

						if (lstParametros != null && lstParametros.Count > 0)
						{
							foreach (var oParam in lstParametros)
							{
								oCommand.Parameters.Add(oParam);
							}
						}
						parameters = oCommand.Parameters;
						oConnection.Open();

						return GenericHelper.GetAsList<T>(oCommand.ExecuteReader());
					}
				}
			}
			catch (SqlException ex)
			{
				

				if ((int)ex.State != 2)
					throw new Exception(ex.Message);
				else
					throw;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public static List<T> ExecuteQueryProcedure<T>(string sProcedimiento, Dictionary<string, object> dctParametros = null, string sCadenaConexion = "")
		{
			try
			{
				using (SqlConnection oConnection = new SqlConnection(sCadenaConexion))
				{
					using (SqlCommand oCommand = new SqlCommand(sProcedimiento, oConnection))
					{
						sDatabase = oCommand.Connection.Database;
						oCommand.CommandType = CommandType.StoredProcedure;
						oCommand.CommandTimeout = 9000;
						if (dctParametros != null)
						{
							foreach (var oParametro in dctParametros)
							{
								var oSqlParametro = new SqlParameter("@" + oParametro.Key, oParametro.Value);

								if (!(oParametro.Value is DateTime))
								{
									oSqlParametro.SqlDbType = SqlDbType.VarChar;
								}

								oSqlParametro.Direction = ParameterDirection.Input;
								oCommand.Parameters.Add(oSqlParametro);
							}
						}

						parameters = oCommand.Parameters;
						oConnection.Open();

						return GenericHelper.GetAsList<T>(oCommand.ExecuteReader());
					}
				}
			}
			catch (SqlException ex)
			{

				if ((int)ex.State != 2)
					throw new Exception(ex.Message);
				else
					throw;
			}
			catch (Exception)
			{
				throw;
			}
		}


		


		

	}
}
