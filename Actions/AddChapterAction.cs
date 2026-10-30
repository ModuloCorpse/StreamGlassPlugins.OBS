using CorpseLib.Actions;
using StreamGlass.Core;

namespace OBSPlugin.Actions
{
    public class AddChapterAction(Manager manager) : AStreamGlassAction(ms_Definition)
    {
        private static readonly ActionDefinition ms_Definition = new("AddChapter", "Add a chapter in OBS record");
        public override bool AllowDirectCall => true;
        public override bool AllowCLICall => true;
        public override bool AllowScriptCall => true;
        public override bool AllowRemoteCall => true;

        private readonly Manager m_Manager = manager;

        public override async Task<object?[]> Call(object?[] args)
        {
            string chapterName = args.Length > 0 && args[0] is string name ? name : string.Empty;
            string verticalChapterName = args.Length > 1 && args[1] is string verticalName ? verticalName : string.Empty;
            m_Manager.AddChapter(chapterName, verticalChapterName);
            return [];
        }
    }
}
