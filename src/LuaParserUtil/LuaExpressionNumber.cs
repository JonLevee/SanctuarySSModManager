using System.Diagnostics;

namespace LuaParserUtil
{
    [DebuggerDisplay("{Name} = {Value}")]
    public class LuaExpressionNumber : LuaExpression
    {
        public static bool TryGetNumber(LuaTableLoaderState state, LuaString name, ref LuaExpression expression)
        {
            if (char.IsNumber(state.C) || (state.C == '-' && char.IsNumber(state.PeekC)))
            {
                var start = state.Index++;
                while (char.IsNumber(state.C))
                    ++state.Index;
                expression = new LuaExpressionNumber(name, new LuaString(state.SB, start, state.Index));
                return true;
            }
            return false;
        }

        public LuaExpressionNumber(LuaString name, LuaString value)
        {
            Name = name;
            Value = value;
        }

        public LuaString Name { get; }
        public LuaString Value { get; }
    }


}
