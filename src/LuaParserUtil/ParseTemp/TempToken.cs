using System.Diagnostics;
using System.Text;

namespace LuaParserUtil.ParseTemp
{
    [DebuggerDisplay("[{TokenType}] '{Text}'")]
    public class TempToken
    {
        public static readonly TempToken Null = new TempToken();
        private readonly StringBuilder sb;
        private readonly int startIndex;
        public int Length { get; }

        private TempToken()
        {
            TokenType = TempTokenType.Null;
        }

        public TempToken(TempTokenType tokenType, StringBuilder sb, int startIndex, int endIndex)
        {
            TokenType = tokenType;
            this.sb = sb;
            this.startIndex = startIndex;
            Length = endIndex - startIndex; ;
        }

        public string Text => sb.ToString(startIndex, Length);

        public TempTokenType TokenType { get; }
    }
}
