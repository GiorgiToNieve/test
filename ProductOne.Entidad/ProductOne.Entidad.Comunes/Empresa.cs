﻿using ProductOne.Entidad.ProductOne.Entidad.Base;

namespace ProductOne.Entidad.ProductOne.Entidad.Comunes
{
	public class Empresa : BaseEntidad
	{
		#region "Columnas"

		[PrimaryKey]
		public int Empresa_Id { set; get; }

		[Field]
		public string sEmpRuc { set; get; }

		[Field]
		public string sEmpNombre { set; get; }

		[Field]
		public string sEmpBreve { set; get; }

		[Field]
		public string sEmpEmail { set; get; }

		[Field]
		public string sEmpTelefono { set; get; }

		[ForeignKey]
		public int Ubigeo_Id { set; get; }

		/// <summary>
		/// Direccion fiscal de la empresa emisora
		/// </summary>
		[Field]
		public string sEmpDireccion { set; get; }


		#region Campos de Login para la Facturacion Electronica

		[Field]
		public string sEmpUsuarioFE { set; get; }

		/// <summary>
		/// 
		/// </summary>
		[Field]
		public string sEmpPasswordFE { get; set; }

		/// <summary>
		/// Clave del certificado digital de la empresa
		/// </summary>
		[Field]
		public string sEmpClaveCertificadoFE { get; set; }

		/// <summary>
		/// ruta raiz donde se guardan las demas carpetas del proceso de FE
		/// </summary>
		[Field]
		public string sEmpRuta { get; set; }

		/// <summary>
		/// nombre del certificado digital de la empresa
		/// para que sea localizado por el mismo
		/// </summary>
		[Field]
		public string sEmpNombreCertificado { get; set; }


		/// <summary>
		/// 0:Pruebas
		/// 1:Produccion
		/// </summary>
		[Field]
		public int nEmpProduccion { get; set; }

		#endregion

		#endregion

		#region "Otros"

		/// <summary>
		/// codigo del ubigeo de la empresa a 4 jerarquias
		/// esto esta seteado desde la base de datos
		/// y esta en duracell va en la fact_elect
		/// </summary>
		[Field]
		public string sEmpCodigoUbigeoSunat { set; get; }

		/// <summary>
		/// concatenacion de los ubigeos en jerarquia
		/// de los ubigeos pero de la direccion de operaciones
		/// de la empresa, esta seteadoen el store en duracell
		/// va en la ciudad de la facturacion electronica
		/// </summary>
		[Field]
		public string sEmpCiudad { set; get; }

		/// <summary>
		/// campo abreviado del pais en este caso PE de Perú
		/// </summary>
		[Field]
		public string sEmpAbreviaturaPais { set; get; }

		/// <summary>
		/// codigo de identificacion de la empresa emisora
		/// por default es 06 y viene tbn desde la base de datos
		/// </summary>
		[Field]
		public string sEmpTipoIdentificadorEmpresaSunat { set; get; }

		/// <summary>
		/// codigo de identificacion del cliente por default es 06
		/// y viene desde la base de datos
		/// </summary>
		[Field]
		public string sEmpTipoIdentificadorClienteSunat { set; get; }

		/// <summary>
		/// cuenta de banco de la nacion de la empresa para las detracciones
		/// </summary>
		[Field]
		public string sEmpCtaBancoDetracciones { set; get; }

		[Field]
		public string sEmpCodigoDetraccion { set; get; }

		/// <summary>
		/// 0: SIN RELACION; 1: LA EMPRESA ES INAFECTO AL IGV
		/// </summary>
		[Field]
		public int nEmpExonerado { set; get; }

		[Field]
		public int nEmpEstado { set; get; }

		[Field]
		public int nEmpAfilidadoOSE { set; get; }

		#endregion

		#region "Constructor"

		public Empresa()
		{
			sEmpRuc = string.Empty;
			sEmpNombre = string.Empty;
			sEmpBreve = string.Empty;
			sEmpEmail = string.Empty;
			sEmpTelefono = string.Empty;
			sEmpDireccion = string.Empty;
			sEmpTipoIdentificadorClienteSunat = string.Empty;
			sEmpTipoIdentificadorEmpresaSunat = string.Empty;
			sEmpCiudad = string.Empty;
			sEmpCodigoUbigeoSunat = string.Empty;
			sEmpUsuarioFE = string.Empty;
			sEmpPasswordFE = string.Empty;
			sEmpClaveCertificadoFE = string.Empty;
			sEmpCodigoDetraccion = string.Empty;
			nEmpProduccion = 0;
			nEmpAfilidadoOSE = 0;
		}

		#endregion

	}
}
