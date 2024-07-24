using System.Diagnostics;

namespace LuaParserUtil.ToDelete
{
    [DebuggerDisplay("{Name} = {Value}")]
    public class LuaExpressionString : LuaExpression
    {
        public static bool TryGet(LuaTableLoaderState state, LuaString name, ref LuaExpression expression)
        {
            if (state.C == '"' || state.C == '\'')
            {
                var endChar = state.C;
                var start = ++state.Index;
                while (state.C != endChar && !(state.C == endChar && state.PreviousC != '\\'))
                    ++state.Index;
                expression = new LuaExpressionString(name, new LuaString(state.SB, start, state.Index));
                ++state.Index;
                return true;
            }
            return false;
        }

        public LuaExpressionString(LuaString name, LuaString value)
        {
            Name = name;
            Value = value;
        }

        public LuaString Name { get; }
        public LuaString Value { get; }

    }
}
