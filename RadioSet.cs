using CorpseLib;
using System.Collections;
using CorpseLib.DataNotation;

namespace OBSPlugin
{
    public class RadioSet(string name) : IEnumerable<RadioItem>
    {
        public class DataSerializer : ADataSerializer<RadioSet>
        {
            protected override OperationResult<RadioSet> Deserialize(DataObject reader)
            {
                if (reader.TryGet("name", out string? name) && name != null)
                {
                    RadioSet set = new(name);
                    List<RadioItem> items = reader.GetList<RadioItem>("items");
                    foreach (RadioItem item in items)
                        set.Add(item);
                    return new(set);
                }
                return new("Deserialization error", "No name field in json");
            }

            protected override void Serialize(RadioSet obj, DataObject writer)
            {
                writer["name"] = obj.m_Name;
                writer["items"] = obj.Items;
            }
        }

        private readonly Dictionary<string, RadioItem> m_Items = [];
        private readonly string m_Name = name;

        public RadioItem[] Items => [.. m_Items.Values];
        public string Name => m_Name;

        public bool Contains(string scene, string source)
        {
            if (m_Items.TryGetValue(scene, out RadioItem? item))
                return item.Contains(source);
            return false;
        }

        public void Add(RadioItem item) => m_Items[item.SceneName] = item;

        public IEnumerator<RadioItem> GetEnumerator() => ((IEnumerable<RadioItem>)m_Items.Values).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)m_Items.Values).GetEnumerator();
    }
}
