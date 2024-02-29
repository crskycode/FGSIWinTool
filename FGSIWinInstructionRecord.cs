namespace FGSIWinTool
{
    public class FGSIWinInstructionRecord
    {
        public FGSIWinInstruction Instruction { get; set; }
        public int Addr { get; set; }
        public int NewAddr { get; set; }
        public int Length { get; set; }
    }
}
