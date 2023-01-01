using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using CustomEnumName;
namespace CustomEnumName.Tests;

public class SerializeDeserialize
{
    [JsonConverter(typeof(EnumConverter<EnumWithCustomName>))]
    public enum EnumWithCustomName
    {
        [EnumMember(Value = "$UNKNOWN")]
        Unknown = 0 // JSON is "$UNKNOWN"
    }

    [JsonConverter(typeof(EnumConverter<EnumWithoutCustomName>))]
    public enum EnumWithoutCustomName
    {
        Unknown = 0 // JSON is "Unknown"
    }

    public class ClassWithEnumProperty
    {
        public EnumWithCustomName enumProp { set; get; }
        public EnumWithoutCustomName enumPropNoCustomName { set; get; }
    }

    [Fact]
    public void DeserializeEnumWithCustomName()
    {
        string deserialized = "\"$UNKNOWN\"";
        var value = JsonSerializer.Deserialize<EnumWithCustomName>(deserialized);

        Assert.True(value == EnumWithCustomName.Unknown, $"{value}");
    }

    [Fact]
    public void DeserializeEnumWithoutCustomName()
    {
        string deserialized = "\"Unknown\"";
        var value = JsonSerializer.Deserialize<EnumWithoutCustomName>(deserialized);

        Assert.True(value == EnumWithoutCustomName.Unknown, $"{value}");
    }

    [Fact]
    public void DeserializeInvalidEnum()
    {
        string deserialized = "\"Invalid\"";
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<EnumWithoutCustomName>(deserialized));
    }

    [Fact]
    public void SerializeEnumWithCustomName()
    {
        ClassWithEnumProperty jsonObj = new ClassWithEnumProperty {
            enumProp = EnumWithCustomName.Unknown,
            enumPropNoCustomName = EnumWithoutCustomName.Unknown
        };

        string serialized = JsonSerializer.Serialize<ClassWithEnumProperty>(jsonObj);
        Assert.Equal("{\"enumProp\":\"$UNKNOWN\",\"enumPropNoCustomName\":\"Unknown\"}", $"{serialized}");
    }
}
