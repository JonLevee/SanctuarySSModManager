using System.Diagnostics;

namespace SanctuarySSLib.LuaTableParsing.Expression
{
    [DebuggerDisplay("Op: [{Op}] Value: {Value.Value}")]
    public class LuaTokenBinaryExpression : LuaTokenExpression
    {
        public LuaTokenBinaryExpression(string op, string left, LuaTokenExpression right) : base(op)
        {
            LeftValue = left;
            RightValue = right;
        }

        public string LeftValue { get; }
        public LuaTokenExpression RightValue { get; }

    }
}