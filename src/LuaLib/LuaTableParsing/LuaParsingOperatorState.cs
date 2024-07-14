using System.Diagnostics;

namespace SanctuarySSLib.LuaTableParsing
{
    [DebuggerDisplay("Token: {TokenName} OpName: {OpName}")]
    public class LuaParsingOperatorState
    {
        public LuaParsingOperatorState(LuaParsingTokenHandlers parsingHandler, LuaParsingState parsingState, string tokenName, string opName)
        {
            ParsingHandler = parsingHandler;
            ParsingState = parsingState;
            TokenName = tokenName;
            OpName = opName;
        }

        public LuaParsingTokenHandlers ParsingHandler { get; }
        public LuaParsingState ParsingState { get; }
        public string TokenName { get; }
        public string OpName { get; }
    }
}