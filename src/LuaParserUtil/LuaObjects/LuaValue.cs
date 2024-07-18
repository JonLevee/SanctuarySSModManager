using System.Diagnostics;
using LuaParserUtil.Tokens;

namespace LuaParserUtil.LuaObjects
{
    [DebuggerDisplay("{Token}")]
    public class LuaValue : LuaValueObject
    {
        public static readonly LuaValue Null = new LuaValue();
        private LuaValue() { }


        public LuaValue(Token token)
        {
            Token = token;
        }

        public Token Token { get; }
    }
}
