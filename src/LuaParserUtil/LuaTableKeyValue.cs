using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace LuaParserUtil
{
    [DebuggerDisplay("Key: [{Key}] Value: [{Value}]")]
    public class LuaTableKeyValue
    {
        private readonly StringBuilder sb;
        private readonly int offset;
        private readonly int length;
        private string? updated;

        public LuaTableKeyValue(StringBuilder sb, string key, Group group)
        {
            this.sb = sb;
            Key = key;
            offset = group.Index;
            length = group.Length;
        }

        public bool IsDirty { get; private set; }
        public string Key { get; private set; }
        public string Value
        {
            get => updated ?? sb.ToString(offset, length);
            set
            {
                updated = value;
                IsDirty = true;
            }
        }

        public override string ToString() => Value;
    }
}