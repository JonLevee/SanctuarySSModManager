namespace LuaParserUtil.ToDelete
{
    public class LuaExpressionList : LuaExpression
    {
        public static LuaExpressionList Empty = new LuaExpressionList(LuaString.Empty);

        public LuaExpressionList(LuaString name)
        {
            Items = new List<LuaExpression>();
            Name = name;
        }

        public List<LuaExpression> Items { get; set; }
        public LuaString Name { get; }
    }


}
