using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TGSP.Shared.Json
{
    public static class JsonOptions
    {
        /// <summary>
        /// This method will set the default options the given options
        /// </summary>
        /// <param name="options"></param>
        public static void SetDefaultOptions(JsonSerializerOptions options)
        {
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.IgnoreReadOnlyProperties = false;
            options.IgnoreNullValues = false;
            options.PropertyNameCaseInsensitive = true;
           /* options.Converters.Add(new DateTimeConverter());
            options.Converters.Add(new NullableDateTimeConverter());
            options.Converters.Add(new GuidConvertor());
            options.Converters.Add(new NullableGuidConvertor());*/
        }

        /// <summary>
        /// This method will return the default serialization options
        /// </summary>
        /// <returns></returns>
        public static JsonSerializerOptions GetDefaultOptions()
        {
            var options = new JsonSerializerOptions();
            SetDefaultOptions(options);
            return options;
        }
    }

    /// <summary>
    /// We use a custom date time converter to deal with the correct iso standard
    /// </summary>
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateTime = DateTime.Parse(reader.GetString(),CultureInfo.InvariantCulture,DateTimeStyles.AssumeUniversal).ToUniversalTime();
            return dateTime;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
           writer.WriteStringValue(value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
        }
    }

    /// <summary>
    /// We use a custom date time converter to deal with the correct iso standard
    /// </summary>
    public class NullableDateTimeConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if(string.IsNullOrEmpty(value)) return null;
            var converter = new DateTimeConverter();
            return converter.Read(ref reader,typeToConvert,options);
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if(value.HasValue == false) writer.WriteNullValue();
            else
            {
                var converter = new DateTimeConverter();
                converter.Write(writer,value.Value,options);
            }
        }
    }

    /// <summary>
    /// The problems is that in some cases the guid are empty, that is problem for the client this is a bit annoying, so we intercept
    /// </summary>
    public class NullableGuidConvertor : JsonConverter<Guid?>
    {
        public override Guid? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var parsed = Guid.TryParse(reader.GetString(), out Guid result);
            if(parsed) return result;
            return null;
        }

        public override void Write(Utf8JsonWriter writer, Guid? value, JsonSerializerOptions options)
        {
            if(value.HasValue == false || value.Value == Guid.Empty) writer.WriteNullValue();
            else writer.WriteStringValue(value.Value.ToString());
        }
    }

    /// <summary>
    /// The problems is that in some cases the guid are empty, that is problem for the client this is a bit annoying, so we intercept
    /// </summary>
    public class GuidConvertor : JsonConverter<Guid>
    {
        public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => Guid.Parse(reader.GetString());

        public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
        {
            if(value == Guid.Empty) writer.WriteNullValue();
            else writer.WriteStringValue(value.ToString());
        }
    }
}