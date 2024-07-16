namespace LuaParserUtil.LuaParsingToken
{
    public class LuaTokenKeyValuePair : LuaToken
    {
        public LuaToken Key { get; }
        public LuaTokenValue Value { get; }

        // KeyValuePair    := Key '=' Value
        public static bool TryGet(LuaParsingState state, out LuaTokenKeyValuePair keyValuePair)
        {
            keyValuePair = null;
            var keep = state.Index;
            if (!LuaTokenKey.TryGetKey(state, out LuaToken key))
            {
                state.Reset(keep);
                return false;
            }
            if (state.Current != '=')
            {
                state.Reset(keep);
                return false;
            }
            state.MoveNext();
            state.SkipWhitespace();
            if (!LuaTokenValue.TryGet(state, out LuaTokenValue value))
            {
                state.Reset(keep);
                return false;
            }
            keyValuePair = new LuaTokenKeyValuePair(key, value);
            return true;
        }

        public LuaTokenKeyValuePair(LuaToken key, LuaTokenValue value) 
        {
            Key = key;
            Value = value;
        }
    }
}
