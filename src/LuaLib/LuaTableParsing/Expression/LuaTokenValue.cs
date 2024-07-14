using System.Text;

namespace SanctuarySSLib.LuaTableParsing.Expression
{
    public class LuaTokenValue
    {
        public static readonly LuaTokenValue Empty = new LuaTokenValue(new StringBuilder(), 0, 0);
        protected readonly StringBuilder sb;
        protected readonly int index;
        protected readonly int length;
        private string? updatedValue = null;
        public bool IsDirty { get; protected set; }
        public bool IsEditable { get; protected set; }

        public LuaTokenValue(StringBuilder sb, int index, int length)
        {
            this.sb = sb;
            this.index = index;
            this.length = length;
        }
        public LuaTokenValue(LuaParsingState state) : this(state.SB, state.startIndex, state.index - state.startIndex)
        {

        }

        public virtual string Value
        {
            get => updatedValue ?? sb.ToString(index, length);
            set
            {
                if (!IsEditable)
                {
                    throw new InvalidOperationException("Not editable");
                }
                IsDirty = true;
                updatedValue = value;
            }
        }

        public override string ToString()
        {
            return Value;
        }
    }
}