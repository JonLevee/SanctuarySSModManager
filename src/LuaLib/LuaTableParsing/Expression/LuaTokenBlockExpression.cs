namespace SanctuarySSLib.LuaTableParsing.Expression
{
    public class LuaTokenBlockExpression : LuaTokenExpression
    {
        public List<LuaTokenExpression> Children { get; set; }
        public LuaTokenBlockExpression(string op) : base(op)
        {
            Children = new List<LuaTokenExpression>();
        }
    }
}