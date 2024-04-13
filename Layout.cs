using OBSCorpse;
using OBSCorpse.Requests;
using System.Collections;
using System.Text;

namespace OBSPlugin
{
    public class Layout
    {
        public class SourceItem(OBSSource source)
        {
            private readonly OBSSource m_Source = source;
            public OBSSource Source => m_Source;
            public string Scene => m_Source.Scene;
            public string Name => m_Source.Name;
            public int ID => m_Source.ID;
        }

        public class GroupItem(OBSSource source) : SourceItem(source), IEnumerable<SourceItem>
        {
            public List<SourceItem> m_Items = [];
            public void Add(SourceItem item) => m_Items.Add(item);
            public IEnumerator<SourceItem> GetEnumerator() => ((IEnumerable<SourceItem>)m_Items).GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)m_Items).GetEnumerator();
        }

        public class Scene(string name) : IEnumerable<SourceItem>
        {
            public List<SourceItem> m_Items = [];

            private readonly string m_Name = name;
            public string Name => m_Name;

            public void Add(SourceItem item) => m_Items.Add(item);
            public IEnumerator<SourceItem> GetEnumerator() => ((IEnumerable<SourceItem>)m_Items).GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)m_Items).GetEnumerator();
        }

        private readonly List<Scene> m_Scenes = [];

        private static SourceItem LoadItem(OBSProtocol client, OBSSceneItem item)
        {
            if (item.IsGroup)
            {
                GroupItem group = new(item.Source);
                OBSGetGroupSceneItemListRequest getGroupItemListRequest = new(group.Name);
                client.Send(getGroupItemListRequest);
                foreach (OBSSceneItem groupItem in getGroupItemListRequest.SceneItems)
                    group.Add(LoadItem(client, groupItem));
                return group;
            }
            else
                return new SourceItem(item.Source);
        }

        public void ReadLayout(OBSProtocol client)
        {
            m_Scenes.Clear();
            OBSGetSceneListRequest getSceneListRequest = new();
            client.Send(getSceneListRequest);
            foreach (OBSScene scene in getSceneListRequest.SceneList.ScenesList)
            {
                OBSGetSceneItemListRequest getSceneItemListRequest = new(scene.Name);
                client.Send(getSceneItemListRequest);
                Scene newScene = new(scene.Name);
                foreach (OBSSceneItem item in getSceneItemListRequest.SceneItems)
                    newScene.Add(LoadItem(client, item));
                m_Scenes.Add(newScene);
            }
        }

        private static void AppendItem(StringBuilder builder, SourceItem item, int depth)
        {
            string indent = new('\t', depth);
            if (item is GroupItem group)
            {
                builder.AppendFormat("{0}- {1}", indent, item.Name);
                builder.AppendLine();
                foreach (SourceItem groupItem in group)
                    AppendItem(builder, groupItem, depth + 1);
            }
            else
            {
                builder.AppendFormat("{0}- {1}", indent, item.Name);
                builder.AppendLine();
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new();
            foreach (Scene scene in m_Scenes)
            {
                builder.AppendFormat("- {0}", scene.Name);
                builder.AppendLine();
                foreach (SourceItem item in scene)
                    AppendItem(builder, item, 1);
            }
            return builder.ToString();
        }
    }
}
