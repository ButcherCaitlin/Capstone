using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Capstone.API.Converters
{
    public class TimeSpanToStringConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            TimeSpan toReturn = TimeSpan.Zero;
            try
            {
                toReturn = TimeSpan.Parse(value);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            }
            return toReturn;
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
