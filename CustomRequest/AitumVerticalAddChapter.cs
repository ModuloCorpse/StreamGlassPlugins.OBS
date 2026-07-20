using OBSCorpse.Requests;

namespace OBSPlugin.CustomRequest
{
    public class AitumVerticalAddChapter() : AOBSCallVendorRequestRequest("aitum-vertical-canvas", "add_chapter")
    {
        private bool m_Success = false;
        public bool Success => m_Success;
        protected override async Task OnResponse(Response response) => m_Success = response.Result;
    }
}
