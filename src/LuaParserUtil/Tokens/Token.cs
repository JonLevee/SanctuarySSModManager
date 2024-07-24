using System.Data.SqlTypes;
using System;
using System.Diagnostics;
using System.Text;

namespace LuaParserUtil.Tokens
{
    [DebuggerDisplay("{Description}")]
    public class Token
    {
        public static readonly Token Null = new Token();
        private readonly StringBuilder sb;
        private readonly int startIndex;
        private readonly char[] eolChars = ['\r', '\n'];
        public int Length { get; }

        private Token()
        {
            TokenType = TokenType.Null;
            sb = new StringBuilder("(null)");
            startIndex = 0;
            Length = sb.Length;
        }

        public Token(TokenType tokenType, StringBuilder sb, int startIndex, int endIndex)
        {
            TokenType = tokenType;
            this.sb = sb;
            this.startIndex = startIndex;
            Length = endIndex - startIndex; ;
        }

        public string Text => sb.ToString(startIndex, Length);

        public TokenType TokenType { get; }

        public string Description => $"[{TokenType}] '{Text}'";
        public string DebugLine => GetDebugLine(null);
        public string GetDebugLine(string filePath)
        {
            var index = startIndex;
            var endIndex = sb.Length;
            var lineCount = Enumerable.Range(0, index).Count(i => sb[i] == '\n');
            var tempSb = new StringBuilder();
            if (!string.IsNullOrEmpty(filePath))
            {
                tempSb.Append($"{filePath}:");
            }
            tempSb.Append($"{lineCount} Index:{index} Line:");
            var start = index;
            while (start > 0 && !eolChars.Contains(sb[start - 1]))
                --start;
            while (start < endIndex && !eolChars.Contains(sb[start]))
            {
                if (start == index)
                {
                    tempSb.Append('[');
                    tempSb.Append(sb[start]);
                    tempSb.Append(']');
                }
                else
                {
                    tempSb.Append(sb[start]);
                }
                ++start;
            }
            return tempSb.ToString();
        }

    }
}
