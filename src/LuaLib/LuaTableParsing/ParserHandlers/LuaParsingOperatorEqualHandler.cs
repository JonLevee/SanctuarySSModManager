using SanctuarySSLib.LuaTableParsing.Expression;

namespace SanctuarySSLib.LuaTableParsing.ParserHandlers
{
    public class LuaParsingOperatorEqualHandler : LuaParsingOperatorHandler
    {
        public override string Operator => "=";

        public override LuaTokenExpression GetExpression(LuaParsingOperatorState state)
        {
            if (!state.ParsingState.TrySkipWhitespace())
            {
                throw new LuaParsingException(state.ParsingState);
            }

            var expression = new LuaTokenAssignmentExpression(
                state.OpName,
                state.TokenName,
                state.ParsingHandler.GetExpression(state.ParsingState));

            return expression;
        }
    }
    public class LuaParsingOperatorBlockHandler : LuaParsingOperatorHandler
    {
        public override string Operator => "{";

        public override LuaTokenExpression GetExpression(LuaParsingOperatorState state)
        {
            if (!state.ParsingState.TrySkipWhitespace())
            {
                throw new LuaParsingException(state.ParsingState);
            }

            var expression = new LuaTokenBlockExpression(
                state.OpName,
                state.TokenName,
                state.ParsingHandler.GetExpression(state.ParsingState));

            return expression;
        }
    }
}