using System.Diagnostics;

namespace LuaParserUtil.LuaParsingToken
{
    [DebuggerDisplay("Key='{Key}'")]
    public class LuaTokenKey : LuaToken
    {
        public string Key { get; }

        // Key             := Letter{Letter | Number} | '['Number']' | [String] 
        public static bool TryGetKey(LuaParsingState state, out LuaToken token)
        {
            token = LuaToken.Empty;
            var keep = state.Index;
            // Letter{Letter | Number}
            if (LuaTokenName.TryGet(state, out LuaTokenName name))
            {
                token = new LuaTokenKey(name.Name);
                state.SkipWhitespace();
                return true;
            }
            // '['Number']' | [String] 
            if (state.CanSkipToMatchingChar)
            {
                state.SkipPastMatchingChar();
                token = new LuaTokenKey(state.SB.ToString(keep, state.Index - keep));
                state.SkipWhitespace();
                return true;
            }
            return false;
        }

        public LuaTokenKey(string token)
        {
            Key = token;
        }
    }

}
