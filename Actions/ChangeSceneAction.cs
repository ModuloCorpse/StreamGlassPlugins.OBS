using CorpseLib.Actions;

namespace OBSPlugin.Actions
{
    public class ChangeSceneAction(Manager manager) : AAction(ms_Definition)
    {
        private static readonly ActionDefinition ms_Definition = new ActionDefinition("ChangeScene")
            .AddArgument<string>("scene");

        private readonly Manager m_Manager = manager;

        public override object?[] Call(object?[] args)
        {
            m_Manager.SetScene((string)args[0]!);
            return [];
        }
    }
}
