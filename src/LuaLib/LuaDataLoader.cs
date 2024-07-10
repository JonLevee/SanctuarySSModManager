
using NLua;
using Sprache;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SanctuarySSModManager
{
    public class LuaDataLoader
    {
        private readonly ModManagerMetaData modManagerMetaData;

        public LuaDataLoader(ModManagerMetaData modManagerMetaData)
        {
            this.modManagerMetaData = modManagerMetaData;
        }

        internal LuaFile Load(string luaRelativePath)
        {

            var luaFile = new LuaFile(Path.Combine(modManagerMetaData.FullModRootFolder, luaRelativePath));
            var options = RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.Singleline;
            var tableRegex = new Regex(@"^\s*\b(?<table>\w+)\s*=\s*{(?<data>.*)}\s*$", options);
            var keyValuesRegex = new Regex(@"^\s*(?<key>\w+)\s*[=:]\s*(?<value>\""?[^\n]*\""?)\s*,\s*$", options);
            /* 
             * 
             * table: 
             * key/values:   ^\s*(?<key>\w+)\s*[=:]\s*(?<value>\"?[^\n]*\"?)\s*,\s*$
             */
            var tableMatches = tableRegex.Matches(luaFile.StringData.ToString()).ToList();
            foreach (var tableMatch in tableMatches)
            {
                var tableName = tableMatch.Groups["table"].Value;
                var tableData = tableMatch.Groups["data"].Value;
                var chunks = Regex.Split(tableData, @"\s*}\s*,\s*{\s*");
                foreach (var chunk in chunks)
                {

                }
            }


            return luaFile;
        }

    }

    public class LuaFile
    {
        public LuaFile(string filePath)
        {
            FilePath = filePath;
            StringData = new StringBuilder(File.ReadAllText(filePath));
        }
        public string FilePath { get; }
        public StringBuilder StringData { get; }
    }


}