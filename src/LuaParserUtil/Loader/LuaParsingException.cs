using System.Runtime.CompilerServices;

namespace LuaParserUtil.Loader
{
    public class LuaParsingException : Exception
    {
        public LuaParsingException(
            string message,
            [CallerMemberName]
            string caller = null) : base($"{caller}: {message}")
        {

        }
    }
}
