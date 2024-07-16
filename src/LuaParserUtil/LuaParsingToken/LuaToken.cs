namespace LuaParserUtil.LuaParsingToken
{
    public abstract class LuaToken
    {
        public static LuaToken Empty = new LuaTokenEmpty();
        public class LuaTokenEmpty : LuaToken 
        { 
        }
    }
}
