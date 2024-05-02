using StreamGlass.Core;

namespace OBSPlugin
{
    public class OBSProcessListenerHandler(Manager manager) : StreamGlassProcessListener.Handler()
    {
        private readonly Manager m_Manager = manager;

        protected override void OnNewProcessStart() { }

        protected override void OnNewProcessStop() { }

        protected override void OnProcessStart() => Task.Delay(3000).ContinueWith(_ => m_Manager.StartClient());

        protected override void OnProcessStop() { }
    }
}
