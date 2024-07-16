namespace LuaParserUtil.LuaParsingToken
{
    public class LuaTokenComment : LuaToken
    {
        public string Comment { get; }

        // Comment         := --{any}(\r|\n)
        public static bool TryGetComment(LuaParsingState state, out LuaTokenComment comment)
        {
            var index = state.Index;
            if (state.Current == '-' && state.MoveNext())
            {
                if (state.Current == '-')
                {
                    var begin = state.Index + 1;
                    while (state.MoveNext() && state.Current != '\r' && state.Current != '\n') ;
                    comment = new LuaTokenComment(state.SB.ToString(begin, state.Index - begin));
                    state.SkipWhitespace();
                    return true;
                }
            }
            state.Reset(index);
            comment = null;
            return false;
        }

        private LuaTokenComment(string comment)
        {
            Comment = comment;
        }
    }
}
