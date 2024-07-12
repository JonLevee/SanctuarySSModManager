using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace SanctuarySSLib.WorkInProgressNotUsed
{
    /*
     * we need to extract tables (which are really variables) and potentially functions from the lua, but we
     * also need to keep track of values in such as way that we can allow editing of the values and then
     * be able to update the original source with minimal loss.  For example, given this variable:
     *  myvar = "foo" -- comment for myvar
     * we want to be able to replace the value without losing the comment
     * to do this, we will use a custom written recursive descent parser that keeps track of the offsets
     * of where the values are stored, so flush() will update only the original values
     */

    public class LuaScript
    {
        public LuaScript(StringBuilder stringData)
        {
            StringData = stringData;
        }

        public StringBuilder StringData { get; }
    }
    public abstract class LuaToken
    {
        protected LuaToken(string tokenType)
        {
            TokenType = tokenType;
        }

        public string TokenType { get; }
    }

    public class LuaUnaryToken : LuaToken
    {
        public LuaUnaryToken(string tokenType, string value) : base(tokenType)
        {
            Value = value;
        }

        public string Value { get; }
    }

    public class LuaBinaryToken : LuaToken
    {
        public LuaBinaryToken(string tokenType, string leftValue, string op, string rightValue) : base(tokenType)
        {
            LeftValue = leftValue;
            Op = op;
            RightValue = rightValue;
        }

        public string LeftValue { get; }
        public string Op { get; }
        public string RightValue { get; }
    }

    public class LuaDescentParser
    {
        private delegate bool TryHandleToken(string tokenName, string op, ref LuaToken? token);
        private static readonly List<TryHandleToken> tokenHandlers = [];
        private readonly char[] eolChars = "\r\n".ToCharArray();
        private readonly char[] eosChars = ";\r\n".ToCharArray();
        private readonly char[] opChars = "#=~<>+-*/%^(){}".ToCharArray();
        private readonly StringBuilder sb;

        private int index = 0;
        private int tokenStartIndex = 0;

        private char C => sb[index];
        private string TokenValue => sb.ToString(tokenStartIndex, index - tokenStartIndex);
        private bool CanContinue => index < sb.Length;
        private bool IsComment => CanContinue && C == '-' && index + 1 < sb.Length && sb[index + 1] == '-';
        private bool IsNotDelimiter => char.IsBetween(C, 'a', 'z') ||
            char.IsBetween(C, 'A', 'Z') ||
            char.IsBetween(C, '0', '9') ||
            C == '_';
        private bool IsOperator => CanContinue && opChars.Contains(C);

        public LuaDescentParser(StringBuilder sb)
        {
            this.sb = sb;
            if (!tokenHandlers.Any())
            {
                var delegates = GetType()
                    .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Select(mi => (TryHandleToken?)Delegate.CreateDelegate(typeof(TryHandleToken), this, mi, false))
                    .Where(d => d != null)
                    .Cast<TryHandleToken>()
                    .ToList();
                tokenHandlers.AddRange(delegates);
            }
        }

        private void AdvanceToEOL()
        {
            while (index < sb.Length && !eolChars.Contains(C))
            {
                ++index;
            }
        }
        public IEnumerable<LuaToken> Parse()
        {
            while (CanContinue)
            {
                // advance past whitespace
                while (CanContinue && char.IsWhiteSpace(C))
                {
                    ++index;
                }
                tokenStartIndex = index;
                // comments are special case
                if (IsComment)
                {
                    index += 2;
                    tokenStartIndex = index;
                    AdvanceToEOL();
                    yield return new LuaUnaryToken("Comment", TokenValue);
                    continue;
                }

                while (CanContinue && IsNotDelimiter)
                {
                    ++index;
                }
                var tokenName = TokenValue;
                tokenStartIndex = index;
                while (CanContinue && IsOperator)
                {
                    ++index;
                }

                var op = TokenValue;
                tokenStartIndex = index;
                if (op == string.Empty)
                {
                    throw new InvalidOperationException();
                }

                LuaToken? token = null;
                if (!tokenHandlers.Any(th => th(tokenName, op, ref token)))
                    throw new InvalidOperationException($"No token handler found for '{tokenName}'");
                if (null != token)
                    yield return token;
            }
        }

        private bool TryGetKeyword(string tokenName, string op, ref LuaToken? token)
        {
            // this handler is catch all for unhandled keywords, if we have one we need to implement handler
            string[] keywords = [
                "and",
                "break",
                "do",
                "else",
                "elseif",
                "end",
                "false",
                "for",
                "function",
                "if",
                "in",
                "local",
                "nil",
                "not",
                "or",
                "repeat",
                "return",
                "then",
                "true",
                "until",
                "while",
                ];
            token = null;

            if (!keywords.Contains(tokenName))
                return false;

            throw new NotImplementedException($"handler for keyword {tokenName} is not implemented");
        }

        private bool TryGetVariable(string tokenName, string op, ref LuaToken? token)
        {
            // this handler is catch all, if we get to this point it's a variable

            token = null;
            if (tokenName != "--")
                return false;


            return true;
        }
    }
}