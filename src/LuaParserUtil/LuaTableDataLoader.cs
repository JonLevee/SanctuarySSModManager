
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace LuaParserUtil
{
    public class LuaTableDataLoader : ILuaTableDataLoader
    {
        private const RegexOptions regexOptions = RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline;
        private readonly Regex tableRegex = new Regex(@"^(?<table>[a-zA-Z0-9-]+)\s*=\s*(?<data>{\s*.+^})", regexOptions);

        public LuaTableLoaderState state { get; private set; }

        public IEnumerable<string> GetUnsupportedTableNames()
        {
            yield break;
        }

        public LuaTableDataLoader()
        {
        }

        private bool TryAdvancePastComma()
        {
            state.SkipWhitespace();
            if (state.C == ',')
            {
                ++state.Index;
                state.SkipWhitespace();
                return true;
            }
            return false;
        }

        private bool TryGetList(LuaString name, ref LuaExpression expression)
        {
            if (state.C != '{')
                return false;
            ++state.Index;
            var list = new LuaExpressionList(name);
            while (state.C != '}')
            {
                list.Items.Add(GetExpression());
                var hasComma = TryAdvancePastComma();
            }
            ++state.Index;
            expression = list;
            return true;
        }



        public LuaExpression GetExpression()
        {
            var expression = LuaExpression.Empty;
            state.SkipWhitespace();
            if (state.TrySkipComment())
            {
                return GetExpression();
            }
            if (state.TryGetNamedToken(out LuaString name))
            {
                state.SkipPastAssignmentOperator();
                state.SkipWhitespace();
            }

            if (TryGetList(name, ref expression))
                return expression;

            if (LuaExpressionNumber.TryGetNumber(state, name, ref expression))
                return expression;

            if (LuaExpressionString.TryGet(state, name, ref expression))
                return expression;

            var line = state.Line;
            throw new NotImplementedException();
        }


        public void Load(LuaTableData tableData)
        {
            state = new LuaTableLoaderState(tableData.FileData);
            foreach (Match m in tableRegex.Matches(state.SB.ToString()))
            {
                state.SetMatch(m);
                var expression = GetExpression();
                if (expression is not LuaExpressionList)
                {
                    throw new InvalidOperationException();
                }
                tableData.TableData.Add((LuaExpressionList)expression);
            }
        }

    }


}
