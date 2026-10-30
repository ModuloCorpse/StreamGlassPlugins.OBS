using CorpseLib.DataNotation;
using CorpseLib.Json;
using CorpseLib.Network.API;
using CorpseLib.Network.Http;

namespace OBSPlugin.Endpoints
{
    public class ChapterEndpoint(Manager manager) : AHTTPEndpoint(new("chapter"))
    {
        private readonly Manager m_Manager = manager;

        protected override async Task<Response> OnPostRequest(Request request)
        {
            string chapterName = string.Empty;
            string verticalChapterName = string.Empty;
            string body = request.Body;
            if (!string.IsNullOrEmpty(body))
            {
                try
                {
                    DataObject jsonObject = JsonParser.Parse(body);
                    if (jsonObject.TryGet("name", out string? nameValue) && !string.IsNullOrEmpty(nameValue))
                    {
                        chapterName = nameValue;
                        verticalChapterName = nameValue;
                    }
                    if (jsonObject.TryGet("verticalName", out string? verticalChapterNameValue) && !string.IsNullOrEmpty(verticalChapterNameValue))
                        verticalChapterName = verticalChapterNameValue;
                }
                catch { }
            }
            m_Manager.AddChapter(chapterName, verticalChapterName);
            return new(200, "Ok");
        }
    }
}
