﻿using LuaParserUtil.LuaParsingToken;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LuaParserUtil.ParseTemp
{
    public class LuaTableDataLoader3 : ILuaTableDataLoader
    {
        private const RegexOptions regexOptions = RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline;
        private readonly Regex tableRegex = new Regex(@"^(?<table>[a-zA-Z0-9-]+)\s*=\s*(?<data>{\s*.+^})", regexOptions);
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public void Load(LuaTableData tableData)
        {
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

                    var tokens = new Stack<TempToken>(parser.Tokens.Reverse());
                    // first tokens should always be represent keyValue
                    if (!TryGetKeyValue(out LuaKeyValue o))
                        throw new InvalidOperationException();
                }
                catch (Exception e)
                {
                    logger.Debug(e, "while parsing {table}", m.Groups["table"].Value);
                    throw;
                }
            }
        }

        private Stack<TempToken> tokens = new Stack<TempToken>();
        private bool TryGetKeyValue(out LuaKeyValue keyValue)
        {
            keyValue = LuaKeyValue.Null;

            var token = tokens.Pop();
            if (TryGetToken(out TempToken name, TempTokenType.Name, TempTokenType.Variable))
            {
                if (TryGetToken(out TempToken assignment, TempTokenType.Assignment))
                {
                    if (TryGetValue(out LuaValue value))
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

        private bool TryGetValue(out LuaValue value)
        {
            var token = tokens.Pop();
            switch (token.TokenType)
            {
                case TempTokenType.OpenBrace:
                    if (!TryGetDictionary(out LuaDictionary dictionary))
                        throw new LuaParsingException("Unable to get dictionary");
                    value = new LuaValue(dictionary);
                    return true;
                default:
                        throw new LuaParsingException($"Don't know how to handle tokentype {token.TokenType}");
            }
        }

        private bool TryGetDictionary(out LuaDictionary dictionary)
        {
            var token = tokens.Pop();
            dictionary = new LuaDictionary();
            while (token.TokenType != TempTokenType.CloseBrace)
            {

            }
            return true;
        }

        private bool TryGetToken(out TempToken token, params TempTokenType[] tokenTypes)
        {
            token = TempToken.Null;
            if (!tokenTypes.Contains(tokens.Peek().TokenType))
                return false;
            token = tokens.Pop();
            return true;
        }
    }

    public class LuaDictionary : LuaObject
    {
        private Dictionary<object, LuaObject> dictionary = new Dictionary<object, LuaObject>();
        public LuaDictionary()
        {
        }
        public int ArrayLength { get; set; }
        public void Add(LuaObject value)
        {
            dictionary.Add(++ArrayLength, value);
        }
    }
    public class LuaObject
    {
        public LuaObject()
        {
        }
    }

    /// <summary>
    /// this class is used to hold any type of lua value
    /// </summary>
    public class LuaValue
    {
        public LuaValue(LuaDictionary dictionary)
        {
            Dictionary = dictionary;
        }

        public LuaDictionary Dictionary { get; }
    }

    public class LuaKeyValue : LuaObject
    {
        public static readonly LuaKeyValue Null = new LuaKeyValue(null, null);
        private readonly TempToken name;

        public LuaKeyValue(TempToken name, LuaValue value)
        {
            this.name = name;
            Value = value;
        }

        public string Key => name.Text;

        public LuaValue Value { get; }
    }

    public class LuaParsingException : Exception
    { 
        public LuaParsingException(string message) : base(message) 
        { 
        }
    }
}
