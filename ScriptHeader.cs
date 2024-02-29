namespace FGSIWinTool
{
    public class ScriptHeader
    {
        public int Magic { get; set; }
        public int Unknow { get; set; }
        public int LabelCount { get; set; }
        public int LabelPos { get; set; }
        public int CodePos { get; set; }
    }
}
