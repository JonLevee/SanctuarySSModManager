namespace LuaParserUtil.LuaParsingToken
{
    public class LuaTokenConstant : LuaToken
    {
        public string Constant { get; }

        // Name            := Letter{Letter | Number}
        // Constant        := String | Number
        // String          := Quote {PrintableSymbol} Quote
        // PrintableSymbol := Letter | Digit | ' '
        public static bool TryGet(LuaParsingState state, out LuaTokenConstant name)
        {
            var keep = state.Index;
            if (LuaTokenNumber.TryGet(state, out LuaTokenNumber number))
            {
                name = new LuaTokenConstant(number.Number);
                return true;
            }
            if (LuaTokenString.TryGet(state, out LuaTokenString value))
            {
                name = new LuaTokenConstant(value.Value);
                return true;
            }
            name = null;
            return false;
        }

        public LuaTokenConstant(string Constant)
        {
            this.Constant = Constant;
        }
    }
}
