namespace CleaseSolution
{
    using System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public static class JsonExtensions
    {
        public static string ToJson(this object value)
        {
            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.Converters.Add(new StringEnumConverter());
            return JsonConvert.SerializeObject(value, jsonSettings);
        }

        public static T FromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static T FromJson<T>(this string json, T defaultValue)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return defaultValue;
            }
        }
    }
}