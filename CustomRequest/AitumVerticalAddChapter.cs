using CorpseLib.DataNotation;
using OBSCorpse.Requests;

namespace OBSPlugin.CustomRequest
{
    public class AitumVerticalAddChapter : AOBSCallVendorRequestRequest
    {
        public AitumVerticalAddChapter() : base("aitum-vertical-canvas", "add_chapter") { }
        public AitumVerticalAddChapter(string chapterName) : base("aitum-vertical-canvas", "add_chapter", new DataObject() { { "chapter_name", chapterName } }) { }

        private bool m_Success = false;
        public bool Success => m_Success;
        protected override async Task OnResponse(Response response) => m_Success = response.Result;
    }
}
