namespace LuaParserUtil.LuaParsingToken
{
    public class LuaTokenValue : LuaToken
    {
        public LuaToken Value { get; }

        // Value           := Name | Constant | String | Dictionary
        public static bool TryGet(LuaParsingState state, out LuaTokenValue value)
        {
            value = null;
            if (LuaTokenName.TryGet(state, out LuaTokenName name))
            {
                value = new LuaTokenValue(name);
                return true;
            }
            if (LuaTokenConstant.TryGet(state, out LuaTokenConstant constant))
            {
                value = new LuaTokenValue(constant);
                return true;
            }
            if (LuaTokenString.TryGet(state, out LuaTokenString stringValue))
            {
                value = new LuaTokenValue(stringValue);
                return true;
            }
            if (LuaTokenDictionary.TryGet(state, out LuaTokenDictionary dictionary))
            {
                value = new LuaTokenValue(dictionary);
                return true;
            }
            value = null;
            return false;
        }

        public LuaTokenValue(LuaToken value)
        {
            Value = value;
        }
    }
}
