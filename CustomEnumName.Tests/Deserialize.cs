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

    public class ClassWithEnumProperty
    {
        public EnumWithCustomName enumProp { set; get; }
    }

    [Fact]
    public void DeserializeEnumWithCustomName()
    {
        var options = new JsonSerializerOptions();
        string deserialized = "\"$UNKNOWN\"";
        var value = JsonSerializer.Deserialize<EnumWithCustomName>(deserialized, options);

        Assert.True(value == EnumWithCustomName.Unknown, "Should be deserialized as Unknown");
    }
    [Fact]
    public void SerializeEnumWithCustomName()
    {
        ClassWithEnumProperty jsonObj = new ClassWithEnumProperty {
            enumProp = EnumWithCustomName.Unknown
        };

        string serialized = JsonSerializer.Serialize<ClassWithEnumProperty>(jsonObj);
        Assert.Equal("{\"enumProp\":\"$UNKNOWN\"}", $"{serialized}");
    }
}