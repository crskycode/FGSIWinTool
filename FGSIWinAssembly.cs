using System;
using System.Collections.Generic;
using System.Linq;

namespace FGSIWinTool
{
    public class FGSIWinAssembly
    {
        public LinkedList<FGSIWinInstructionRecord> Instructions { get; }

        public FGSIWinAssembly()
        {
            Instructions = new LinkedList<FGSIWinInstructionRecord>();
        }

        public void Add(FGSIWinInstruction instruction, long addr, long length)
        {
            var inst = new FGSIWinInstructionRecord();

            inst.Instruction = instruction;
            inst.Addr = Convert.ToInt32(addr);
            inst.NewAddr = Convert.ToInt32(addr);
            inst.Length = Convert.ToInt32(length);

            Instructions.AddLast(inst);
        }

        public IEnumerator<FGSIWinInstructionRecord> GetEnumerator()
        {
            return Instructions.GetEnumerator();
        }

        public int Count
        {
            get => Instructions.Count;
        }

        public int TotalLength
        {
            get => Instructions.Sum(x => x.Length);
        }
    }
}
