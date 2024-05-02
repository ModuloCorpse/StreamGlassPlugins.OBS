using CorpseLib.Actions;
using StreamGlass.Core;

namespace OBSPlugin.Actions
{
    public class StartRecordAction(Manager manager) : AStreamGlassAction(ms_Definition)
    {
        private static readonly ActionDefinition ms_Definition = new("StartRecord", "Start OBS record");
        public override bool AllowDirectCall => true;
        public override bool AllowCLICall => true;
        public override bool AllowScriptCall => true;
        public override bool AllowRemoteCall => true;

        private readonly Manager m_Manager = manager;

        public override object?[] Call(object?[] _)
        {
            m_Manager.StartRecord();
            return [];
        }
    }
}
