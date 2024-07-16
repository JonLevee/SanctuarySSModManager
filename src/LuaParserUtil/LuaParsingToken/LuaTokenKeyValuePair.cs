using System.Diagnostics;

namespace LuaParserUtil.LuaParsingToken
{
    [DebuggerDisplay("Key='{Key}' Value='{Value}'")]
    public class LuaTokenKeyValuePair : LuaToken
    {
        public LuaToken Key { get; }
        public LuaTokenValue Value { get; }

        // KeyValuePair    := Key '=' Value
        public static bool TryGet(LuaParsingState state, out LuaTokenKeyValuePair keyValuePair)
        {
            keyValuePair = null;
            var keep = state.Index;
            if (LuaTokenKey.TryGetKey(state, out LuaToken key))
            {
                if (state.MoveIf('='))
                {
                    state.SkipWhitespace();
                    if (LuaTokenValue.TryGet(state, out LuaTokenValue value))
                    {
                        keyValuePair = new LuaTokenKeyValuePair(key, value);
                        return true;
                    }
                }
            }
            state.Reset(keep);
            return false;
        }

        public LuaTokenKeyValuePair(LuaToken key, LuaTokenValue value)
        {
            Key = key;
            Value = value;
        }
    }
}
