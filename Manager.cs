using CorpseLib.Network;
using OBSCorpse;
using OBSCorpse.Requests;

namespace OBSPlugin
{
    public class Manager : IOBSHandler
    {
        private OBSProtocol? m_Client = null;
        private Settings? m_Settings = null;
        private readonly Dictionary<string, RadioSet> m_RadioSets = [];

        public void Add(RadioSet radioSet) => m_RadioSets[radioSet.Name] = radioSet;

        public void AddToSet(string setName, params Tuple<string, string>[] sources)
        {
            if (!m_RadioSets.ContainsKey(setName))
                m_RadioSets[setName] = new(setName);
            Dictionary<string, RadioItem> items = [];
            RadioSet set = m_RadioSets[setName];
            foreach (Tuple<string, string> source in sources)
            {
                if (items.TryGetValue(source.Item1, out RadioItem? item))
                    item.Add(source.Item2);
                else
                {
                    RadioItem newItem = new(source.Item1);
                    newItem.Add(source.Item2);
                    items[source.Item1] = newItem;
                    set.Add(newItem);
                }
            }
        }

        internal void SetSetting(Settings setting) => m_Settings = setting;

        public void StartClient()
        {
            if (m_Settings == null)
                return;
            m_Client?.Disconnect();
            string password = m_Settings.Password;
            m_Client = OBSProtocol.NewConnection(password, URI.Build("ws").Host(m_Settings.Host).Port(m_Settings.Port).Path("/").Build(), this);
            //OBSLayout layout = new();
            //layout.ReadLayout(m_Client!);
            //StreamGlassContext.LOGGER.Log(layout.ToString());
        }

        public void SetScene(string sceneName)
        {
            if (m_Client == null)
                return;
            m_Client.Send(new OBSSetCurrentProgramSceneRequest(sceneName));
        }

        public void SetSourceVisibility(string sceneName, string sourceName, bool visibility)
        {
            if (m_Client == null)
                return;
            OBSGetSceneSourceRequest getSceneSourceRequest = new(sceneName, sourceName);
            m_Client.Send(getSceneSourceRequest);
            if (getSceneSourceRequest.Source.ID >= 0)
            {
                OBSSetSceneItemEnabledRequest request = new(getSceneSourceRequest.Source, visibility);
                m_Client.Send(request);
            }
        }

        public void OnDisconnect() { }

        public void OnSceneChanged(string newScene) { }

        public void OnSceneItemEnableStateChanged(string sceneName, int sceneItemID, bool enabled)
        {
            if (!enabled || m_Client == null)
                return;
            OBSGetSceneSourceRequest request = new(sceneName, sceneItemID);
            m_Client.Send(request);
            OBSSource source = request.Source;
            foreach (RadioSet set in m_RadioSets.Values)
            {
                if (set.Contains(source.Scene, source.Name))
                {
                    List<OBSGetSceneSourceRequest> getSceneItemIds = [];
                    foreach (RadioItem item in set)
                    {
                        foreach (string itemSource in item.Sources)
                        {
                            if (item.SceneName != source.Scene || itemSource != source.Name)
                                getSceneItemIds.Add(new(item.SceneName, itemSource));
                        }
                    }
                    m_Client.Send(getSceneItemIds);
                    List<OBSSetSceneItemEnabledRequest> requests = [];
                    foreach (OBSGetSceneSourceRequest getSceneItemId in getSceneItemIds)
                    {
                        if (getSceneItemId.Source.ID >= 0)
                            requests.Add(new(getSceneItemId.Source, false));
                    }
                    m_Client.Send(requests);
                    return;
                }
            }
        }

        public void OnStreamStatusChanged(bool newStatus, string outputState) { }

        internal void SetSourceText(string scene, string source, string text)
        {
            if (m_Client == null)
                return;
            OBSSetTextRequest setTextRequest = new(new(scene, source, -1), text);
            m_Client.Send(setTextRequest);
        }
    }
}
