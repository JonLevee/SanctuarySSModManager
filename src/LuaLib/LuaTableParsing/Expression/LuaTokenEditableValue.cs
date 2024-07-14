using System.Text;

namespace SanctuarySSLib.LuaTableParsing.Expression
{
    public class LuaTokenEditableValue : LuaTokenValue
    {

        public LuaTokenEditableValue(StringBuilder sb, int index, int length) : base(sb, index, length)
        {
            IsEditable = true;
        }
    }
}