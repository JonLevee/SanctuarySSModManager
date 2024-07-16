namespace LuaParserUtil.LuaParsingToken
{
    public class LuaTokenDictionary: LuaToken
    {
        // Value           := Name | Constant | String | Dictionary
        public static bool TryGet(LuaParsingState state, out LuaTokenDictionary dictionary)
        {
            dictionary = null;
            var keep = state.Index;
            if (!state.MoveIf('{'))
            {
                return false;
            }
            if (!LuaTokenKey.TryGetKey(state, out LuaToken key))
            {
                state.Reset(keep);
            }
        }

        public LuaTokenDictionary()
        {
        }
    }
}
