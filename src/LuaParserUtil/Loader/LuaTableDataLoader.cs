using System.Diagnostics;
using System.Text.RegularExpressions;
using LuaParserUtil.LuaObjects;
using LuaParserUtil.Tokens;

namespace LuaParserUtil.Loader
{
    [DebuggerDisplay("{DebugLine}")]
    public class LuaTableDataLoader : ILuaTableDataLoader
    {
        private const RegexOptions regexOptions = RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline;
        private readonly Regex tableRegex = new Regex(@"^(?<table>[a-zA-Z0-9-]+)\s*=\s*(?<data>{\s*.+^})", regexOptions);
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private Stack<Token> tokens = new Stack<Token>();
        private Token currentToken = Token.Null;
        private Token previousToken = Token.Null;
        private string filePath;

        public void Load(LuaTableData tableData)
        {
            filePath = tableData.FilePath;
            var parser = new TokenParser(tableData.FileData, tableData.FilePath);
            foreach (Match m in tableRegex.Matches(tableData.FileData.ToString()))
            {
                var state = new ParsingState(tableData.FileData, m, tableData.FilePath);
                logger.Debug(" parsing {table}", m.Groups["table"].Value);
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
                    parser.Parse(m);

                    tokens = new Stack<Token>(parser.Tokens.Reverse());
                    // first tokens should always be represent keyValue
                    if (!TryGetKeyValue(out LuaKeyValue keyValue))
                        throw new LuaParsingException("Could not get top level keyValue");
                    tableData.Tables.Add(keyValue);
                }
                catch (Exception e)
                {
                    logger.Debug(e, "while parsing {table}", m.Groups["table"].Value);
                    throw;
                }
            }
        }

        private bool TryGetKeyValue(out LuaKeyValue keyValue)
        {
            keyValue = LuaKeyValue.Null;

            if (TryGetToken(out Token name, TokenType.Name, TokenType.Variable))
            {
                if (TryGetToken(out Token assignment, TokenType.Assignment))
                {
                    if (TryGetValue(out LuaValueObject value))
                    {
                        keyValue = new LuaKeyValue(name, value);
                        return true;
                    }
                    tokens.Push(assignment);
                }
                tokens.Push(name);
            }
            return false;
        }

        private readonly TokenType[] validValueTypes = [
            TokenType.Name,
            TokenType.String,
            TokenType.Number,
            TokenType.Double,
            TokenType.Variable
            ];
        private bool TryGetValue(out LuaValueObject value)
        {
            value = LuaValueObject.Null;
            if (TryGetDictionary(out LuaDictionary dictionary))
            {
                value = dictionary;
                return true;
            }

            if (TryGetToken(out Token token, validValueTypes))
            {
                value = new LuaValue(token);
                return true;
            }
            throw new LuaParsingException($"Don't know how to handle tokentype {PeekTokenType}");
        }

        private bool TryGetDictionary(out LuaDictionary dictionary)
        {
            dictionary = LuaDictionary.Null;
            if (!TryGetToken(out _, TokenType.OpenBrace))
                return false;
            dictionary = new LuaDictionary();
            while (PeekTokenType != TokenType.CloseBrace)
            {
                if (TryGetComment(out _))
                    continue;

                if (TryGetSeparator(out _))
                    continue;

                if (TryGetKeyValue(out LuaKeyValue keyValue))
                {
                    dictionary.Add(keyValue);
                    continue;
                }
                if (TryGetValue(out LuaValueObject value))
                {
                    dictionary.Add(value);
                    continue;
                }
                throw new LuaParsingException($"Don't know how to handle tokentype {PeekTokenType}");
            }
            EatToken(TokenType.CloseBrace);
            return true;
        }

        private bool TryGetSeparator(out LuaObject value)
        {
            value = LuaObject.Null;
            if (TryGetToken(out Token token, TokenType.Separator))
            {
                // eat it
                return true;
            }
            return false;
        }

        private bool TryGetComment(out LuaObject value)
        {
            value = LuaObject.Null;
            if (TryGetToken(out Token token, TokenType.Comment))
            {
                // eat it
                return true;
            }
            return false;
        }

        private bool TryGetToken(out Token token, params TokenType[] tokenTypes)
        {
            token = Token.Null;
            if (!tokenTypes.Contains(PeekTokenType))
                return false;
            token = GetToken();
            return true;
        }

        private void EatToken(TokenType tokenType)
        {
            if (!TryGetToken(out Token token, tokenType))
            {
                throw new LuaParsingException($"Expected tokentype {PeekTokenType}");
            }
        }
        private Token GetToken()
        {
            previousToken = currentToken;
            var token = currentToken = tokens.Pop();
            logger.Debug($"Popped {token.Description}");
            return token;
        }

        private TokenType PeekTokenType => tokens.Peek().TokenType;

        public string DebugLine
        {
            get => $"Prev:{previousToken.TokenType} Curr:{currentToken.TokenType} Next: {PeekTokenType} {tokens.Peek().GetDebugLine(filePath)}";
        }

    }
}
