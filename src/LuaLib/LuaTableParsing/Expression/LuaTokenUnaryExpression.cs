using System.Diagnostics;
using System.Text;

namespace SanctuarySSLib.LuaTableParsing.Expression
{
    [DebuggerDisplay("Op: [{Op}] Value: {Value.Value}")]
    public class LuaTokenUnaryExpression : LuaTokenExpression
    {
        public LuaTokenUnaryExpression(string op) : base(op)
        {
            Value = LuaTokenValue.Empty;
        }
        public LuaTokenUnaryExpression(StringBuilder op) : base(op)
        {
            Value = LuaTokenValue.Empty;
        }
        public LuaTokenValue Value { get; set; }

    }
}