
using LuaParserUtil.LuaParsingToken;
using System.Text.RegularExpressions;

namespace LuaParserUtil
{
    public class LuaTableDataLoader2 : ILuaTableDataLoader
    {
        private const RegexOptions regexOptions = RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline;
        private readonly Regex tableRegex = new Regex(@"^(?<table>[a-zA-Z0-9-]+)\s*=\s*(?<data>{\s*.+^})", regexOptions);

        public void Load(LuaTableData tableData)
        {
            foreach (Match m in tableRegex.Matches(tableData.FileData.ToString()))
            {
                var state = new LuaParsingState(tableData.FileData, m);
                // Statement       := KeyValuePair | Comment
                // 
                // 
                // Dictionary      := { (KeyValuePair,{KeyValuePair} }
                // KeyValuePair    := Key '=' Value
                // Key             := Name | '['Number']' | [String] 
                // Comment         := --{any}(\r|\n)
                // Value           := Name | Constant | String | Dictionary
                // Name            := Letter{Letter | Number}
                // Constant        := String | Number
                // String          := Quote {PrintableSymbol} Quote
                // PrintableSymbol := Letter | Digit | ' '
                // Quote           := ' | "
                // Word            := Letter{Letter}
                // Number          := Digit{Digit}
                // Letter          := "a" .. "z" | "A" .. "Z"
                // Digit           := "0" | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9"

                try
                {
                    if (!LuaTokenStatement.TryGet(state, out LuaTokenStatement token))
                        throw new InvalidOperationException();
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }
    }
}
