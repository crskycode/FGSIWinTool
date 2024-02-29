namespace FGSIWinTool
{
    public enum FGSIWinInstruction
    {
        Unused = 0x00,
        Throw = 0x01,
        Op0A = 0x0A,
        Op0B = 0x0B,
        Op0C = 0x0C,
        Op0D = 0x0D,
        Op0E = 0x0E,
        Op0F = 0x0F,
        Op10 = 0x10,
        Op11 = 0x11,
        Msg = 0x14,
        CJmp = 0x15,
        Jmp = 0x16,
        CallFunc = 0x17,
        Op18 = 0x18,
        Call = 0x19,
        Op1A = 0x1A,
        PushDword = 0x32,
        PushString = 0x33,
        Add = 0x34,
        Sub = 0x35,
        Mul = 0x36,
        Div = 0x37,
        Mod = 0x38,
        Op39 = 0x39,
        Op3A = 0x3A, // is one
        Op3B = 0x3B, // not one
        And = 0x3C,
        Or = 0x3D,
        Lt = 0x3E,
        Gt = 0x3F,
        Le = 0x40,
        Ge = 0x41,
        Eq = 0x42,
        Op43 = 0x43, // not zero
    }
}
