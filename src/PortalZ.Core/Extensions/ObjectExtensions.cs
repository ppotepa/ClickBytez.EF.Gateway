using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PortalZ.Extensions
{
    internal static class ObjectExtensions
    {
        public static string ToJson(this object @object)
            => JsonConvert.SerializeObject(@object);

        public static JToken ToJToken(this object @object)
           => JToken.FromObject(@object);

    }
}



