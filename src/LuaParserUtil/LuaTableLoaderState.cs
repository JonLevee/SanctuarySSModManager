using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace LuaParserUtil
{
    [DebuggerDisplay("C='{C.ToString()}' Line={Line}")]
    public class LuaTableLoaderState
    {
        public StringBuilder SB { get; private set; }
        public int Index { get; set; }
        private Group matchGroup;
        private readonly char[] eolChars = ['\r', '\n'];

        public LuaTableLoaderState(StringBuilder sb)
        {
            SB = sb;
        }

        public char C => SB[Index];
        public char PeekC => SB[Index + 1];
        public char PreviousC => SB[Index - 1];

        public bool EndOfMatch => Index >= SB.Length || Index >= matchGroup.Index + matchGroup.Length;

        public string Line
        {
            get
            {
                var start = Index;
                while (start > 0 && !eolChars.Contains(SB[start - 1]))
                    --start;
                var end = Index;
                while (end < SB.Length && !eolChars.Contains(SB[end]))
                    ++end;
                var tempSb = new StringBuilder();
                tempSb.Append(SB.ToString(start, Index - start));
                tempSb.Append('[');
                tempSb.Append(C);
                tempSb.Append(']');
                tempSb.Append(SB.ToString(Index + 1, end - Index - 1));
                return tempSb.ToString();
            }
        }


        public void SetMatch(Match match)
        {
            matchGroup = match;
            Index = matchGroup.Index;
        }

        public void SkipWhitespace()
        {
            while (!EndOfMatch && char.IsWhiteSpace(C))
            {
                ++Index;
            }
        }

        public bool TrySkipComment()
        {
            if (C == '-' & PeekC == '-')
            {
                while (!EndOfMatch && !eolChars.Contains(C))
                {
                    ++Index;
                }
                return true;
            }
            return false;
        }


        public bool TryGetNamedToken(out LuaString name)
        {
            SkipWhitespace();
            var start = Index;
            while (char.IsLetterOrDigit(C))
                ++Index;
            name = new LuaString(SB, start, Index);
            return name.Length > 0;
        }

        public void SkipPastAssignmentOperator()
        {
            SkipWhitespace();
            if (C != '=')
                throw new InvalidOperationException($"got '{C}' but expected '='");
            ++Index;
        }
    }


}
