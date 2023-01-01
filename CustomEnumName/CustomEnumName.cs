using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;
using System.Runtime.Serialization;

namespace CustomEnumName;

public class EnumConverter<T> : JsonConverter<T>
    where T : struct, Enum
{
    private Dictionary<string, object>? enumMembers;
    public override bool CanConvert(Type type)
    {
        return type.IsEnum;
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (enumMembers == null)
        {
            var query = from field in typeToConvert.GetFields(BindingFlags.Public | BindingFlags.Static)
                        let attr = field.GetCustomAttribute<EnumMemberAttribute>()
                        where attr != null
                        select (attr.Value, field.GetValue(field.Name));
            enumMembers = query.ToDictionary(p => p.Item1, p => p.Item2);
        }

        string? customName = reader.GetString();
        if (enumMembers.Count > 0 && customName != null)
        {
            var ret = enumMembers[customName];
            return (T)ret;
        }

        throw new NotImplementedException("Not implemented.");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        throw new NotImplementedException("Not implemented.");
    }
}
