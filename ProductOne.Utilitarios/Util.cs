using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Speech.Synthesis;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using System.Data;
using System.Net.Sockets;

namespace ProductOne.Utilitarios
{
	public static class Util
	{
		

		/// <summary>
		/// Devuelve el ip del cliente
		/// </summary>
		/// <returns></returns>
		public static string ObtenerIPCliente()
		{
			var host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (var MiIP in host.AddressList)
			{
				if (MiIP.AddressFamily == AddressFamily.InterNetwork)
				{
					return MiIP.ToString();
				}
			}
			throw new Exception("Sin Información de red dirección IPv4");
		}


		public static bool ValidaCorreo(string sCadena)
		{
			return true;
		}

		public static bool ValidaNumero(String sCadena)
		{
			return Regex.IsMatch(sCadena, "[0-9]+$");
		}

		public static bool ValidaPaginaWeb(String url)
		{
			if (url == null || url == "") return false;
			Regex urlRx =
				new Regex(
					@"^(http|ftp|https|www)(.://)?([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?$",
					RegexOptions.IgnoreCase);
			return urlRx.Match(url).Success;
		}


		/// <summary>
		/// Devuelve sin acentos la cadena ingresada
		/// parametros caneda="", 
		/// tipo=1 minuscula
		/// tipo=2 mayuscula
		/// </summary>
		/// <param name="cadena"></param>
		/// <param name="TipoCaracter"></param>
		/// <returns></returns>
		public static string QuitarAcentos(string cadena, int TipoCaracter = 0)
		{
			cadena = cadena.ToLower();

			Regex a = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
			Regex e = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
			Regex i = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
			Regex o = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
			Regex u = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
			Regex n = new Regex("[ñ|Ñ]", RegexOptions.Compiled);
			cadena = a.Replace(cadena, "a");
			cadena = e.Replace(cadena, "e");
			cadena = i.Replace(cadena, "i");
			cadena = o.Replace(cadena, "o");
			cadena = u.Replace(cadena, "u");
			cadena = n.Replace(cadena, "n");

			if (TipoCaracter == 1)
				cadena = cadena.ToUpper();
			else
			{
				if (TipoCaracter == 1)
					cadena = cadena.ToLower();
				else // devuelve la primera letra de la oracion en mayuscula y el resto
					 // minstulas
					cadena = Util.CapitalizarPrimeraLetra(cadena);
			}

			return cadena;
		}

		public static bool ValidaAlfanumerico(String sCadena)
		{
			return true;
		}

		public static String GetArchivoSufijo(String sRutaArchivo, String sExtension)
		{
			String sSufijo = DateTime.Now.TimeOfDay.ToString().Replace(":", "").Replace(".", "");
			return sRutaArchivo + "_" + sSufijo + "." + sExtension;
		}

		public static bool ContieneSoloLetras(String sCadena)
		{
			return Regex.IsMatch(sCadena, "[a-zA-Z]+$");
		}

		public static bool ContieneSoloNumerosYLetras(String sCadena)
		{
			return Regex.IsMatch(sCadena, "[0-9a-zA-Z]+$");
		}

		public static string SerializeXML<T>(T obj)
		{
			string returnXml;
			var serializer = new XmlSerializer(typeof(T));
			//using (var writer = new StringWriterUtf8())
			using (var writer = new StringWriterIso8859())
			{
				serializer.Serialize(new XmlTextWriter(writer), obj);
				returnXml = writer.ToString();
			}
			return returnXml;
		}

		public static T DeserializeXML<T>(string sXml)
		{
			XmlSerializer deserializer = new XmlSerializer(typeof(T));
			TextReader reader = new StringReader(sXml);
			object obj = deserializer.Deserialize(reader);
			T XmlData = (T)obj;
			reader.Close();
			return XmlData;
		}

		public static T DeserializeXML<T>(string sXml, string sRoot)
		{
			XmlSerializer deserializer = new XmlSerializer(typeof(T), new XmlRootAttribute(sRoot));
			TextReader reader = new StringReader(sXml);
			object obj = deserializer.Deserialize(reader);
			T XmlData = (T)obj;
			reader.Close();
			return XmlData;
		}

		public static T CopyValues<T>(T target, T source)
		{
			var t = typeof(T);

			var properties = t.GetProperties().Where(prop => prop.CanRead && prop.CanWrite);

			foreach (var prop in properties)
			{
				var value = prop.GetValue(source, null);
				if (value != null)
					prop.SetValue(target, value, null);
			}
			return target;
		}

		public static T CopyProperties<T>(this object source, T destination)
		{
			if (source == null || destination == null)
				throw new Exception("Source or/and Destination Objects are null");
			Type typeDest = destination.GetType();
			Type typeSrc = source.GetType();

			PropertyInfo[] srcProps = typeSrc.GetProperties();
			foreach (PropertyInfo srcProp in srcProps)
			{
				if (!srcProp.CanRead)
				{
					continue;
				}
				PropertyInfo targetProperty = typeDest.GetProperty(srcProp.Name);
				if (targetProperty == null)
				{
					continue;
				}
				//Se cambio temporalmente 20170907
				if (srcProp.Name.Contains("Serializable"))
				{
					continue;
				}
				if (!targetProperty.CanWrite)
				{
					continue;
				}
				if (targetProperty.GetSetMethod(true) != null && targetProperty.GetSetMethod(true).IsPrivate)
				{
					continue;
				}
				if ((targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) != 0)
				{
					continue;
				}
				if (!targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType))
				{
					continue;
				}
				// Passed all tests, lets set the value
				targetProperty.SetValue(destination, srcProp.GetValue(source, null), null);
			}
			return destination;
		}


