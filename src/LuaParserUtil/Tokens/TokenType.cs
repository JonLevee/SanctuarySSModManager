namespace LuaParserUtil.Tokens
{
    [Flags]
    public enum TokenType
    {
        Null = 0,
        Comment = 1 << 1,
        Name = 1 << 2,
        OpenBrace = 1 << 3,
        CloseBrace = 1 << 4,
        FunctionDefinition = Name | 1 << 5,
        Assignment = 1 << 6,
        Separator = 1 << 7,
        String = 1 << 8,
        Property = 1 << 9,
        OpenParam = 1 << 10,
        CloseParam = 1 << 11,
        Concat = 1 << 12,
        Number = 1 << 13,
        Double = 1 << 14,
        Variable = Name | 1 << 15,
    }
}
