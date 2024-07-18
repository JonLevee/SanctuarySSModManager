using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace LuaParserUtil.ParseTemp
{
    [DebuggerDisplay("{DebugLine}")]
    public class ParsingState
    {
        private readonly string[] matchingChars = ["[]", "''", "\"\""];
        public ParsingState(StringBuilder sb, Group group, string filePath)
        {
            SB = sb;
            FilePath = filePath;
            BeginIndex = group.Index;
            EndIndex = group.Index + group.Length;
            Index = BeginIndex;
        }

        public int BeginIndex { get; private set; }
        public int EndIndex { get; private set; }
        public int Mark { get; set; }
        public StringBuilder SB { get; }
        public string FilePath { get; }
        public int Index { get; private set; }
        public char Current => SB[Index];
        public char Previous => SB[Index - 1];
        public char Next => SB[Index + 1];
        public bool MoveIf(Func<char, bool> func) => func(Current) && MoveNext();

        public bool HasMore => Index < EndIndex;
        public bool MoveNext() => ++Index < EndIndex;

        public void Reset(int? mark = null) => Index = mark ?? Mark;

        public string MarkedText => SB.ToString(Mark, Index - Mark);
        public void SkipWhitespace()
        {
            while (char.IsWhiteSpace(Current) && MoveNext()) ;
            Mark = Index;
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

        private readonly char[] eolChars = ['\r', '\n'];
        public string DebugLine
        {
            get
            {
                var lineCount = Enumerable.Range(0, Index).Count(i => SB[i] == '\n');
                var tempSb = new StringBuilder($"{FilePath}:{lineCount} Begin:{BeginIndex} Index:{Index} End:{EndIndex} Line:");
                var start = Index;
                while (start > 0 && !eolChars.Contains(SB[start - 1]))
                    --start;
                while (start < SB.Length && !eolChars.Contains(SB[start]))
                {
                    if (start == Index)
                    {
                        tempSb.Append('[');
                        tempSb.Append(SB[start]);
                        tempSb.Append(']');
                    }
                    else
                    {
                        tempSb.Append(SB[start]);
                    }
                    ++start;
                }
                return tempSb.ToString();
            }
        }
    }
}
