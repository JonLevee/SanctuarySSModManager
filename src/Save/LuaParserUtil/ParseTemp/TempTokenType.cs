﻿namespace LuaParserUtil.ParseTemp
{
    [Flags]
    public enum TempTokenType
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
        ParamsStart = 1 << 10,
        ParamsEnd = 1 << 11,
        Concat = 1 << 12,
        Number = 1 << 13,
        Variable = Name | 1 << 14,
    }
}