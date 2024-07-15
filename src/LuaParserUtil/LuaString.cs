using System.Text;

namespace LuaParserUtil
{
    public class LuaString
    {
        public static LuaString Empty = new LuaString(new StringBuilder(), 0, 0);
        private readonly StringBuilder sb;
        private readonly int start;
        private readonly int end;

        public LuaString(StringBuilder sb, int start, int end)
        {
            this.sb = sb;
            this.start = start;
            this.end = end;
        }

        public int Length => end - start;

        public static implicit operator string(LuaString s) => s.ToString();
        public override string ToString()
        {
            return sb.ToString(start, Length);
        }
    }
}
