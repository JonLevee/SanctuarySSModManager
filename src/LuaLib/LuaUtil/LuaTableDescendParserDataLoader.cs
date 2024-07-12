using SanctuarySSLib.Models;
using SanctuarySSModManager.Extensions;
using System.Diagnostics;
using System.Text;

namespace SanctuarySSLib.LuaUtil
{
    [DebuggerDisplay("Op: [{Op}]")]
    public abstract class LuaTokenExpression
    {
        public class LuaTokenExpressionEmpty : LuaTokenExpression
        {
            public LuaTokenExpressionEmpty() : base(string.Empty)
            {

            }
        }

        public static LuaTokenExpression Empty = new LuaTokenExpressionEmpty();
        protected LuaTokenExpression(string op)
        {
            Op = op;
        }
        protected LuaTokenExpression(StringBuilder op)
        {
            Op = op.ToString();
        }
        public string Op { get; }

    }

    [DebuggerDisplay("Op: [{Op}] Value: {Value.Value}")]
    public class LuaTokenUnaryExpression : LuaTokenExpression
    {
        public LuaTokenUnaryExpression(string op) : base(op)
        {
            Value = LuaTokenValue.Empty;
        }
        public LuaTokenUnaryExpression(StringBuilder op) : base(op)
        {
            Value = LuaTokenValue.Empty;
        }
        public LuaTokenValue Value { get; set; }

    }

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
    public class LuaTokenEditableValue : LuaTokenValue
    {

        public LuaTokenEditableValue(StringBuilder sb, int index, int length) : base(sb, index, length)
        {
            IsEditable = true;
        }
    }

    public class LuaTableDescendParserDataLoader : ILuaTableDataLoader
    {
        private int startIndex = -1;
        private int index = -1;
        private StringBuilder fileData = new StringBuilder();
        private StringBuilder token = new StringBuilder();
        private StringBuilder op = new StringBuilder();
        private List<LuaTokenExpression> expressions = new List<LuaTokenExpression>();
        private readonly char[] eolChars = ['\r', '\n'];
        private readonly string[] unaryTokens = ["not"];

        private readonly Dictionary<string, string> operatorHandlers = new Dictionary<string, string>
        {
            { "=", string.Empty },
        };
        private readonly char[] operators;

        public LuaTableDescendParserDataLoader()
        {
            operators = string.Concat(operatorHandlers.Keys).ToCharArray().Distinct().ToArray();
        }
        private bool HasMore => index < fileData.Length;
        private char CurrentChar => fileData[index];
        private char NextChar => fileData[index + 1];
        private string CurrentToken => fileData.ToString(startIndex, index - startIndex);
        private bool IsNonDeliminator => char.IsBetween(CurrentChar, 'a', 'z') ||
            char.IsBetween(CurrentChar, 'A', 'Z') ||
            char.IsBetween(CurrentChar, '0', '9') ||
            CurrentChar == '_';
        private bool IsOperator => operators.Contains(CurrentChar);
        private bool IsCurrentTokenUnary => unaryTokens.Contains(CurrentToken);

        public IEnumerable<string> GetUnsupportedTableNames()
        {
            yield break;
        }


        public void Load(LuaTableData tableData)
        {
            index = startIndex = 0;
            fileData = tableData.FileData;
            while (TryGetLuaExpression(out LuaTokenExpression expression))
            {
                expressions.Add(expression);
            }
            throw new NotImplementedException();
        }

        private bool TrySkipWhitespace()
        {
            while (HasMore && char.IsWhiteSpace(CurrentChar))
            {
                ++index;
            }
            return HasMore;
        }

        private bool TryGetLuaExpression(out LuaTokenExpression expression)
        {
            expression = LuaTokenExpression.Empty;
            token.Clear();
            op.Clear();

            Constraint.NotNull(fileData);
            // advance past any whitespace
            if (!TrySkipWhitespace())
            {
                return false;
            }

            startIndex = index;

            // comment is special case
            if (CurrentChar == '-' && NextChar == '-')
            {
                op.Append(CurrentChar);
                op.Append(NextChar);
                startIndex += 2;
                while (HasMore && !eolChars.Contains(CurrentChar))
                {
                    ++index;
                }
                expression = new LuaTokenUnaryExpression(op.ToString())
                {
                    Value = new LuaTokenValue(fileData, startIndex, index - startIndex)
                };
                return true;
            }

            if (!TrySkipToToken())
            {
                return false;
            }

            var leftStart = startIndex;
            var leftLength = index - startIndex;
            //var leftToken = new LuaTokenValue(fileData, startIndex, index - startIndex);
            startIndex = index;

            if (!TrySkipOperator())
            {
                return false;
            }
            //if (IsCurrentTokenUnary)
            //{
            //    expression = new LuaTokenUnaryExpression(CurrentToken)
            //    {
            //        Value = new LuaTokenValue(fileData, startIndex, index - startIndex)
            //    };

            //    throw new InvalidOperationException("don't know what to do here");
            //}
            if (operatorHandlers.ContainsKey(CurrentToken))
            {

            }
            if (!TryGetLuaExpression(out LuaTokenExpression rightSide))
            {
                throw new InvalidOperationException("don't know what to do here");
            }
            return false;
        }

        private bool TrySkipOperator()
        {
            if (TrySkipWhitespace())
            {
                while (HasMore && IsOperator)
                {
                    ++index;
                }
            }
            return HasMore;
        }

        private bool TrySkipToToken()
        {
            if (TrySkipWhitespace())
            {
                while (HasMore && IsNonDeliminator)
                {
                    ++index;
                }
            }
            return HasMore;
        }


    }
}