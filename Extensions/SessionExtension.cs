using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System;

namespace Ferreteria.Extensions
{
    public static class SessionExtension
    {
        // Método de extensión para almacenar un objeto en la sesión
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            // Serializar el objeto usando JSON y almacenarlo en la sesión como una cadena
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        // Método de extensión para recuperar un objeto de la sesión
        public static T GetObject<T>(this ISession session, string key)
        {
            // Obtener la cadena almacenada en la sesión con la clave proporcionada
            var value = session.GetString(key);

            // Si no hay valor, devolver el valor predeterminado para el tipo
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

    }
}
