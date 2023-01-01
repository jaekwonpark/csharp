using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;
using System.Runtime.Serialization;

namespace CustomEnumName;

public class EnumConverter<T> : JsonConverter<T>
    where T : struct, Enum
{
    private Dictionary<string, Enum>? stringToEnum;
    private Dictionary<Enum, string>? enumToString;
    public override bool CanConvert(Type type)
    {
        return type.IsEnum;
    }

    private void InitDictionaryIfNot(Type typeToConvert)
    {
        if (stringToEnum == null || enumToString == null)
        {
            var queryNameAndValue = from field in typeToConvert.GetFields(BindingFlags.Public | BindingFlags.Static)
                                    let attr = field.GetCustomAttribute<EnumMemberAttribute>()
                                    where attr != null
                                    select (attr.Value, field.GetValue(field.Name));
            stringToEnum = queryNameAndValue.ToDictionary(p => p.Item1, p => (Enum)p.Item2);
            var queryValueAndName = from field in typeToConvert.GetFields(BindingFlags.Public | BindingFlags.Static)
                                    let attr = field.GetCustomAttribute<EnumMemberAttribute>()
                                    where attr != null
                                    select (field.GetValue(field.Name), attr.Value);
            enumToString = queryValueAndName.ToDictionary(p => (Enum)p.Item1, p => p.Item2);
        }

    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        InitDictionaryIfNot(typeToConvert);

        string? customName = reader.GetString();
        if (stringToEnum!.Count > 0 && customName != null)
        {
            var ret = stringToEnum[customName];
            return (T)ret;
        }

        // Handle deserializing Enum with no custom name
        JsonTokenType token = reader.TokenType;

        if (token == JsonTokenType.String)
        {
            // Try parsing case sensitive first
            if (Enum.TryParse(customName, out T value)
                || Enum.TryParse(customName, ignoreCase: true, out value))
            {
                return value;
            }
        }

        throw new JsonException($"Unknown Enum {customName}");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        InitDictionaryIfNot(value.GetType());
        if (enumToString!.ContainsKey(value)) {
            writer.WriteStringValue(enumToString![value]);
        } else {
            // Handle serializing Enum with no custom name
            writer.WriteStringValue(value.ToString());
        }
    }
}
