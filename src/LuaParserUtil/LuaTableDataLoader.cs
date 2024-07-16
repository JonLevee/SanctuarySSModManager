
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace LuaParserUtil
{
    [DebuggerDisplay("{state}")]
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
                if (TryGetExpression(out LuaExpression childExpression))
                    list.Items.Add(childExpression);
                var hasComma = TryAdvancePastComma();
            }
            ++state.Index;
            expression = list;
            return true;
        }

        private bool TryGetConstant(LuaString name, ref LuaExpression expression)
        {
            if (LuaExpressionNumber.TryGet(state, name, ref expression))
                return true;

            if (LuaExpressionString.TryGet(state, name, ref expression))
                return true;

            if (LuaExpressionBoolean.TryGet(state, name, ref expression))
                return true;

            return false;
        }

        public bool TryGetExpression(out LuaExpression expression)
        {
            expression = LuaExpression.Empty;
            state.SkipWhitespace();
            while (state.TrySkipComment()) ;
            if (state.TryGetNamedToken(out LuaString name))
            {
                if (!state.TrySkipPastAssignmentOperator())
                {
                    // reset to get constant
                    state.Index -= name.Length;
                    if (!TryGetConstant(LuaString.Empty, ref expression))
                        throw new InvalidOperationException();
                    return true;
                }
                state.SkipWhitespace();
            }
            if (TryGetList(name, ref expression))
                return true;

            if (TryGetConstant(name, ref expression))
            {
                state.TrySkipComma();
                return true;
            }
            // if we are at end block, then this is an empty expression
            if (state.C == '}' && name.IsEmpty)
            {
                return false;
            }
            var line = state.Line;
            throw new NotImplementedException();
        }


        public void Load(LuaTableData tableData)
        {
            state = new LuaTableLoaderState(tableData.FileData, Path.GetFileName(tableData.FilePath));
            foreach (Match m in tableRegex.Matches(state.SB.ToString()))
            {
                state.SetMatch(m);
                if (TryGetExpression(out LuaExpression expression))
                {
                    if (expression is not LuaExpressionList)
                    {
                        throw new InvalidOperationException();
                    }
                    tableData.TableData.Add((LuaExpressionList)expression);
                }
            }
        }

    }
}
