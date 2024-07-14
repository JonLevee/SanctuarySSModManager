namespace SanctuarySSLib.LuaTableParsing
{
    public class LuaParsingException : Exception
    {
        public LuaParsingException(LuaParsingState state) : this(state, "Unexpected end of file")
        {
            
        }

        public LuaParsingException(LuaParsingState state, string errorMessage) : base(GetFormattedMessage(state, errorMessage))
        {

        }
        private static string GetFormattedMessage(LuaParsingState state, string errorMessage)
        {

            var start = state.index;
            while (start > 0 && !state.eolChars.Contains(state.SB[start]))
            {
                --start;
            }
            if (start > 0)
            {
                ++start;
            }
            var end = state.index;
            while (end < state.SB.Length && !state.eolChars.Contains(state.SB[end]))
            {
                ++end;
            }
            if (end < state.SB.Length)
            {
                --end;
            }
            var line = state.SB.ToString(start, end - start);
            var message = $"{errorMessage} at offset {start} in {state.FilePath} in line: {line}";
            return message;
        }
    }
}