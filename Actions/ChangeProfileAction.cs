using CorpseLib.Actions;
using StreamGlass.Core;

namespace OBSPlugin.Actions
{
    public class ChangeProfileAction(Manager manager) : AStreamGlassAction(ms_Definition)
    {
        private static readonly ActionDefinition ms_Definition = new ActionDefinition("ChangeProfile", "Change current OBS profile")
            .AddArgument<string>("profile", "Profile to change to");
        public override bool AllowDirectCall => true;
        public override bool AllowCLICall => true;
        public override bool AllowScriptCall => true;
        public override bool AllowRemoteCall => true;

        private readonly Manager m_Manager = manager;

        public override object?[] Call(object?[] args)
        {
            m_Manager.SetProfile((string)args[0]!);
            return [];
        }
    }
}
