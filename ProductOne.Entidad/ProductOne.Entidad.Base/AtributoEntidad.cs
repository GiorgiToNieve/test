﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductOne.Entidad.ProductOne.Entidad.Base
{
	public class AtributoEntidad
	{
		public string sNombre { get; set; }
		public object oValor { get; set; }
		public TipoColumna oTipo { get; set; }

		public AtributoEntidad()
		{
			sNombre = string.Empty;
			oValor = null;
			oTipo = TipoColumna.Field;
		}
	}

	public enum TipoColumna : byte { PrimaryKey, ForeignKey, Field };
}

