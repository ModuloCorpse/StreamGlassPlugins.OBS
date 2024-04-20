using CorpseLib.DataNotation;
using StreamGlass.Core.Stat;

namespace OBSPlugin
{
    public class StringSourceAggregatorOBSSource(Manager manager) : StringSourceAggregator
    {
        private readonly Manager m_Manager = manager;
        private string m_Scene = string.Empty;
        private string m_Source = string.Empty;

        protected override void OnSave(DataObject json)
        {
            json["scene"] = m_Scene;
            json["source"] = m_Source;
        }

        protected override void OnLoad(DataObject json)
        {
            if (json.TryGet("scene", out string? scene))
                m_Scene = scene!;
            if (json.TryGet("source", out string? source))
                m_Source = source!;
        }

        protected override string GetAggregatorType() => "obs_source";
        protected override void OnAggregate(string text) => m_Manager.SetSourceText(m_Scene, m_Source, text);
    }
}
