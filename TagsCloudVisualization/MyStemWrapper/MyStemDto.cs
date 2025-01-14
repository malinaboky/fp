using System.Text.Json.Serialization;

namespace TagsCloudVisualization.MyStemWrapper;

public class MyStemDto
{
    [JsonPropertyName("analysis")]
    public List<WordInfo> Analysis { get; set; }
    
    [JsonPropertyName("text")]
    public string Text { get; set; }
}

public class WordInfo
{
    [JsonPropertyName("lex")]
    public string Lemma { get; set; }
    
    [JsonPropertyName("gr")]
    public string Grammeme { get; set; }
}