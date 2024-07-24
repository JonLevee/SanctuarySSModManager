namespace LuaParserUtil.ParseTemp
{
    [Flags]
    public enum TokenType
    {
        Empty = 0,
        Number,
        Name,
        Key,
        KeyValue,
        Boolean,
        String,
        Editable = Number | String | Boolean,
        Comment = 8,
    }
}
