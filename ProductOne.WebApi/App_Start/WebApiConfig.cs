using System;
using Microsoft.AspNet.WebApi.Extensions.Compression.Server;
using System.Net.Http.Extensions.Compression.Core.Compressors;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ProductOne.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuración y servicios de API web

            // Rutas de API web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "servicio/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

			config.Formatters.Remove(config.Formatters.XmlFormatter);

			config.MessageHandlers.Insert(0,
				new ServerCompressionHandler(new GZipCompressor(), new DeflateCompressor()));


		}
    }
}
