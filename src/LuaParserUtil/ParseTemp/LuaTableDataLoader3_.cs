using System.Text.RegularExpressions;

namespace LuaParserUtil.ParseTemp
{
    public class LuaTableDataLoader3_ : ILuaTableDataLoader
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
                    parser.Parse();
                    var tokens = parser.Tokens;
                    if (!TryGetKeyValue(state, out TokenKeyValue token))
                        throw new InvalidOperationException();
                }
                catch (Exception e)
                {
                    logger.Debug(e, "while parsing {table}", m.Groups["table"].Value);
                    throw;
                }
            }
        }

        private bool TryGetValue(ParsingState state, out TokenValue value)
        {
            if (TryGetConstant(state, out value) ||
                TryGetName(state, out value) ||
                TryGetString(state, out value) ||
                TryGetDictionary(state, out value))
            {
                return true;
            }
            state.Reset();
            value = TokenValue.Null;
            return false;
        }

        private bool TryGetDictionary(ParsingState state, out TokenValue dictionaryValue)
        {
            var dictionary = new TokenDictionary();
            var mark = state.Index;
            if (state.MoveIf(c => c == '{'))
            {
                state.SkipWhitespace();
                while (!state.MoveIf(c => c == '}'))
                {
                    if (TryGetComment(state, out TokenValue value))
                    {
                        continue;
                    }

                    if (TryGetKeyValue(state, out TokenKeyValue keyValue))
                    {
                        dictionary.Add(keyValue);
                    }
                    else if (TryGetName(state, out value) ||
                             TryGetConstant(state, out value) ||
                             TryGetString(state, out value) ||
                             TryGetDictionary(state, out value))
                    {
                        dictionary.Add(value);
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                    if (state.MoveIf(c => c == ','))
                        state.SkipWhitespace();
                }
            }
            state.SkipWhitespace();
            dictionaryValue = TokenValue.Null;
            return true;
        }

        private bool TryGetComment(ParsingState state, out TokenValue value)
        {
            if (state.MoveIf(c => c == '-') && state.MoveIf(c => c == '-'))
            {

                state.Mark = state.Index;
                while (state.MoveNext() && state.Current != '\r' && state.Current != '\n') ;
                value = new TokenValue(state, TokenType.Comment);
                state.SkipWhitespace();
                logger.Debug("got {Comment}", value.Value);
                return true;
            }
            state.Reset();
            value = TokenValue.Null;
            return false;
        }

        private bool TryGetConstant(ParsingState state, out TokenValue value)
        {
            if (TryGetNumber(state, out value) ||
                TryGetBoolean(state, out value) ||
                TryGetString(state, out value))
            {
                return true;
            }
            state.Reset();
            value = TokenValue.Null;
            return false;
        }

        private bool TryGetString(ParsingState state, out TokenValue value)
        {
            if (state.Current != '[' && state.CanSkipToMatchingChar)
            {
                state.SkipPastMatchingChar();
                value = new TokenValue(state, TokenType.String);
                state.SkipWhitespace();
                logger.Debug("got string {String}", value.Value);
                return true;
            }
            state.Reset();
            value = TokenValue.Null;
            return false;
        }

        private bool TryGetBoolean(ParsingState state, out TokenValue value)
        {
            if (char.IsLetter(state.Current))
            {
                while (state.MoveNext() && char.IsLetterOrDigit(state.Current)) ;
                if (bool.TryParse(state.MarkedText, out _))
                {
                    value = new TokenValue(state, TokenType.Boolean);
                    state.SkipWhitespace();
                    logger.Debug("got bool {Boolean}", value.Value);
                    return true;
                }
            }
            state.Reset();
            value = TokenValue.Null;
            return false;
        }

        private bool TryGetKeyValue(ParsingState state, out TokenKeyValue keyValue)
        {
            var mark = state.Index;
            if (TryGetKey(state, out TokenValue key))
            {
                if (state.MoveIf(c => c == '='))
                {
                    state.SkipWhitespace();
                    if (TryGetValue(state, out TokenValue value))
                    {
                        keyValue = new TokenKeyValue(state, key, value);
                        state.SkipWhitespace();
                        logger.Debug("got {key} and {value}", keyValue.Key.Value, keyValue.Value.Value);
                        return true;
                    }
                }
            }
            state.Reset(mark);
            keyValue = TokenKeyValue.Null;
            return false;
        }

        private bool TryGetKey(ParsingState state, out TokenValue value)
        {
            // Letter{Letter | Number}
            if (TryGetName(state, out value))
            {
                value.Type = TokenType.Key;
                logger.Debug("modified Name to type {TokenType}", value.Type);
                return true;
            }
            // '['Number']' | [String] 
            if (state.CanSkipToMatchingChar)
            {
                state.SkipPastMatchingChar();
                value = new TokenValue(state, TokenType.Key);
                state.SkipWhitespace();
                logger.Debug("got key {Key}", value.Value);
                return true;
            }
            state.Reset();
            value = TokenValue.Null;
            return false;
        }

        private bool TryGetName(ParsingState state, out TokenValue value)
        {
            if (char.IsLetter(state.Current))
            {
                while (state.MoveNext() && char.IsLetterOrDigit(state.Current)) ;
                value = new TokenValue(state, TokenType.Name);
                state.SkipWhitespace();
                logger.Debug("got name {Name}", value.Value);
                return true;
            }
            state.Reset();
            value = TokenValue.Null;
            return false;
        }

        private bool TryGetNumber(ParsingState state, out TokenValue value)
        {
            if (char.IsDigit(state.Next) || state.Current == '-' && char.IsDigit(state.Next))
            {
                while (state.MoveNext() && char.IsDigit(state.Current)) ;
                value = new TokenValue(state, TokenType.Number);
                state.SkipWhitespace();
                logger.Debug("got number {Number}", value.Value);
                return true;
            }
            state.Reset();
            value = TokenValue.Null;
            return false;
        }

    }
}
