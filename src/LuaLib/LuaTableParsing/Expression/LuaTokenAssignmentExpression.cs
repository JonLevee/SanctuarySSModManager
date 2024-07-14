namespace SanctuarySSLib.LuaTableParsing.Expression
{
    public class LuaTokenAssignmentExpression : LuaTokenBinaryExpression
    {
        public LuaTokenAssignmentExpression(string op, string left, LuaTokenExpression right) : base(op, left, right)
        {
        }
    }
}