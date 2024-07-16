namespace LuaParserUtil.LuaParsingToken
{
    public class LuaTokenName : LuaToken
    {
        public string Name { get; }

        public static bool TryGet(LuaParsingState state, out LuaTokenName name)
        {
            var keep = state.Index;
            if (!char.IsLetter(state.Current))
            {
                name = null;
                return false;
            }
            while (state.MoveNext() && char.IsLetterOrDigit(state.Current)) ;
            name = new LuaTokenName(state.SB.ToString(keep, state.Index - keep));
            state.SkipWhitespace();
            return true;
        }

        public LuaTokenName(string name)
        {
            Name = name;
        }
    }
}
