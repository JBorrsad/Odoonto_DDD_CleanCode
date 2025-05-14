using System;
using System.Collections.Generic;
using Google.Cloud.Firestore;

namespace Odoonto.Data.Mappings
{
    /// <summary>
    /// Clase base con métodos de utilidad para mapeo entre documentos Firestore y entidades
    /// </summary>
    public static class BaseFirestoreMapper
    {
        /// <summary>
        /// Extrae un valor Guid desde un diccionario
        /// </summary>
        public static Guid GetGuidValue(Dictionary<string, object> data, string key)
        {
            if (data == null || !data.TryGetValue(key, out var value) || value == null)
                return Guid.Empty;

            var guidStr = value.ToString();
            return !string.IsNullOrEmpty(guidStr) && Guid.TryParse(guidStr, out var guid)
                ? guid
                : Guid.Empty;
        }

        /// <summary>
        /// Extrae un valor string desde un diccionario
        /// </summary>
        public static string GetStringValue(Dictionary<string, object> data, string key, string defaultValue = "")
        {
            return data?.GetValueOrDefault(key)?.ToString() ?? defaultValue;
        }

        /// <summary>
        /// Extrae un valor DateTime desde un diccionario
        /// </summary>
        public static DateTime GetDateTimeValue(Dictionary<string, object> data, string key, DateTime defaultValue)
        {
            if (data == null || !data.TryGetValue(key, out var value) || value == null)
                return defaultValue;

            if (value is Timestamp timestamp)
                return timestamp.ToDateTime();

            return defaultValue;
        }

        /// <summary>
        /// Extrae un valor bool desde un diccionario
        /// </summary>
        public static bool GetBoolValue(Dictionary<string, object> data, string key, bool defaultValue = false)
        {
            if (data == null || !data.TryGetValue(key, out var value) || value == null)
                return defaultValue;

            if (value is bool boolValue)
                return boolValue;

            return bool.TryParse(value.ToString(), out var result) ? result : defaultValue;
        }

        /// <summary>
        /// Extrae un valor decimal desde un diccionario
        /// </summary>
        public static decimal GetDecimalValue(Dictionary<string, object> data, string key, decimal defaultValue = 0)
        {
            if (data == null || !data.TryGetValue(key, out var value) || value == null)
                return defaultValue;

            if (value is double doubleValue)
                return (decimal)doubleValue;

            if (value is long longValue)
                return (decimal)longValue;

            if (value is int intValue)
                return (decimal)intValue;

            return decimal.TryParse(value.ToString(), out var result) ? result : defaultValue;
        }

        /// <summary>
        /// Extrae un valor int desde un diccionario
        /// </summary>
        public static int GetIntValue(Dictionary<string, object> data, string key, int defaultValue = 0)
        {
            if (data == null || !data.TryGetValue(key, out var value) || value == null)
                return defaultValue;

            if (value is int intValue)
                return intValue;

            if (value is long longValue)
                return (int)longValue;

            if (value is double doubleValue)
                return (int)doubleValue;

            return int.TryParse(value.ToString(), out var result) ? result : defaultValue;
        }

        /// <summary>
        /// Extrae un valor TimeSpan desde un diccionario (almacenado como string)
        /// </summary>
        public static TimeSpan GetTimeSpanValue(Dictionary<string, object> data, string key, TimeSpan defaultValue)
        {
            if (data == null || !data.TryGetValue(key, out var value) || value == null)
                return defaultValue;

            var timeSpanStr = value.ToString();
            return TimeSpan.TryParse(timeSpanStr, out var result) ? result : defaultValue;
        }

        /// <summary>
        /// Convierte una fecha a Timestamp para almacenar en Firestore
        /// </summary>
        public static Timestamp ToFirestoreTimestamp(DateTime dateTime)
        {
            return Timestamp.FromDateTime(dateTime.ToUniversalTime());
        }

        /// <summary>
        /// Obtiene una lista de objetos desde un diccionario
        /// </summary>
        public static List<T> GetList<T>(Dictionary<string, object> data, string key, Func<object, T> converter)
        {
            var result = new List<T>();

            if (data == null || !data.TryGetValue(key, out var value) || value == null)
                return result;

            if (value is List<object> list)
            {
                foreach (var item in list)
                {
                    if (item != null)
                    {
                        result.Add(converter(item));
                    }
                }
            }

            return result;
        }
    }
}