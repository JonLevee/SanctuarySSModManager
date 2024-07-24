using LuaParserUtil.Tokens;

namespace LuaParserUtil.LuaObjects
{
    public class LuaKeyValue : LuaObject
    {
        public static readonly LuaKeyValue Null = new LuaKeyValue(Token.Null, LuaValue.Null);

        public LuaKeyValue(Token key, LuaValueObject value)
        {
            KeyToken = key;
            Value = value;
        }

        public Token KeyToken { get; }
        public string Key => KeyToken.Text;
        public LuaValueObject Value { get; }
    }
}
