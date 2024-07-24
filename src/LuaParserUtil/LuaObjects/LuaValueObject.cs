namespace LuaParserUtil.LuaObjects
{
    public class LuaValueObject : LuaObject
    {
        public static readonly LuaValueObject Null = new LuaValueObject();
        protected LuaValueObject() { }
        public bool IsDictionary => this is LuaDictionary;
        public bool IsValue => this is LuaValue;
    }
}
