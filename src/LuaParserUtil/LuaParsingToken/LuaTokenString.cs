namespace LuaParserUtil.LuaParsingToken
{
    public class LuaTokenString : LuaToken
    {
        public string Value { get; }

        public static bool TryGet(LuaParsingState state, out LuaTokenString value)
        {
            var keep = state.Index;
            if (state.Current != '[' && state.CanSkipToMatchingChar)
            {
                state.SkipPastMatchingChar();
                value = new LuaTokenString(state.SB.ToString(keep, state.Index - keep));
                return true;
            }
            value = null;
            return false;
        }

        public LuaTokenString(string value)
        {
            Value = value;
        }
    }
}
