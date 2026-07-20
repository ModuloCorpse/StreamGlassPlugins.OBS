using CorpseLib.Network.API;
using CorpseLib.Network.Http;

namespace OBSPlugin.Endpoints
{
    public class ClipEndpoint(Manager manager) : AHTTPEndpoint(new("clip"))
    {
        private readonly Manager m_Manager = manager;

        protected override async Task<Response> OnPostRequest(Request request)
        {
            m_Manager.AddChapter();
            return new(200, "Ok");
        }
    }
}
