using System;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Text;

namespace SanctuarySSLib.LuaTableParsing
{
    [DebuggerDisplay("index: {index} CurrentChar: {CurrentChar} CurrentToken: {CurrentToken}")]
    public class LuaParsingState
    {
        public static readonly LuaParsingState Empty = new LuaParsingState(string.Empty, new StringBuilder());
        private readonly char[] eolChars = ['\r', '\n'];
        public string FilePath { get; }

        public LuaParsingState(string filePath, StringBuilder stringData)
        {
            FilePath = filePath;
            SB = stringData;
        }

        public StringBuilder SB { get; }

        public int index { get; set; }
        public int startIndex { get; set; }

        public bool HasMore => index < SB.Length;
        public char CurrentChar => SB[index];
        public char NextChar => SB[index + 1];
        public string CurrentToken => SB.ToString(startIndex, index - startIndex);
        public bool IsNonDeliminator => char.IsBetween(CurrentChar, 'a', 'z') ||
            char.IsBetween(CurrentChar, 'A', 'Z') ||
            char.IsBetween(CurrentChar, '0', '9') ||
            CurrentChar == '_';

        public void AdvanceToEOL(int length = int.MaxValue)
        {
            while (HasMore && !eolChars.Contains(CurrentChar) && --length >= 0)
            {
                ++index;
            }
        }

        public bool TrySkipWhitespace()
        {
            while (HasMore && char.IsWhiteSpace(CurrentChar))
            {
                ++index;
            }
            startIndex = index;
            return HasMore;
        }
        public bool TryGetTokenName(out string name)
        {
            name = string.Empty;
            if (TrySkipWhitespace())
            {
                while (HasMore && IsNonDeliminator)
                {
                    ++index;
                }
                name = CurrentToken;
                startIndex = index;
            }
            return HasMore;
        }
        public bool TryGetOpName(LuaParsingTokenHandlers tokenHandlers, out string name)
        {
            name = string.Empty;
            if (TrySkipWhitespace())
            {
                while (HasMore && tokenHandlers.IsOperator(CurrentChar))
                {
                    ++index;
                }
                name = CurrentToken;
                startIndex = index;
                if (string.IsNullOrWhiteSpace(name))
                {
                    GetCurrentLine(out string line, out int start);
                    var message = $"Invalid operator at offset {start} in {FilePath} in line: {line}";
                    throw new InvalidOperationException(message);
                }
            }
            return HasMore;
        }

        public void GetCurrentLine(out string line, out int start)
        {
            start = index;
            while (start > 0 && !eolChars.Contains(SB[start]))
            {
                --start;
            }
            if (start > 0)
            {
                ++start;
            }
            var end = index;
            while (end < SB.Length && !eolChars.Contains(SB[end]))
            {
                ++end;
            }
            if (end < SB.Length)
            {
                --end;
            }
            line = SB.ToString(start, end - start);
        }
    }
}