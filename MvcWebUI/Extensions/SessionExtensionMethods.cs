using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace MvcWebUI.Extensions
{
    public static class SessionExtensionMethods
    {
        public static void SetObject(this ISession session, string key, Object value)
        {
            var objectString = JsonConvert.SerializeObject(value);
            session.SetString(key, objectString);
        }

        public static T GetObject<T>(this ISession session, string key) where T : class
        {
            string stringObject = session.GetString(key);
            if (string.IsNullOrEmpty(stringObject))
            {
                return null;
            }
            else
            {
                T value = JsonConvert.DeserializeObject<T>(stringObject);
                return value;
            }
        }
    }
}
