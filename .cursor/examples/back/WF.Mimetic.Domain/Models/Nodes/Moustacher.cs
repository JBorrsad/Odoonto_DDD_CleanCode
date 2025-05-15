using HandlebarsDotNet;
using HandlebarsDotNet.Extension.NewtonsoftJson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WF.Mimetic.Domain.Core.Models.Exceptions;

namespace WF.Mimetic.Domain.Models.Nodes
{
    public static class Moustacher
    {
        //private static readonly IHandlebars _jsonHandlebars = Handlebars.Create(
        //    new HandlebarsConfiguration().UseNewtonsoftJson()
        //);

        private static readonly IHandlebars _jsonHandlebars = Handlebars.Create(
            new HandlebarsConfiguration()
            {
                FormatProvider = System.Globalization.CultureInfo.InvariantCulture,
                TextEncoder = new DummyEncoder()
            }
            .UseNewtonsoftJson()
        );

        private class DummyEncoder : ITextEncoder
        {
            // Método principal que simplemente devuelve el valor sin modificar
            public string Encode(string value) => value;

            // Escribe el texto directamente en el TextWriter
            public void Encode(StringBuilder text, TextWriter target)
            {
                if (text == null || target == null) return;
                target.Write(text.ToString());
            }

            // Escribe el string directamente en el TextWriter
            public void Encode(string text, TextWriter target)
            {
                if (text == null || target == null) return;
                target.Write(text);
            }

            // Escribe los caracteres del IEnumerator en el TextWriter
            public void Encode<T>(T text, TextWriter target) where T : IEnumerator<char>
            {
                if (text == null || target == null) return;

                while (text.MoveNext())
                {
                    target.Write(text.Current);
                }
            }
        }

        public static string GetValueFromJson(string json, string property)
        {
            if (json is null)
            {
                throw new WrongOperationException("The msg cant be null.");
            }

            if (string.IsNullOrWhiteSpace(property))
            {
                throw new WrongOperationException("The property cant be null/empty.");
            }

            HandlebarsTemplate<object, object> compiled = _jsonHandlebars.Compile("{{{" + property + "}}}");
            return compiled(JsonConvert.DeserializeObject(json));
        }

        public static JObject GetJObjectFromJson(string json) {
            if (json is null)
            {
                throw new WrongOperationException("The msg cant be null.");
            }
            return JObject.Parse(json);
        }

        public static string PrintFromJson(string json, string template)
        {
            if (json is null)
            {
                throw new WrongOperationException("The msg cant be null.");
            }

            if (string.IsNullOrWhiteSpace(template))
            {
                throw new WrongOperationException("The template cant be null/empty.");
            }

            HandlebarsTemplate<object, object> compiled = _jsonHandlebars.Compile(template);
            return compiled(JsonConvert.DeserializeObject(json));
        }

        public static string SetStringToJson(string value, string propertyName, string json)
        {
            JObject jsonObject = JObject.Parse(json);
            jsonObject[propertyName] = value;

            return JsonConvert.SerializeObject(jsonObject);
        }

        public static string SetObjectToJson(object value, string propertyName, string json)
        {
            JObject jsonObject = JObject.Parse(json);
            jsonObject[propertyName] = JObject.FromObject(value);

            return JsonConvert.SerializeObject(jsonObject);
        }

        public static string SetJsonToJson(string value, string propertyName, string json)
        {
            JObject jsonObject = JObject.Parse(json);
            jsonObject[propertyName] = JObject.Parse(value);

            return JsonConvert.SerializeObject(jsonObject);
        }

        public static string WrapJson(string propertyName, string json)
        {
            return string.IsNullOrEmpty(json) ? "{ \"" + propertyName + "\": null }" : "{ \"" + propertyName + "\": " + json + " }";
        }

        public static bool IsJson(string value)
        {
            try
            {
                JObject.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
