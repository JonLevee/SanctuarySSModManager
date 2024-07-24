namespace LuaParserUtil.ParseTemp
{
    public class TokenKeyValue : Token
    {
        public readonly static TokenKeyValue Null = new TokenKeyValue();

        public TokenValue Key { get; set; }
        public TokenValue Value { get; set; }

        protected TokenKeyValue() { }
        public TokenKeyValue(ParsingState state, TokenValue key, TokenValue value) : base()
        {
            Key = key;
            Value = value;
        }
    }
}
