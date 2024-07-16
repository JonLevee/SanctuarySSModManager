using System.Diagnostics;

namespace LuaParserUtil
{
    [DebuggerDisplay("{Name} = {Value}")]
    public class LuaExpressionBoolean : LuaExpression
    {
        public static bool TryGet(LuaTableLoaderState state, LuaString name, ref LuaExpression expression)
        {
            var validValues = new string[]
            {
                "true",
                "false",
            };
            var end = state.Index;
            while (end < state.SB.Length && char.IsLetter(state.SB[end]))
                ++end;
            var text = state.SB.ToString(state.Index, end - state.Index);
            if (validValues.Contains(text))
            {
                expression = new LuaExpressionBoolean(name, new LuaString(state.SB, state.Index, end));
                state.Index = end;
                return true;
            }
            return false;
        }

        public LuaExpressionBoolean(LuaString name, LuaString value)
        {
            Name = name;
            Value = value;
        }

        public LuaString Name { get; }
        public LuaString Value { get; }

    }
}
