using SanctuarySSLib.LuaTableParsing.Expression;

namespace SanctuarySSLib.LuaTableParsing.ParserHandlers
{
    public abstract class LuaParsingOperatorHandler
    {
        public abstract string Operator { get; }
        public abstract LuaTokenExpression GetExpression(LuaParsingOperatorState state);
    }
}