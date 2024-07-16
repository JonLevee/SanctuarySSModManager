
using System.Text;
using System.Text.RegularExpressions;

namespace LuaParserUtil
{
    public class LuaTableDataLoader2 : ILuaTableDataLoader
    {
        private const RegexOptions regexOptions = RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline;
        private readonly Regex tableRegex = new Regex(@"^(?<table>[a-zA-Z0-9-]+)\s*=\s*(?<data>{\s*.+^})", regexOptions);

        public IEnumerable<string> GetUnsupportedTableNames()
        {
            yield break;
        }

        public void Load(LuaTableData tableData)
        {
            var lex = new Dictionary<Type, Func<object>>
            {
                { typeof(LuaTokenVariableName), ()=>{} }
            };
            foreach (Match m in tableRegex.Matches(tableData.FileData.ToString()))
            {
                var state = new LuaParsingState(tableData.FileData);
            }
        }
    }

    public class LuaParsingState
    {
        public LuaParsingState(StringBuilder sb)
        {
            SB = sb;
        }

        public StringBuilder SB { get; }
        public int Index { get; set; }
    }
    public abstract class LuaParsingToken
    {
        protected LuaParsingToken(LuaParsingState state, int index, int length)
        {
            State = state;
            Index = index;
            Length = length;
        }

        public LuaParsingState State { get; }
        public int Index { get; }
        public int Length { get; }

        public static implicit operator string(LuaParsingToken s) => s.ToString();
        public override string ToString()
        {
            return State.SB.ToString(Index, Index + Length);
        }

    }

    public class LuaTokenVariableName : LuaParsingToken
    {
        public static bool
        public static bool TryParse(LuaParsingState state, out LuaTokenVariableName token)
        {
            var start = state.Index;
            while (char.IsLetterOrDigit(state.SB[start]))
                ++start;
            token = new LuaTokenVariableName(state, start, state.Index);
            return true;
        }

        public LuaTokenVariableName(LuaParsingState state, int index, int length) : base(state, index, length)
        {
        }
    }

    public class LuaTokenAssignmentOperator : LuaParsingToken
    {
        public LuaTokenAssignmentOperator
    }
}
