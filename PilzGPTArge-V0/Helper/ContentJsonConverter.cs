using PilzGPTArge_V0.Models;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class ContentListJsonConverter : JsonConverter<List<IContent>>
{
    public override List<IContent> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var contentList = new List<IContent>();

        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            foreach (var element in doc.RootElement.EnumerateArray())
            {
                var type = element.GetProperty("type").GetString();
                IContent content = type switch
                {
                    "text" => JsonSerializer.Deserialize<TextContent>(element.GetRawText(), options),
                    "image_url" => JsonSerializer.Deserialize<ImageContent>(element.GetRawText(), options),
                    _ => throw new NotSupportedException($"Content type '{type}' is not supported")
                };
                contentList.Add(content);
            }
        }

        return contentList;
    }

    public override void Write(Utf8JsonWriter writer, List<IContent> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        foreach (var content in value)
        {
            switch (content)
            {
                case TextContent textContent:
                    JsonSerializer.Serialize(writer, textContent, options);
                    break;
                case ImageContent imageContent:
                    JsonSerializer.Serialize(writer, imageContent, options);
                    break;
                default:
                    throw new NotSupportedException($"Content type '{content.GetType().Name}' is not supported");
            }
        }

        writer.WriteEndArray();
    }
}
