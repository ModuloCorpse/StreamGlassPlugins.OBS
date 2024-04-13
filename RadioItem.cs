using CorpseLib;
using CorpseLib.Json;

namespace OBSPlugin
{
    public class RadioItem(string sceneName)
    {
        public class JsonSerializer : AJsonSerializer<RadioItem>
        {
            protected override OperationResult<RadioItem> Deserialize(JsonObject reader)
            {
                if (reader.TryGet("scene", out string? scene) && scene != null)
                {
                    RadioItem item = new(scene);
                    List<string> sources = reader.GetList<string>("sources");
                    foreach (string source in sources)
                        item.Add(source);
                    return new(item);
                }
                return new("Deserialization error", "No scene field in json");
            }

            protected override void Serialize(RadioItem obj, JsonObject writer)
            {
                writer["scene"] = obj.m_SceneName;
                writer["sources"] = obj.Sources;
            }
        }

        private readonly HashSet<string> m_Sources = [];
        private readonly string m_SceneName = sceneName;

        public string[] Sources => [.. m_Sources];
        public string SceneName => m_SceneName;

        public bool Contains(string source) => m_Sources.Contains(source);
        public void Add(string source) => m_Sources.Add(source);
    }
}
