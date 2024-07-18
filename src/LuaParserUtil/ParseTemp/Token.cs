using System.Text;

namespace LuaParserUtil.ParseTemp
{
    public class Token
    {
        public readonly static Token Null = new Token();
        protected readonly StringBuilder sb;

        protected Token() { }

        protected Token(ParsingState state)
        {
            sb = state.SB;
            StartIndex = state.Mark;
            EndIndex = state.Index;
        }

        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int Length => EndIndex - StartIndex;
    }
}
