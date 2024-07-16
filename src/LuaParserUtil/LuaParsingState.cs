using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace LuaParserUtil
{
    public class LuaParsingState : IEnumerator<char>
    {
        private readonly string[] matchingChars = ["[]", "''", "\"\""];
        public LuaParsingState(StringBuilder sb, Group group)
        {
            SB = sb;
            BeginIndex = group.Index;
            EndIndex = group.Index + group.Length;
            Index = BeginIndex;
        }

        public int BeginIndex { get; private set; }
        public int EndIndex { get; private set; }
        public StringBuilder SB { get; }
        public int Index { get; private set; }
        public char Current => SB[Index];
        public char Previous => SB[Index-1];
        public char Next => SB[Index + 1];

        object IEnumerator.Current => SB[Index];

        public bool MoveIf(char c)
        {
            if (Current == c) { 
                MoveNext();
                return true;
            }
            return false;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            return ++Index < EndIndex;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
        public void Reset(int index)
        {
            Index = index;
        }

        public void SkipWhitespace()
        {
            while(char.IsWhiteSpace(Current) && MoveNext());
        }

        public bool CanSkipToMatchingChar => matchingChars.Any(p => p[0] == Current);

        public void SkipPastMatchingChar()
        {
            var match = matchingChars.Single(P => P[0] == Current)[1];
            while (MoveNext()) 
            {
                if (Current == match && Previous != '\\')
                {
                    MoveNext();
                    return;
                }
            }
        }
    }

}
