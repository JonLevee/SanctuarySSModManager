using System.Diagnostics;
using System.Text;

namespace SanctuarySSLib.LuaTableParsing.Expression
{
    [DebuggerDisplay("Op: [{Op}]")]
    public abstract class LuaTokenExpression
    {
        public class LuaTokenExpressionEmpty : LuaTokenExpression
        {
            public LuaTokenExpressionEmpty() : base(string.Empty)
            {

            }
        }

        public static LuaTokenExpression Empty = new LuaTokenExpressionEmpty();
        protected LuaTokenExpression(string op)
        {
            Op = op;
        }
        protected LuaTokenExpression(StringBuilder op)
        {
            Op = op.ToString();
        }
        public string Op { get; }

    }
}