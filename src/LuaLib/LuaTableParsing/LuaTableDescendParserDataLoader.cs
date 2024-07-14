using SanctuarySSLib.LuaTableParsing.Expression;
using SanctuarySSLib.Models;
using SanctuarySSLib.WorkInProgressNotUsed;
using SanctuarySSModManager.Extensions;
using System.Diagnostics;
using System.Text;

namespace SanctuarySSLib.LuaTableParsing
{


    public class LuaTableDescendParserDataLoader : ILuaTableDataLoader
    {
        private List<LuaTokenExpression> expressions = new List<LuaTokenExpression>();
        private readonly string[] unaryTokens = ["not"];
        private readonly LuaParsingTokenHandlers tokenHandlers = new LuaParsingTokenHandlers();

        private readonly Dictionary<string, string> operatorHandlers = new Dictionary<string, string>
        {
            { "=", string.Empty },
        };
        private readonly char[] operators;
        private LuaParsingState state;

        public LuaTableDescendParserDataLoader()
        {
            operators = string.Concat(operatorHandlers.Keys).ToCharArray().Distinct().ToArray();
            state = LuaParsingState.Empty;
        }

        public IEnumerable<string> GetUnsupportedTableNames()
        {
            yield break;
        }


        public void Load(LuaTableData tableData)
        {
            state = new LuaParsingState(tableData.FilePath, tableData.FileData);
            while (TryGetLuaExpression(out LuaTokenExpression expression))
            {
                expressions.Add(expression);
            }
            throw new NotImplementedException();
        }


        private bool TryGetLuaExpression(out LuaTokenExpression expression)
        {
            expression = LuaTokenExpression.Empty;

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
}