using Module.Util.Extensao;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Module.Util.Json
{
    /// <summary>
    /// Classe que implementa um decorator para bytearray registrado na construção do api core
    /// </summary>
    public class JsonConverterByteArrayGlobal : JsonConverter<byte[]>
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(byte[]);
        }

        public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return TryConvertBase64(reader);
        }

        public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
        {
            if (value == null)
                return;

            TryWriteJson(writer, value);
        }

        /// <summary>
        /// Método que tenta escrever para base 64
        /// </summary>
        /// <param name="reader">Reader que o .net core envia para conversão</param>
        /// <returns>bytearray</returns>
        private static byte[] TryConvertBase64(Utf8JsonReader reader)
        {
            try
            {
                var valorStr = ProcessBase64(reader.GetString());
                var byteArray = Convert.FromBase64String(valorStr);

                return byteArray;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Método que tenta escrever para json
        /// </summary>
        /// <param name="writer">Writer que o .net core envia para conversão</param>
        /// <param name="value">void</param>
        private static void TryWriteJson(Utf8JsonWriter writer, object value)
        {
            try
            {
                byte[] bytes = (byte[])value;

                writer.WriteStringValue(bytes.ToHtmlBase64());
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Método para tratar texto base 64 para conversão para bytearray
        /// </summary>
        /// <param name="valor">Objeto base 64</param>
        /// <returns>Texto sem cabeçalho base6</returns>
        private static string ProcessBase64(string valor)
        {
            var regexDataBase64 = new Regex("data:(.)*;base64,", RegexOptions.IgnoreCase);
            var valueStr = regexDataBase64.Replace(valor, String.Empty);

            return valueStr;
        }
    }
}
