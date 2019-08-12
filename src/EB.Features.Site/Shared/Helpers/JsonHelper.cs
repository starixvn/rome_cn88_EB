using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace EB.Features.Site.Shared.Helpers
{
    public static class JsonHelper
    {
        [ThreadStatic]
        private static Func<object, string> toJsonFunc;

        public static Func<object, string> ToJsonFunc
        {
            get
            {
                return toJsonFunc ?? ToJson;
            }

            set
            {
                toJsonFunc = value;
            }
        }

        internal static string ToJson(object data) => JsonConvert.SerializeObject(data).Replace("\"", "'");
    }
}