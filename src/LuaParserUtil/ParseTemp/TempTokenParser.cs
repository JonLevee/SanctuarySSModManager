using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace LuaParserUtil.ParseTemp
{

    [DebuggerDisplay("{DebugLine}")]
    public class TempTokenParser
    {
        private readonly StringBuilder sb;
        private int index;
        private int startIndex;
        private int endIndex;
        private readonly char[] eolChars = ['\r', '\n'];
        private readonly char[] quoteChars = ['"', '\''];
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        Dictionary<char, SingeCharTempTokenType> singleCharTokens = ((SingeCharTempTokenType[])[
            new SingeCharTempTokenType('=', TempTokenType.Assignment),
            new SingeCharTempTokenType('{', TempTokenType.OpenBrace),
            new SingeCharTempTokenType('}', TempTokenType.CloseBrace),
            new SingeCharTempTokenType(',', TempTokenType.Separator),
            new SingeCharTempTokenType('.', TempTokenType.Property)
            {
                MustFollow = [TempTokenType.Name, TempTokenType.ParamsEnd, TempTokenType.Variable]
            },
            new SingeCharTempTokenType('(', TempTokenType.ParamsStart),
            new SingeCharTempTokenType(')', TempTokenType.ParamsEnd),
            ]).ToDictionary(c => c.C);

        public TempTokenParser(StringBuilder sb, string filePath)
        {
            this.sb = sb;
            FilePath = filePath;
            this.Tokens = new Queue<TempToken>();
            index = 0;
            startIndex = 0;
            endIndex = 0;
        }
        public string FilePath { get; }
        public Queue<TempToken> Tokens { get; }


        public void Parse(Match m = null)
        {
            Tokens.Clear();
            index = m != null ? m.Index : 0;
            endIndex = m != null ? m.Index + m.Length : sb.Length;

            try
            {
                while (TrySkipWhitespace())
                {
                    startIndex = index;
                    if (Is("--"))
                    {
                        AdvanceUntil(eolChars.Contains);
                        EnqueueToken(TempTokenType.Comment);
                        continue;
                    }
                    if (Is(".."))
                    {
                        AdvanceUntil(eolChars.Contains);
                        EnqueueToken(TempTokenType.Concat);
                        continue;
                    }
                    if (TestChar(quoteChars.Contains))
                    {
                        var quote = Current;
                        ++index;
                        AdvanceUntil(c => c == quote && Previous != '\\');
                        ++index;
                        EnqueueToken(TempTokenType.String);
                        continue;
                    }
                    // don't need to handle function definitions ...
                    if (Is("function "))
                    {
                        // still need to eat it up untile we find ^end$
                        AdvanceUntil(c => TestChar(eolChars.Contains) && sb.ToString(index - 3, 3) == "end" && TestChar(eolChars.Contains, 4));
                        EnqueueToken(TempTokenType.FunctionDefinition);
                        continue;

                    }
                    if (TestChar(IsValidVarNameChar))
                    {
                        AdvanceWhile(IsValidVarNameChar);
                        if (CurrentToken.All(char.IsDigit))
                            EnqueueToken(TempTokenType.Number);
                        else if (CurrentToken.All(char.IsLetter))
                            EnqueueToken(TempTokenType.Name);
                        else
                            EnqueueToken(TempTokenType.Variable);
                        continue;
                    }
                    if (singleCharTokens.TryGetValue(Current, out SingeCharTempTokenType item))
                    {
                        item.Validate(LastQueuedToken);
                        ++index;
                        EnqueueToken(item.TokenType);
                        continue;
                    }

                    throw new NotImplementedException($"Don't know how to handle token '{Current}' in {DebugLine}");
                }

            }
            catch (Exception e)
            {
                logger.Error(e);
                throw;
            }
        }

        private bool TrySkipWhitespace()
        {
            while (!EOF && char.IsWhiteSpace(Current))
                ++index;
            return !EOF;
        }

        private char Current => sb[index];
        private char Previous => sb[index - 1];
        private string CurrentToken => sb.ToString(startIndex, index - startIndex);

        private void AdvanceWhile(Func<char, bool> condition)
        {
            while (!EOF && condition(Current))
                ++index;
        }
        private void AdvanceUntil(Func<char, bool> condition)
        {
            while (!EOF && !condition(Current))
                ++index;
        }
        private bool TryCompare(char c)
        {
            if (Current == c)
            {
                ++index;
                return true;
            }
            return false;
        }

        private bool Is(char c, int offset = 0) => index + offset < endIndex && sb[index + offset] == c;
        private bool TestChar(Func<char, bool> getC, int offset = 0) => index + offset < endIndex && getC(sb[index + offset]);
        private bool Is(string chars) => Enumerable.Range(0, chars.Length).All(i => chars[i] == sb[index + i]);
        private bool IsValidVarNameChar(char c) => char.IsLetterOrDigit(c) || c == '_';
        private bool EOF => index >= endIndex;


        private TempToken LastQueuedToken;
        private void EnqueueToken(TempTokenType tokenType)
        {
            LastQueuedToken = new TempToken(tokenType, sb, startIndex, index);
            logger.Debug("got {TokenType} {Text}", LastQueuedToken.TokenType, LastQueuedToken.Text);
            Tokens.Enqueue(LastQueuedToken);
        }

        public string DebugLine
        {
            get
            {
                var lineCount = Enumerable.Range(0, index).Count(i => sb[i] == '\n');
                var tempSb = new StringBuilder($"{FilePath}:{lineCount} Index:{index} End:{endIndex} Line:");
                var start = index;
                while (start > 0 && !eolChars.Contains(sb[start - 1]))
                    --start;
                while (start < endIndex && !eolChars.Contains(sb[start]))
                {
                    if (start == index)
                    {
                        tempSb.Append('[');
                        tempSb.Append(sb[start]);
                        tempSb.Append(']');
                    }
                    else
                    {
                        tempSb.Append(sb[start]);
                    }
                    ++start;
                }
                return tempSb.ToString();
            }
        }

        private class SingeCharTempTokenType
        {
            public SingeCharTempTokenType(char c, TempTokenType tokenType)
            {
                C = c;
                TokenType = tokenType;
                Validate = DefaultValidation;
                MustFollow = [];
            }

            public char C { get; }
            public TempTokenType TokenType { get; }
            public TempTokenType[] MustFollow { get; set; }
            public Action<TempToken> Validate { get; set; }

            private void DefaultValidation(TempToken previous)
            {
                if (MustFollow.Any() && !MustFollow.Contains(previous.TokenType))
                {
                    throw new InvalidOperationException($"TokenType {TokenType} preceding token type of {previous.TokenType} must be one of {string.Join(',', MustFollow)}");
                }
            }
        }
    }
}
