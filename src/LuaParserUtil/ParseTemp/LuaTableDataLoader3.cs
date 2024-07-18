using LuaParserUtil.LuaParsingToken;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LuaParserUtil.ParseTemp
{
    [DebuggerDisplay("{DebugLine}")]
    public class LuaTableDataLoader3 : ILuaTableDataLoader
    {
        private const RegexOptions regexOptions = RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline;
        private readonly Regex tableRegex = new Regex(@"^(?<table>[a-zA-Z0-9-]+)\s*=\s*(?<data>{\s*.+^})", regexOptions);
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private Stack<TempToken> tokens = new Stack<TempToken>();
        private TempToken currentToken = TempToken.Null;
        private TempToken previousToken = TempToken.Null;
        private string filePath;
        public void Load(LuaTableData tableData)
        {
            filePath = tableData.FilePath;
            var parser = new TempTokenParser(tableData.FileData, tableData.FilePath);
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

                    tokens = new Stack<TempToken>(parser.Tokens.Reverse());
                    // first tokens should always be represent keyValue
                    if (!TryGetKeyValue(out LuaKeyValue keyValue))
                        throw new LuaParsingException("Could not get top level keyValue");
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

            if (TryGetToken(out TempToken name, TempTokenType.Name, TempTokenType.Variable))
            {
                if (TryGetToken(out TempToken assignment, TempTokenType.Assignment))
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

        private readonly TempTokenType[] validValueTypes = [
            TempTokenType.Name, 
            TempTokenType.String, 
            TempTokenType.Number,
            TempTokenType.Double,
            TempTokenType.Variable
            ];
        private bool TryGetValue(out LuaValueObject value)
        {
            value = LuaValueObject.Null;
            if (TryGetDictionary(out LuaDictionary dictionary))
            {
                value = dictionary;
                return true;
            }

            if (TryGetToken(out TempToken token, validValueTypes))
            {
                value = new LuaValue(token);
                return true;
            }
            throw new LuaParsingException($"Don't know how to handle tokentype {PeekTokenType}");
        }

        private bool TryGetDictionary(out LuaDictionary dictionary)
        {
            dictionary = LuaDictionary.Null;
            if (!TryGetToken(out _, TempTokenType.OpenBrace))
                return false;
            dictionary = new LuaDictionary();
            while (PeekTokenType != TempTokenType.CloseBrace)
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
            EatToken(TempTokenType.CloseBrace);
            return true;
        }

        private bool TryGetSeparator(out LuaObject value)
        {
            value = LuaObject.Null;
            if (TryGetToken(out TempToken token, TempTokenType.Separator))
            {
                // eat it
                return true;
            }
            return false;
        }

        private bool TryGetComment(out LuaObject value)
        {
            value = LuaObject.Null;
            if (TryGetToken(out TempToken token, TempTokenType.Comment))
            {
                // eat it
                return true;
            }
            return false;
        }

        private bool TryGetToken(out TempToken token, params TempTokenType[] tokenTypes)
        {
            token = TempToken.Null;
            if (!tokenTypes.Contains(PeekTokenType))
                return false;
            token = GetToken();
            return true;
        }

        private void EatToken(TempTokenType tokenType)
        {
            if (!TryGetToken(out TempToken token, tokenType))
            {
                throw new LuaParsingException($"Expected tokentype {PeekTokenType}");
            }
        }
        private TempToken GetToken()
        {
            previousToken = currentToken;
            var token = currentToken = tokens.Pop();
            logger.Debug($"Popped {token.Description}");
            return token;
        }

        private TempTokenType PeekTokenType => tokens.Peek().TokenType;

        public string DebugLine
        {
            get => $"Prev:{previousToken.TokenType} Curr:{currentToken.TokenType} Next: {PeekTokenType} {tokens.Peek().GetDebugLine(filePath)}";
        }

    }

    public class LuaObject
    {
        public static readonly LuaObject Null = new LuaObject();
        public LuaObject()
        {
        }
    }

    public class LuaValueObject : LuaObject
    {
        public static readonly LuaValueObject Null = new LuaValueObject();
        protected LuaValueObject() { }
        public bool IsDictionary => this is LuaDictionary;
        public bool IsValue => this is LuaValue;
    }

    [DebuggerDisplay("Count:{dictionary.Count}")]
    public class LuaDictionary : LuaValueObject
    {
        public static readonly LuaDictionary Null = new LuaDictionary();
        private Dictionary<object, LuaValueObject> dictionary = new Dictionary<object, LuaValueObject>();
        public LuaDictionary()
        {
        }
        public int ArrayLength { get; set; }
        public void Add(LuaValueObject value)
        {
            dictionary.Add(++ArrayLength, value);
        }
        public void Add(LuaKeyValue keyValue)
        {
            dictionary.Add(keyValue.Key, keyValue.Value);
        }
    }

    [DebuggerDisplay("{Token}")]
    public class LuaValue : LuaValueObject
    {
        public static readonly LuaValue Null = new LuaValue();
        private LuaValue() { }


        public LuaValue(TempToken token)
        {
            Token = token;
        }

        public TempToken Token { get; }
    }

    public class LuaKeyValue : LuaObject
    {
        public static readonly LuaKeyValue Null = new LuaKeyValue(TempToken.Null, LuaValue.Null);

        public LuaKeyValue(TempToken key, LuaValueObject value)
        {
            KeyToken = key;
            Value = value;
        }

        public TempToken KeyToken { get; }
        public string Key => KeyToken.Text;
        public LuaValueObject Value { get; }
    }

    public class LuaParsingException : Exception
    { 
        public LuaParsingException(
            string message,
            [CallerMemberName]
            string caller = null) : base($"{caller}: {message}") 
        { 
            
        }
    }
}
