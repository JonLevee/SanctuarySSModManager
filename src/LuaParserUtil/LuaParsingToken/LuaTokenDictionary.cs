namespace LuaParserUtil.LuaParsingToken
{
    public class LuaTokenDictionary : LuaToken
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
            state.SkipWhitespace();
            dictionary = new LuaTokenDictionary();
            while (!state.MoveIf('}'))
            {
                if (LuaTokenComment.TryGetComment(state, out LuaTokenComment comment))
                {
                    continue;
                }
                else if (LuaTokenKeyValuePair.TryGet(state, out LuaTokenKeyValuePair keyValuePair))
                {
                    dictionary.Add(keyValuePair);
                }
                else if (LuaTokenName.TryGet(state, out LuaTokenName name))
                {
                    dictionary.Add(name);
                }
                else if (LuaTokenConstant.TryGet(state, out LuaTokenConstant constant))
                {
                    dictionary.Add(constant);
                }
                else if (LuaTokenString.TryGet(state, out LuaTokenString stringValue))
                {
                    dictionary.Add(stringValue);
                }
                else if (LuaTokenDictionary.TryGet(state, out LuaTokenDictionary dictionaryValue))
                {
                    dictionary.Add(dictionaryValue);
                }
                else
                {
                    throw new InvalidOperationException();
                }
                if (state.MoveIf(','))
                    state.SkipWhitespace();
            }
            state.SkipWhitespace();
            return true;
        }

        public LuaTokenDictionary()
        {
            Dictionary = new Dictionary<object, object>();
            ListLength = 0;
        }

        public Dictionary<object, object> Dictionary { get; private set; }
        public int ListLength { get; private set; }

        public void Add(LuaToken arrayValue)
        {
            Dictionary.Add(++ListLength, arrayValue);
        }
        public void Add(LuaTokenKey key, LuaTokenValue value)
        {
            Dictionary.Add(key.Key, value);
        }
    }
}
