namespace LuaParserUtil.LuaParsingToken
{
    public class LuaTokenBoolean : LuaToken
    {
        public string Value { get; }

        public static bool TryGet(LuaParsingState state, out LuaTokenBoolean boolValue)
        {
            var keep = state.Index;
            if (char.IsLetter(state.Current))
            {
                while (state.MoveNext() && char.IsLetterOrDigit(state.Current)) ;
                var text = state.SB.ToString(keep, state.Index - keep);
                if (bool.TryParse(text, out _))
                {
                    boolValue = new LuaTokenBoolean(text);
                    return true;
                }
            }
            state.Reset(keep);
            boolValue = null;
            return false;
        }
        public LuaTokenBoolean(string value)
        {
            Value = value;
        }
    }
}
