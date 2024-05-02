using CorpseLib.Actions;
using StreamGlass.Core;

namespace OBSPlugin.Actions
{
    public class StopRecordAction(Manager manager) : AStreamGlassAction(ms_Definition)
    {
        private static readonly ActionDefinition ms_Definition = new("StopRecord", "Stop OBS record");
        public override bool AllowDirectCall => true;
        public override bool AllowCLICall => true;
        public override bool AllowScriptCall => true;
        public override bool AllowRemoteCall => true;

        private readonly Manager m_Manager = manager;

        public override object?[] Call(object?[] _)
        {
            m_Manager.StopRecord();
            return [];
        }
    }
}
