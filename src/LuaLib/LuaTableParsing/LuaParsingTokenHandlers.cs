using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
using SanctuarySSLib.LuaTableParsing.Expression;
using SanctuarySSLib.LuaTableParsing.ParserHandlers;
using Constraint = SanctuarySSModManager.Extensions.Constraint;

namespace SanctuarySSLib.LuaTableParsing
{
    public class LuaParsingTokenHandlers
    {
        private Dictionary<string, LuaParsingOperatorHandler> operatorHandlers;
        private HashSet<char> operatorChars = new HashSet<char>();
        public LuaParsingTokenHandlers()
        {
            operatorHandlers = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => !t.IsAbstract)
                .Where(t => t.IsAssignableTo(typeof(LuaParsingOperatorHandler)))
                .Select(t =>
                {
                    var instance = Activator.CreateInstance(t) as LuaParsingOperatorHandler;
                    Constraint.NotNull(instance);
                    return instance;
                })
                .ToDictionary(i => i.Operator);
            operatorChars = new HashSet<char>(operatorHandlers.Keys.SelectMany(k => k).Distinct().ToArray());
        }

        public bool IsOperator(char c) => operatorChars.Contains(c);

        public bool TryHandle(LuaParsingState state, string tokenName, string opName, out LuaTokenExpression expression)
        {
            if (!operatorHandlers.TryGetValue(opName, out var handler))
            {
                throw new LuaParsingException(state, "invalid operator");
            }
            var handlerState = new LuaParsingOperatorState(this, state, tokenName, opName);
            expression = handler.GetExpression(handlerState);
            return false;
        }

        public LuaTokenExpression GetExpression(LuaParsingState state)
        {

            // advance past any whitespace
            if (!state.TrySkipWhitespace())
            {
                throw new LuaParsingException(state);
            }

            // comment is special case
            if (state.CurrentChar == '-' && state.NextChar == '-')
            {
                state.AdvanceToEOL(2);
                var comment = new LuaTokenUnaryExpression("Comment");
                state.AdvanceToEOL();
                comment.Value = new LuaTokenValue(state);
                return comment;
            }

            if (!state.TryGetTokenName(out string tokenName))
            {
                throw new LuaParsingException(state);
            }

            if (!state.TryGetOpName(this, out string opName))
            {
                throw new LuaParsingException(state);
            }

            if (!TryHandle(state, tokenName, opName, out LuaTokenExpression expression))
            {
                throw new LuaParsingException(state);
            }
            return expression;

        }

    }
}