		public static Image Base64ToImage(string base64String)
		{
			// Convert base 64 string to byte[]
			byte[] imageBytes = Convert.FromBase64String(base64String);
			// Convert byte[] to Image
			using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
			{
				Image image = Image.FromStream(ms, true);
				return image;
			}
		}

		public static string ImageToBase64(Image image, ImageFormat format)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				// Convert Image to byte[]
				image.Save(ms, format);
				byte[] imageBytes = ms.ToArray();

				// Convert byte[] to base 64 string
				string base64String = Convert.ToBase64String(imageBytes);
				return base64String;
			}
		}


		/// <summary>
		/// [vnieve] Devuelve la imagen en un arreglo de bytes[]
		/// </summary>
		/// <param name="imageIn"></param>
		/// <returns></returns>
		public static byte[] ImageToByteArray(Image imageIn)
		{
			try
			{
				using (var ms = new MemoryStream())
				{
					imageIn.Save(ms, ImageFormat.Png);
					return ms.ToArray();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static string DateTimeToString(DateTime dFecha, string sFormatoFecha = "yyyyMMdd")
		{
			if (sFormatoFecha.Equals("yyyyMMdd"))
			{
				char[] delim = { '/' };
				string[] arrFecha = dFecha.ToShortDateString().Split(delim);
				return (arrFecha[2] + arrFecha[1] + arrFecha[0] == "00010101"
					? ""
					: arrFecha[2] + arrFecha[1] + arrFecha[0]);
			}
			if (sFormatoFecha.Equals("dd/MM/yyyy"))
			{
				return (dFecha.ToString("dd/MM/yyyy").Contains("01/01/0001") ? "" : dFecha.ToString("dd/MM/yyyy"));
			}
			return "";
		}

		public static DateTime StringToDatetime(string sFecha, string sFormatoFecha = "dd/MM/yyyy")
		{
			try
			{
				if (sFormatoFecha == "dd/MM/yyyy")
				{
					char[] arDelimitador = { '/' };
					string[] arFecha = sFecha.Split(arDelimitador);
					return new DateTime(int.Parse(arFecha[2]), int.Parse(arFecha[1]), int.Parse(arFecha[0]));
				}
				return new DateTime();
			}
			catch (Exception)
			{
				return new DateTime();
			}
		}

		/// <summary>
		/// [ccerna 01/06/2016]
		/// Método para concatenar los elementos llave valor de un Dictionary
		/// </summary>
		/// <param name="dct">Dictionary que contiene los elementos llave valor</param>
		/// <param name="sDelimitadorElementos">Delimitador entre elementos llave valor</param>
		/// <param name="sDelimitadorxElemento">Delimitador x elemento que separa la llave del valor</param>
		/// <returns></returns>
		public static string DictionaryToString(Dictionary<string, object> dct, char sDelimitadorElementos = '&',
			char sDelimitadorxElemento = '=')
		{
			try
			{
				var oSb = new StringBuilder();
				if (dct != null)
				{
					if (dct.Count > 0)
					{
						bool bPrimerElemento = true;
						foreach (var kvpItem in dct)
						{
							if (bPrimerElemento)
							{
								bPrimerElemento = false;
							}
							else
							{
								oSb.Append(sDelimitadorElementos);
							}

							oSb.Append(kvpItem.Key + sDelimitadorxElemento + kvpItem);
						}
					}
					else
					{
						throw new Exception("El diccionario debe contener elementos");
					}
				}
				else
				{
					throw new Exception("El diccionario debe ser diferente de null");
				}
				return oSb.ToString();
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// [ccerna 01/06/2016]
		/// Método para transformar una cadena a un Dictionary (ej: QueryString)
		/// </summary>
		/// <param name="sCadena">Cadena que contiene los elementos llave valor</param>
		/// <param name="sDelimitadorElementos">Delimitador entre elementos llave valor</param>
		/// <param name="sDelimitadorxElemento">Delimitador x elemento que separa la llave del valor</param>
		/// <returns>Diccionario con los elementos contenidos en la cadena</returns>
		public static Dictionary<string, object> StringToDictionary(string sCadena, char sDelimitadorElementos = '&',
			char sDelimitadorxElemento = '=')
		{
			try
			{
				var dct = new Dictionary<string, object>();
				string[] arItems = sCadena.Split(sDelimitadorElementos);
				if (arItems.Length > 0)
				{
					foreach (string sKeyValueItem in arItems)
					{
						var arKeyValue = sKeyValueItem.Split(sDelimitadorxElemento);
						if (arKeyValue.Length == 2)
						{
							dct.Add(arKeyValue[0].Trim(), arKeyValue[1].Trim());
						}
						else
						{
							throw new Exception("Longitud de elemento llave valor no es válida (" +
												arKeyValue.Length + ")");
						}
					}
				}
				else
				{
					throw new Exception("No se pueden distinguir elementos dentro de la cadena");
				}
				return dct;
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// [rhoyos 20/10/2016]
		/// Método para obtener un valor del objeto Tag
		/// </summary>
		/// <param name="oTag">Objeto contenido en el control (Ejemplo:Form.Tag)</param>
		/// <param name="key">Nombre de la clave a recuperar</param> 
		/// <returns>Cadena recuperada del tag</returns>
		public static String ObtenerValorTag(object oTag, String key)
		{
			if (oTag != null)
			{
				if (oTag is String)
				{
					var dctTag = StringToDictionary(oTag.ToString());
					if (dctTag.ContainsKey(key))
					{
						return dctTag[key].ToString();
					}
				}
			}
			return String.Empty;
		}

		//nuevo get con dictionary
		public static String ObtenerValorDTag(this object oTag, String key, String porDefecto = "")
		{
			if (oTag != null)
			{
				var dctTag = new Dictionary<String, Object>();

				if (oTag is Dictionary<String, Object>)
				{
					dctTag = (Dictionary<String, Object>)oTag;
					if (dctTag.ContainsKey(key))
						return dctTag[key].ToString();
					return porDefecto;
				}
				if (oTag is String)
					dctTag = StringToDictionary(oTag.ToString());

				if (dctTag.ContainsKey(key))
					return dctTag[key].ToString();
				return porDefecto;
			}
			return porDefecto;
		}

		//public static void AddKey<T>(this T tag,String key, String val) where T:Dictionary<String,Object>
		public static Dictionary<String, Object> AgregarLlave(this Object oTag, String key, String val)
		{
			var dctTag = new Dictionary<String, Object>();
			if (oTag != null)
			{
				if (oTag is String)
					dctTag = StringToDictionary(oTag.ToString());
				else if (oTag is Dictionary<String, Object>)
				{
					dctTag = (Dictionary<String, Object>)oTag;
				}
				if (!dctTag.ContainsKey(key))
					dctTag.Add(key, val);
				else
					dctTag[key] = val;
			}
			else
				dctTag[key] = val;
			return dctTag;
		}
		//public static void AddKey<T>(this T tag,String key, String val) where T:Dictionary<String,Object>
		public static Dictionary<String, Object> AgregarLlaveT(this Object oTag, String key, Object val)
		{
			var dctTag = new Dictionary<String, Object>();
			if (oTag != null)
			{
				if (oTag is String)
					dctTag = StringToDictionary(oTag.ToString());
				else if (oTag is Dictionary<String, Object>)
				{
					dctTag = (Dictionary<String, Object>)oTag;
				}
			}
			if (!dctTag.ContainsKey(key))
				dctTag.Add(key, val);
			else
				dctTag[key] = val;

			return dctTag;
		}
		

		public class Fechas
		{
			public static DateTime FechaHoy()
			{
				return Convert.ToDateTime(DateTime.Now.ToString("s"));
			}
			public static int CalcularSemana(DateTime dFecha)
			{
				//int nAnio = Convert.ToInt32(this.txtPCaAnio.Text);
				int nAnio = dFecha.Date.Year;

				DateTime dPrimeroEnero = new DateTime(nAnio, 1, 1);
				DateTime dInicioSemanaCero = dPrimeroEnero;
				int nRestar = 1;
				if (dPrimeroEnero.DayOfWeek != DayOfWeek.Monday)
				{
					nRestar = 0;
					while (dInicioSemanaCero.DayOfWeek != DayOfWeek.Monday)
						dInicioSemanaCero = dInicioSemanaCero.AddDays(-1);
				}
				TimeSpan ts = (dFecha - dInicioSemanaCero);
				int nSemana = (ts.Days / 7) - nRestar;
				return nSemana;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="dFechaInicio"></param>
			/// <param name="dFechaFin"></param>
			/// <param name="sDatePart"></param>
			/// <returns></returns>
			public static int RestarFechas(DateTime dFechaInicio, DateTime dFechaFin, char sDatePart = 'd')
			{
				TimeSpan ts = dFechaInicio - dFechaFin;
				int nDiferencia = 0;
				if (sDatePart == 'd')
				{
					nDiferencia = ts.Days;
				}
				return nDiferencia;
			}
		}


		/// <summary>
		/// Metodos Metodos de Encriptar y desencriptar para una cadena.  
		/// </summary>
		public static string key = "ABCDEFG54669525PQRSTUVWXYZabcdef852846opqrstuvwxyz";

		public static string Encriptar(string cadena)
		{
			//arreglo de bytes donde guardaremos la llave
			byte[] keyArray;
			//arreglo de bytes donde guardaremos el texto
			//que vamos a encriptar
			byte[] Arreglo_a_Cifrar =
				Encoding.UTF8.GetBytes(cadena);

			//se utilizan las clases de encriptación
			//provistas por el Framework
			//Algoritmo MD5
			MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
			//se guarda la llave para que se le realice
			//hashing
			keyArray = hashmd5.ComputeHash(
				Encoding.UTF8.GetBytes(key));

			hashmd5.Clear();

			//Algoritmo 3DAS
			TripleDESCryptoServiceProvider tdes =
				new TripleDESCryptoServiceProvider();

			tdes.Key = keyArray;
			tdes.Mode = CipherMode.ECB;
			tdes.Padding = PaddingMode.PKCS7;

			//se empieza con la transformación de la cadena
			ICryptoTransform cTransform =
				tdes.CreateEncryptor();

			//arreglo de bytes donde se guarda la
			//cadena cifrada
			byte[] ArrayResultado =
				cTransform.TransformFinalBlock(Arreglo_a_Cifrar,
					0, Arreglo_a_Cifrar.Length);

			tdes.Clear();

			//se regresa el resultado en forma de una cadena
			return Convert.ToBase64String(ArrayResultado,
				0, ArrayResultado.Length);
		}

		public static string Desencriptar(string clave)
		{
			byte[] keyArray;
			//convierte el texto en una secuencia de bytes
			byte[] Array_a_Descifrar =
				Convert.FromBase64String(clave);

			//se llama a las clases que tienen los algoritmos
			//de encriptación se le aplica hashing
			//algoritmo MD5
			MD5CryptoServiceProvider hashmd5 =
				new MD5CryptoServiceProvider();

			keyArray = hashmd5.ComputeHash(
				Encoding.UTF8.GetBytes(key));

			hashmd5.Clear();

			TripleDESCryptoServiceProvider tdes =
				new TripleDESCryptoServiceProvider();

			tdes.Key = keyArray;
			tdes.Mode = CipherMode.ECB;
			tdes.Padding = PaddingMode.PKCS7;

			ICryptoTransform cTransform =
				tdes.CreateDecryptor();

			byte[] resultArray =
				cTransform.TransformFinalBlock(Array_a_Descifrar,
					0, Array_a_Descifrar.Length);

			tdes.Clear();
			//se regresa en forma de cadena
			return Encoding.UTF8.GetString(resultArray);
		}

		/// <summary>
		/// Fin Metodos de Encriptar y desencritar
		/// </summary>
		public static Image EscalarImagen(Image image, int maxWidth, int maxHeight)
		{
			var ratioX = (double)maxWidth / image.Width;
			var ratioY = (double)maxHeight / image.Height;
			var ratio = Math.Min(ratioX, ratioY);

			var newWidth = (int)(image.Width * ratio);
			var newHeight = (int)(image.Height * ratio);

			var newImage = new Bitmap(newWidth, newHeight);

			using (var graphics = Graphics.FromImage(newImage))
				graphics.DrawImage(image, 0, 0, newWidth, newHeight);

			return newImage;
		}


		//public static string FechaFormatoMilitar(DateTime fecha)
		//{
		//    char[] delim = { '/' };

		//    string[] arrFecha = fecha.ToShortDateString().Split(delim);

		//    return arrFecha[2] + arrFecha[1] + arrFecha[0];
		//}

		public static string FechaFormatoMilitar(String fecha)
		{
			char[] delim = { '/' };

			string[] arrFecha = fecha.Split(delim);

			return arrFecha[2] + arrFecha[1] + arrFecha[0];
		}

		
		#region  Formato decimal 13,2

		public static string formatoDecimal(decimal value)
		{
			return value.ToString("N2", new CultureInfo("en-US"));
		}

		#endregion

		/// <summary>VNieve
		/// Remueve elementos duplicados en un Lista de tipo string
		/// </summary>
		/// <param name="Lista"></param>
		/// <returns>Lista sin datos duplicados</returns>
		public static List<string> RemoverDuplicados(List<string> Lista)
		{
			Dictionary<string, int> dctDiccionario = new Dictionary<string, int>();
			List<string> finalList = new List<string>();
			foreach (string valor in Lista)
			{
				if (!dctDiccionario.ContainsKey(valor))
				{
					dctDiccionario.Add(valor, 0);
					finalList.Add(valor);
				}
			}
			return finalList;
		}

		/// <summary>
		/// [vnieve] Verifica si es Directorio o Carpeta es vacio
		/// parametro: ruta del archivo a comprobar
		/// </summary>
		/// <param name="sRuta"></param>
		/// <returns></returns>
		public static bool EsCarpetaVacia(string sRuta)
		{
			try
			{
				return Directory.GetFiles(sRuta).Length <= 0;
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>[vnieve] 
		/// Validar que los caracteres sean Números enteros:
		/// True->Valido  False->Inválido
		/// </summary>
		/// <param name="Enteros"></param>
		/// <returns>true si es válido/false si no es válido</returns>
		public static bool ValidarNumeros(string Enteros)
		{
			Regex rex = new Regex(@"^[+-]?\d+(\.\d+)?$");
			if (rex.IsMatch(Enteros))
				return true;
			return false;
		}

		/// <summary>[vnieve]
		///  Validar que los caracteres sean Números decimales
		/// </summary>
		/// <param name="NumeroDecimal"></param>
		/// <returns>true si es válido/false si no es válido</returns>
		public static bool ValidarDecimal(string NumeroDecimal)
		{
			Regex rex = new Regex(@"^(\d|-)?(\d|,)*\.?\d*$");
			if (NumeroDecimal == string.Empty)
				return false;
			if (NumeroDecimal.Equals(".") || NumeroDecimal.Equals(","))
				return false;
			if (rex.IsMatch(NumeroDecimal))
				return true;
			return false;
		}

		/// <summary>[vnieve]
		///  Valida que el formato del email sea válido
		/// </summary>
		/// <param name="Email"></param>
		/// <returns>true si es válido/false si no es válido</returns>
		public static bool ValidarEmail(string Email)
		{
			String expresion;
			expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
			if (Regex.IsMatch(Email, expresion))
			{
				if (Regex.Replace(Email, expresion, String.Empty).Length == 0)
				{
					return true;
				}
				return false;
			}

			return false;
		}

		/// <summary>VNieve
		/// Valida que el formato del teléfono sea válido
		/// </summary>
		/// <param name="Telefono"></param>
		/// <returns>true si es válido/false si no es válido</returns>
		public static bool ValidarTelefono(string Telefono)
		{
			String expresion;
			expresion = "^\\+?\\d{1,3}?[- .]?\\(?(?:\\d{2,3})\\)?[- .]?\\d\\d\\d[- .]?\\d\\d\\d\\d$";
			if (Regex.IsMatch(Telefono, expresion))
			{
				if (Regex.Replace(Telefono, expresion, String.Empty).Length == 0)
				{
					return true;
				}
				return false;
			}

			return false;
		}

		/// <summary>VNieve
		/// Valida que el formato del Url sea válido
		/// </summary>
		/// <param name="Url"></param>
		/// <returns>true si es válido/false si no es válido</returns>
		public static bool ValidarUrl(string Url)
		{
			String expresion;
			expresion =
				@"^(http|https|ftp|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
			if (Regex.IsMatch(Url, expresion))
			{
				if (Regex.Replace(Url, expresion, String.Empty).Length == 0)
				{
					return true;
				}
				return false;
			}

			return false;
		}

		/// <summary>
		/// /// [rhoyos 04/01/2017]
		/// Valida que el patron sea válido
		/// </summary>
		/// <param name="cadena">Cadena que se valida</param>
		/// <param name="patron">Patron validador</param>
		/// <param name="repetido">true si se admite repeticion del patron</param> 
		/// <returns>true si es válido/false si no es válido</returns>
		public static bool ValidarPatron(String cadena, String patron, bool repetido = false)
		{
			if (Regex.IsMatch(cadena, patron))
			{
				if (!repetido)
				{
					if (Regex.Replace(cadena, patron, String.Empty).Length == 0)
					{
						return true;
					}
					return false;
				}
				return true;
			}
			return false;
		}

		/// <summary>
		/// /// [rhoyos 04/01/2017]
		/// Valida que el formato de la placa sea válido
		/// </summary>
		/// <param name="cadena">Cadena que se valida</param>
		/// <param name="pais">Pais de procedencia de la placa</param>        
		/// <returns>true si es válido/false si no es válido</returns>
		public static bool ValidarPlaca(this String cadena, String pais = "peru")
		{
			switch (pais.ToLower())
			{
				case "peru":
					return ValidarPatron(cadena, @"\b[a-zA-Z]{3}\-?\d{3}\b");
				default:
					return false;
			}
		}

		/// <summary>VNieve
		/// Validar que la extensión del documento sea solamente
		/// pdf,doc,xls
		/// </summary>
		/// <param name="Extension"></param>
		/// <returns></returns>
		public static bool ValidarExtensionArchivo(string Extension)
		{
			String expresion;
			expresion = @"^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.pdf|.doc|xls)$";
			if (Regex.IsMatch(Extension, expresion))
			{
				if (Regex.Replace(Extension, expresion, String.Empty).Length == 0)
				{
					return true;
				}
				return false;
			}
			return false;
		}

		/// <summary> VNieve
		/// Convertir la primera letra de la oración
		/// a mayúsculas
		/// </summary>
		/// <param string="" name="Oracion"></param>
		/// <returns></returns>
		public static string CapitalizarPrimeraLetra(string Oracion)
		{
			var myTI = new CultureInfo("en-US", false).TextInfo;
			Oracion = myTI.ToLower(Oracion);

			return myTI.ToTitleCase(Oracion);
		}


		/// <summary>
		/// Retorna el Nombre del mes. Recibe el Nro de mes
		/// </summary>
		/// <param name="NroMes"></param>
		/// <returns></returns>
		public static string ObtenerMesTexto(int NroMes)
		{
			string sMes = string.Empty;

			switch (NroMes)
			{
				case 1:
					sMes = "Enero";
					break;
				case 2:
					sMes = "Febrero";
					break;
				case 3:
					sMes = "Marzo";
					break;
				case 4:
					sMes = "Abril";
					break;
				case 5:
					sMes = "Mayo";
					break;
				case 6:
					sMes = "Junio";
					break;
				case 7:
					sMes = "Julio";
					break;
				case 8:
					sMes = "Agosto";
					break;
				case 9:
					sMes = "Septiembre";
					break;
				case 10:
					sMes = "Octubre";
					break;
				case 11:
					sMes = "Noviembre";
					break;
				case 12:
					sMes = "Diciembre";
					break;
				//--------------------------///
				case 13:
					sMes = "Enero";
					break;
				case 14:
					sMes = "Febrero";
					break;
				case 15:
					sMes = "Marzo";
					break;
				case 16:
					sMes = "Abril";
					break;
				case 17:
					sMes = "Mayo";
					break;
				case 18:
					sMes = "Junio";
					break;
				case 19:
					sMes = "Julio";
					break;
				case 20:
					sMes = "Agosto";
					break;
				case 21:
					sMes = "Septiembre";
					break;
				case 22:
					sMes = "Octubre";
					break;
				case 23:
					sMes = "Noviembre";
					break;
				case 24:
					sMes = "Diciembre";
					break;
			}

			return CapitalizarPrimeraLetra(sMes);
		}

		/// <summary> [vnieve]
		/// Retornar el nombre del mes. Recibe el Nro de mes
		/// </summary>
		/// <param string="" name="sNombreMes"></param>
		/// <returns></returns>
		public static int ObtenerMesNumero(string sNombreMes)
		{
			int sMes = 0;

			switch (sNombreMes.ToUpper().Trim())
			{
				case "ENERO":
					sMes = 1;
					break;
				case "FEBRERO":
					sMes = 2;
					break;
				case "MARZO":
					sMes = 3;
					break;
				case "ABRIL":
					sMes = 4;
					break;
				case "MAYO":
					sMes = 5;
					break;
				case "JUNIO":
					sMes = 6;
					break;
				case "JULIO":
					sMes = 7;
					break;
				case "AGOSTO":
					sMes = 8;
					break;
				case "SEPTIEMBRE":
					sMes = 9;
					break;
				case "OCTUBRE":
					sMes = 10;
					break;
				case "NOVIEMBRE":
					sMes = 11;
					break;
				case "DICIEMBRE":
					sMes = 12;
					break;
			}

			return sMes;
		}

		#region Convertir

		/// <summary>VNieve
		/// Convierte un número entero o decimal
		/// a formato de letras
		/// </summary>
		/// <param name="numero">Número que se desea convertir</param>
		/// <param name="moneda">Moneda que se desea adjuntar al texto</param>
		/// <returns>Número en formato texto</returns>
		public static string ConvertirNumerosAletras(decimal numero, string moneda)
		{
			var res = new StringBuilder();
			try
			{
				string parteEntera = numero.ToString("#0.00").Split('.')[0];
				string parteDecimal = numero.ToString("#0.00").Split('.')[1];

				var numeroFormateado = new StringBuilder();
				int cuenta3 = 0;
				for (int p = parteEntera.Length - 1; p >= 0; p--)
				{
					if (cuenta3 == 3)
					{
						numeroFormateado.Insert(0, "|");
						cuenta3 = 0;
					}
					numeroFormateado.Insert(0, parteEntera.Substring(p, 1));
					cuenta3++;
				}
				string[] numeros = numeroFormateado.ToString().Split('|');

				for (int p = 0; p < numeros.Length; p++)
				{
					string n = num2texto(numeros[p]);
					res.Append(n);
					if (!n.Trim().Equals(""))
					{
						res.Append(grupos(numeros.Length - p - 1));
					}
				}

				res = res.Replace("DIECIUNO", "ONCE");
				res = res.Replace("DIECIDOS", "DOCE");
				res = res.Replace("DIECITRES", "TRECE");
				res = res.Replace("DIECICUATRO", "CATORCE");
				res = res.Replace("DIECICINCO", "QUINCE");
				res = res.Replace("  ", " ");
				res = res.Replace("  ", " ");
				if (res.ToString().StartsWith("UNO MILLONES"))
				{
					res = res.Replace("UNO MILLONES", "UN MILLON");
				}
				res = res.Replace("UNO MIL", "UN MIL");


				return res + "Y " + parteDecimal + "/100 " + moneda;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		private static string grupos(int p)
		{
			string res = string.Empty;
			switch (p)
			{
				case 1:
					res = " MIL ";
					break;
				case 2:
					res = " MILLONES ";
					break;
				case 3:
					res = " MIL ";
					break;
			}
			return res;
		}

		private static string num2texto(string numero)
		{
			string res = string.Empty;

			switch (numero.Length)
			{
				case 1:
					res += unidad2texto(int.Parse(numero.Substring(0, 1)));
					break;

				case 2:
					res += decena2texto(int.Parse(numero.Substring(0, 1)));

					if (int.Parse(numero.Substring(0, 1)) > 0
						&& int.Parse(numero.Substring(1, 1)) == 0)
					{
						res = res.Replace("DIECI", "DIEZ ");
						res = res.Replace("VEINTI", "VEINTE ");
					}


					if (int.Parse(numero.Substring(0, 1)) >= 3
						&& int.Parse(numero.Substring(1, 1)) > 0)
					{
						res += " Y ";
					}
					if (int.Parse(numero.Substring(1, 1)) > 0)
					{
						res += unidad2texto(int.Parse(numero.Substring(1, 1)));
					}
					break;

				case 3:
					res += centena2texto(int.Parse(numero.Substring(0, 1)));

					if (int.Parse(numero.Substring(0, 1)) > 0
						&& int.Parse(numero.Substring(1, 2)) == 0)
					{
						res = res.Replace("CIENTO", "CIEN ");
					}

					if (int.Parse(numero.Substring(1, 2)) > 0)
					{
						if (int.Parse(numero.Substring(1, 1)) > 0)
						{
							res += decena2texto(int.Parse(numero.Substring(1, 1)));

							if (int.Parse(numero.Substring(1, 1)) > 0
								&& int.Parse(numero.Substring(2, 1)) == 0)
							{
								res = res.Replace("DIECI", "DIEZ ");
								res = res.Replace("VEINTI", "VEINTE ");
							}

							if (int.Parse(numero.Substring(1, 1)) >= 3
								&& int.Parse(numero.Substring(1, 1)) > 0)
							{
								res += "  ";
							}
						}

						if (int.Parse(numero.Substring(2, 1)) > 0)
						{
							res += unidad2texto(int.Parse(numero.Substring(2, 1)));
						}
					}
					break;
			}
			return res;
		}

		private static string centena2texto(int numero)
		{
			string res = string.Empty;
			switch (numero)
			{
				case 1:
					res = "CIENTO ";
					break;
				case 2:
					res = "DOSCIENTOS ";
					break;
				case 3:
					res = "TRESCIENTOS ";
					break;
				case 4:
					res = "CUATROCIENTOS ";
					break;
				case 5:
					res = "QUINIENTOS ";
					break;
				case 6:
					res = "SEISCIENTOS ";
					break;
				case 7:
					res = "SETECIENTOS ";
					break;
				case 8:
					res = "OCHOCIENTOS ";
					break;
				case 9:
					res = "NOVECIENTOS ";
					break;
			}
			return res;
		}

		private static string decena2texto(int numero)
		{
			string res = string.Empty;
			switch (numero)
			{
				case 1:
					res = "DIECI";
					break;
				case 2:
					res = "VEINTI";
					break;
				case 3:
					res = "TREINTA ";
					break;
				case 4:
					res = "CUARENTA ";
					break;
				case 5:
					res = "CINCUENTA ";
					break;
				case 6:
					res = "SESENTA ";
					break;
				case 7:
					res = "SETENTA ";
					break;
				case 8:
					res = "OCHENTA ";
					break;
				case 9:
					res = "NOVENTA ";
					break;
			}
			return res;
		}

		private static string unidad2texto(int numero)
		{
			string res = string.Empty;
			switch (numero)
			{
				case 0:
					res = "CERO ";
					break;
				case 1:
					res = "UNO ";
					break;
				case 2:
					res = "DOS ";
					break;
				case 3:
					res = "TRES ";
					break;
				case 4:
					res = "CUATRO ";
					break;
				case 5:
					res = "CINCO ";
					break;
				case 6:
					res = "SEIS ";
					break;
				case 7:
					res = "SIETE ";
					break;
				case 8:
					res = "OCHO";
					break;
				case 9:
					res = "NUEVE ";
					break;
			}
			return res;
		}

		#endregion

		#region Rellenar Ceros a Correlativo Factura-Boleta

		/// <summary>
		/// Rellena de Ceros a la izquiera para el correlativo
		/// de facturas o boletas
		/// </summary>
		/// <param name="nroCadena"></param>
		/// <returns></returns>
		public static string nroCorrelativo(string nroCadena)
		{
			int caracteres = 0;
			caracteres = nroCadena.Length;

			switch (caracteres)
			{
				case 1:
					nroCadena = "000000" + nroCadena;
					break;
				case 2:
					nroCadena = "00000" + nroCadena;
					break;
				case 3:
					nroCadena = "0000" + nroCadena;
					break;
				case 4:
					nroCadena = "000" + nroCadena;
					break;
				case 5:
					nroCadena = "00" + nroCadena;
					break;
				case 6:
					nroCadena = "0" + nroCadena;
					break;
			}

			return nroCadena;
		}

		/// <summary>
		/// Rellena de Ceros a la izquierda 
		/// para el número de serie de la Factura o Boleta
		/// </summary>
		/// <param name="nroCadena"></param>
		/// <returns></returns>
		public static string nroSerie(string nroCadena) //para la serie
		{
			int caracteres = 0;
			caracteres = nroCadena.Length;

			switch (caracteres)
			{
				case 1:
					nroCadena = "00" + nroCadena;
					break;
				case 2:
					nroCadena = "0" + nroCadena;
					break;
			}

			return nroCadena;
		}

		/// <summary>
		/// Rellena de Ceros a la izquiera para el correlativo
		/// </summary>
		/// <param name="nroDigitos"></param>
		/// <param name="nroCorrelativo"></param>/// 
		/// <returns></returns>
		public static string nroCorrelativo(int nroDigitos, long nroCorrelativo)
		{
			string nroCadena = string.Empty;
			nroCadena = new string('0', nroDigitos);
			nroCadena += (nroCorrelativo).ToString();
			nroCadena = nroCadena.Substring(nroCadena.Length - nroDigitos);
			return nroCadena;
		}

		#endregion

		/// <summary>
		/// Conversion de Variables
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dbValue"></param>
		/// <returns></returns>
		public static T Valor<T>(object dbValue) where T : struct
		{
			T Item;
			if (dbValue == null)
			{
				return default(T);
			}
			if (dbValue.Equals(DBNull.Value))
			{
				return default(T);
			}
			if (string.IsNullOrEmpty(dbValue.ToString()))
			{
				Item = default(T);
			}
			try
			{
				var conv = TypeDescriptor.GetConverter(typeof(T));
				Item = (T)conv.ConvertFrom(dbValue.ToString());
			}
			catch
			{
				Item = default(T);
			}
			return Item;
		}

		public static T? DbValueToNullable<T>(Object dbValue) where T : struct
		{
			T? returnValue = null;
			if (string.IsNullOrWhiteSpace(dbValue.Recortar()))
			{
				return null;
			}
			if (dbValue != null && !dbValue.Equals(DBNull.Value))
			{
				var conv = TypeDescriptor.GetConverter(typeof(T));
				returnValue = (T)conv.ConvertFrom(dbValue.ToString());
			}
			return returnValue;
		}

		/// <summary>
		/// Quitar Espacios
		/// </summary>
		/// <param name="dbValue"></param>
		/// <returns></returns>
		public static string Recortar(this object dbValue)
		{
			string valor = Convert.ToString(dbValue).Trim();
			return valor;
		}

		/// <summary>
		///  Convierte un Objeto en Entero 32bits
		/// </summary>
		/// <param name="dbValue"></param>
		/// <returns></returns>
		public static int toInt(this object dbValue)
		{
			try
			{
				if (dbValue == null)
					return 0;
				if (dbValue.ToString().Trim().Equals("")) return 0;
				int valor = Convert.ToInt32(dbValue);
				return valor;
			}
			catch
			{
				return 0;
			}
		}


		/// <summary>
		/// [vnieve] Convierte un Objeto a DateTime respetando
		/// la configuración regional del dispositivo
		/// </summary>
		/// <param name="Valor"></param>
		/// <returns></returns>
		public static DateTime ToDateTime(this object Valor)
		{
			try
			{
				var FechaValor = Convert.ToDateTime(Valor);
				return Convert.ToDateTime(FechaValor.ToString("s"));
			}
			catch
			{
				return new DateTime(1901, 9, 9, 9, 9, 9);
			}
		}


		/// <summary>
		/// [vnieve] Muestra el DateTimePicker en formato
		/// 13/Ago/2018
		/// </summary>
		/// <param name="dtpFecha"></param>
		


		/// <summary>
		///  Convierte un Objeto a Decimal con formato 0.00
		/// </summary>
		/// <param name="dbValue"></param>
		/// <param name="bmoneda"></param>
		/// <returns></returns>
		public static decimal toDecimal(this object dbValue, bool bmoneda = false)
		{
			try
			{
				if (dbValue == null)
					return 0;
				if (dbValue.ToString().Trim().Equals("")) return 0;
				decimal valor = Convert.ToDecimal(dbValue);

				if (bmoneda)
					return Convert.ToDecimal(formatoDecimal(valor));

				return valor;
			}
			catch
			{
				return 0;
			}
		}
		public static bool toBoolean(this object dbValue, bool bmoneda = false)
		{
			try
			{
				if (dbValue == null)
					return false;
				if (dbValue is Boolean)
					return (bool)dbValue;
				else
					return false;
			}
			catch
			{
				return false;
			}
		}


		

		/// <summary>
		/// Convierte un Objeto a string
		/// </summary>
		/// <param name="dbValue"></param>
		/// <returns></returns>
		public static string texto(this object dbValue)
		{
			return dbValue == null ? "" : dbValue.ToString();
		}

		public static void BorraFichero(string Ruta)
		{
			if (!string.IsNullOrWhiteSpace(Ruta))
			{
				if (File.Exists(Ruta))
				{
					File.Delete(Ruta);
				}
			}
		}

		/// <summary>
		/// [ccerna 28/06/2016]
		/// Método que calcula la semana del año en la que se encuentra una fecha determinada
		/// </summary>
		/// <param name="dFecha">Fecha que se desea tratar</param>
		/// <returns></returns>
		public static int CalcularSemanaxFecha(DateTime dFecha)
		{
			return
				(CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(dFecha, CalendarWeekRule.FirstDay, dFecha.DayOfWeek) -
				 1);
		}

		
		/// <summary>
		///  Obtiene El obj[0]=IGV, obj[1]=Base y obj[2]Total 
		/// </summary>
		/// <param name="nIncluye"></param>
		/// <param name="nExcepcionIGV"></param>
		/// <param name="nPrecio"></param>
		/// <param name="nCantidad"></param>
		/// <param name="nIGV"></param>
		/// <returns></returns>
		public static object calcularBase(int nIncluye, int nExcepcionIGV, decimal nPrecio, decimal nCantidad,
			decimal nIGV)
		{
			//nIGV = Math.Round(ImpuestoNeg.Instance.ObtenerValorImpuesto(IGV_Id) / 100, 2);
			decimal nValorIGV = 0;
			decimal nValorBase = 0;
			decimal nValorTotal = 0;

			if (nIncluye == 1 && nExcepcionIGV == 1)
			{
				nValorIGV = decimal.Parse((((nPrecio * nCantidad) * (nIGV * 100)) / (100 + (nIGV * 100))).ToString("N2"));
				nValorBase = decimal.Parse(((nPrecio * nCantidad) - (nValorIGV)).ToString("N2"));
				nValorTotal = decimal.Parse((nPrecio * nCantidad).ToString("N2"));
			}
			if (nIncluye == 1 && nExcepcionIGV == 1) //Incluye IGV: SI
			{
				nValorIGV = decimal.Parse((((nPrecio * nCantidad) * (nIGV * 100)) / (100 + (nIGV * 100))).ToString("N2"));
				nValorBase = decimal.Parse(((nPrecio * nCantidad) - (nValorIGV)).ToString("N2"));
				nValorTotal = decimal.Parse((nPrecio * nCantidad).ToString("N2"));
			}
			else if (nIncluye == 2 && nExcepcionIGV == 1) //Incluye IGV: NO
			{
				nValorIGV = decimal.Parse((((nPrecio * nCantidad) * nIGV)).ToString("N2"));
				nValorBase = decimal.Parse((nPrecio * nCantidad).ToString("N2"));
				nValorTotal = decimal.Parse(((nPrecio * nCantidad) + (nValorIGV)).ToString("N2"));
			}
			else
			{
				nValorIGV = decimal.Parse("0.00");
				nValorBase = decimal.Parse(((nPrecio * nCantidad) - (nValorIGV)).ToString("N2"));
				nValorTotal = decimal.Parse((nPrecio * nCantidad).ToString("N2"));
			}

			return new object[] { nValorIGV, nValorBase, nValorTotal };
		}


		#region Mensajes Para Log

		/// <summary>
		/// 
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="sDatabase"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		

		#endregion


		/// <summary>
		/// [vnieve] Devuelve una cadena con la representación de los tipos
		/// de extensiones de los archivos para filtrar al escoger
		/// los adjuntos de archivos
		/// </summary>
		/// <returns></returns>
		public static string FiltroArchivos()
		{
			return @"Archivos (*.pdf;*.xlsx;*.xls;*.png;*.jpg;*.bmp)|*.pdf;*.xlsx;*.xls;*.png;*.jpg;*.bmp";
		}
	}

	#region Enum para StatusBar

	public enum StatusBarEnum
	{
		INFO = 1,
		CONFIRM = 2,
		WARNING = 3,
		ERROR = 4
	}

	#endregion

	public class StringWriterUtf8 : StringWriter
	{
		public override Encoding Encoding
		{
			get { return Encoding.UTF8; }
		}
	}

	public class StringWriterIso8859 : StringWriter
	{
		public override Encoding Encoding
		{
			get { return Encoding.GetEncoding("ISO-8859-1"); }
		}
	}


}
