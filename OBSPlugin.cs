using CorpseLib.DataNotation;
using CorpseLib.Json;
using OBSCorpse;
using OBSPlugin.Actions;
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
            OBSProtocol.StartLogging();
            DataHelper.RegisterSerializer(new RadioItem.DataSerializer());
            DataHelper.RegisterSerializer(new RadioSet.DataSerializer());
            DataHelper.RegisterSerializer(new Settings.DataSerializer());

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
            m_Manager.AddBrowserSourcesToRefresh(m_Settings.SourcesToRefresh);
            OBSProcessListenerHandler processHandler = new(m_Manager);
            StreamGlassProcessListener.RegisterProcessHandler("obs64.exe", processHandler);
            StreamGlassContext.RegisterAggregator(() => new StringSourceAggregatorOBSSource(m_Manager));

            StreamGlassActions.AddAction(new ChangeCollectionAction(m_Manager));
            StreamGlassActions.AddAction(new ChangeProfileAction(m_Manager));
            StreamGlassActions.AddAction(new ChangeSceneAction(m_Manager));
            StreamGlassActions.AddAction(new StartRecordAction(m_Manager));
            StreamGlassActions.AddAction(new StartStreamAction(m_Manager));
            StreamGlassActions.AddAction(new StopRecordAction(m_Manager));
            StreamGlassActions.AddAction(new StopStreamAction(m_Manager));
            StreamGlassActions.AddAction(new ToggleRecordAction(m_Manager));
        }

        protected override void OnInit() { }

        protected override void OnUnload() => JsonParser.WriteToFile<Settings>(GetFilePath("settings.json"), m_Settings);
    }
}
