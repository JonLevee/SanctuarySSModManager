using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
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
                state.GetCurrentLine(out string line, out int start);
                var message = $"Invalid operator at offset {start} in {state.FilePath} in line: {line}";
                throw new InvalidOperationException(message);
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
                return false;
            }

            // comment is special case
            if (state.CurrentChar == '-' && state.NextChar == '-')
            {
                state.AdvanceToEOL(2);
                var comment = new LuaTokenUnaryExpression("Comment");
                state.AdvanceToEOL();
                comment.Value = new LuaTokenValue(state);
                expression = comment;
                return true;
            }

            if (!state.TryGetTokenName(out string tokenName))
            {
                return false;
            }

            if (!state.TryGetOpName(tokenHandlers, out string opName))
            {
                return false;
            }

            if (!tokenHandlers.TryHandle(state, tokenName, opName, out expression))
            {
                return false;
            }
            return false;

        }

    }

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
    public abstract class LuaParsingOperatorHandler
    {
        public abstract string Operator { get; }
        public abstract LuaTokenExpression GetExpression(LuaParsingOperatorState state);
    }
    public class LuaParsingOperatorEqualHandler : LuaParsingOperatorHandler
    {
        public override string Operator => "=";

        public override LuaTokenExpression GetExpression(LuaParsingOperatorState state)
        {

            throw new NotImplementedException();
        }
    }
}