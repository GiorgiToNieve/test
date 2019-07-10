using ProductOne.Entidad.ProductOne.Entidad.Base;
using ProductOne.Utilitarios;
using System;
using System.Xml.Serialization;

namespace ProductOne.Entidad.ProductOne.Entidad.Logistica
{
	public class Producto : BaseEntidad
	{
		#region "Columnas"

		[XmlAttribute]
		[PrimaryKey]
		public int Producto_Id { set; get; }

		[XmlAttribute]
		[ForeignKey]
		public int Empresa_Id { set; get; }

		[XmlAttribute]
		[ForeignKey]
		public int UnidadMedida_Id { set; get; }

		[XmlAttribute]
		[Field]
		public int nProTipo { set; get; }

		[XmlAttribute]
		[Field]
		public string sProNombre { set; get; }

		[XmlAttribute]
		[Field]
		public string sProDescripcion { set; get; }

		[XmlAttribute]
		[Field]
		public int nProEstado { set; get; }

		[XmlAttribute]
		[Field]
		public DateTime FechaCreacion { get; set; }


		[XmlAttribute]
		[Field]
		public int Usuario_Id { set; get; }

		[XmlAttribute]
		[Field]
		public DateTime FechaModificacion { get; set; }

		[XmlAttribute]
		[Field]
		public int UltimoUsuario_Id { set; get; }

		#endregion

		public string sProEstado
		{
			get
			{
				return nProEstado == 1 ? "Activo" : "Inactivo";
			}
		}



		#region Otros

		[XmlAttribute]
		public string sUMeDescripcion { get; set; }

		[XmlAttribute]
		public string sUMeSimbolo { get; set; }

		[XmlAttribute]
		public string sProTipo { get; set; }

		[XmlAttribute]
		public string sProductoFiltro { get; set; }

		#endregion

		#region "Constructor"

		public Producto()
		{
			Producto_Id = 0;
			nProTipo = 0;
			Empresa_Id = 0;
			nProEstado = 0;
			sProNombre = string.Empty;
			sProDescripcion = string.Empty;
			sProTipo = string.Empty;
			sUMeSimbolo = string.Empty;
			sUMeDescripcion = string.Empty;
			sProductoFiltro = string.Empty;
			Empresa_Id = 0;
			UnidadMedida_Id = 0;
			Usuario_Id = 0;
			UltimoUsuario_Id = 0;
		}

		#endregion

		#region Serializar XML
		public override string SerializarXML()
		{
			return Util.SerializeXML<Producto>(this);
		}
		#endregion


	}
}