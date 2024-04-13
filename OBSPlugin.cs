using CorpseLib.Json;
using StreamGlass.Core;
using StreamGlass.Core.Plugin;

namespace OBSPlugin
{
    public class OBSPlugin() : APlugin("OBS")
    {
        private readonly Manager m_Manager = new();
        private Settings m_Settings = new();

        protected override PluginInfo GeneratePluginInfo() => new("1.0.0-beta", "ModuloCorpse<https://www.twitch.tv/chaporon_>");

        protected override void OnLoad()
        {
            JsonHelper.RegisterSerializer(new RadioItem.JsonSerializer());
            JsonHelper.RegisterSerializer(new RadioSet.JsonSerializer());
            JsonHelper.RegisterSerializer(new Settings.JsonSerializer());

            string settingsFilePath = GetFilePath("settings.json");
            if (File.Exists(settingsFilePath))
            {
                Settings? settings = JsonParser.LoadFromFile<Settings>(settingsFilePath);
                if (settings != null)
                    m_Settings = settings;
            }

            m_Manager.SetSetting(m_Settings);
            foreach (RadioSet set in m_Settings.Radios)
                m_Manager.Add(set);
            OBSProcessListenerHandler processHandler = new(m_Manager);
            StreamGlassProcessListener.RegisterProcessHandler("obs64.exe", processHandler);
            StreamGlassContext.RegisterAggregator(() => new StringSourceAggregatorOBSSource(m_Manager));
        }

        protected override void OnInit() { }

        protected override void OnUnload() => JsonParser.WriteToFile<Settings>(GetFilePath("settings.json"), m_Settings);
    }
}
