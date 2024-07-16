namespace LuaParserUtil.LuaParsingToken
{
    public class LuaTokenNumber : LuaToken
    {
        public string Number { get; }

        public static bool TryGet(LuaParsingState state, out LuaTokenNumber number)
        {
            var keep = state.Index;
            if (char.IsDigit(state.Current) || (state.Current == '-' && char.IsDigit(state.Next)))
            {
                while (state.MoveNext() && char.IsDigit(state.Current)) ;
                number = new LuaTokenNumber(state.SB.ToString(keep, state.Index - keep));
                return true;
            }
            number = null;
            return false;
        }

        public LuaTokenNumber(string number)
        {
            Number = number;
        }
    }
}
