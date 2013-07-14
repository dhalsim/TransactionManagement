using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TransactionManagement.Library
{
    public class TransactionHelper
    {
        private static readonly Lazy<JsonSerializerSettings> s_Settings =
            new Lazy<JsonSerializerSettings>(() =>
                            {
                                var settings = new JsonSerializerSettings {Formatting = Formatting.Indented};
                                settings.Converters.Add(new StringEnumConverter());
                                return settings;
                            });

        public static string ToJson(object @object)
        {
            return JsonConvert.SerializeObject(@object, s_Settings.Value);
        }
    }
}