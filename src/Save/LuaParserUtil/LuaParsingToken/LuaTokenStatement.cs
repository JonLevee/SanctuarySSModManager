namespace LuaParserUtil.LuaParsingToken
{
    public class LuaTokenStatement : LuaToken
    {
        public LuaTokenKeyValuePair KeyValuePair { get; }
        public LuaTokenComment Comment { get; }

        // Statement       := KeyValuePair | Comment
        public static bool TryGet(LuaParsingState state, out LuaTokenStatement statement)
        {
            if (LuaTokenComment.TryGetComment(state, out LuaTokenComment comment))
            {
                statement = new LuaTokenStatement(null, comment);
                return true;
            }
            if (LuaTokenKeyValuePair.TryGet(state, out LuaTokenKeyValuePair keyValuePair))
            {
                statement = new LuaTokenStatement(keyValuePair, null);
                return true;
            }
            statement = null;
            return false;
        }

        public LuaTokenStatement(LuaTokenKeyValuePair keyValuePair, LuaTokenComment comment) 
        {
            KeyValuePair = keyValuePair;
            Comment = comment;
        }
    }
}
