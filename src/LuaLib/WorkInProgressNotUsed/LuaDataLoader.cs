
using NLua;
using SanctuarySSModManager;
using Sprache;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SanctuarySSLib.WorkInProgressNotUsed
{
#if false
    public class LuaDataLoader
    {
        private readonly ModManagerMetaData modManagerMetaData;

        public LuaDataLoader(ModManagerMetaData modManagerMetaData)
        {
            this.modManagerMetaData = modManagerMetaData;
        }

        public void LoadAll(string? luaRelativePath = null)
        {
            var rootPath = luaRelativePath == null
                ? modManagerMetaData.FullModRootFolder
                : Path.Combine(modManagerMetaData.FullModRootFolder, luaRelativePath.ToString());
            var luaFiles = Directory.GetFiles(rootPath, "*.lua", SearchOption.AllDirectories);
            foreach (var luaFile in luaFiles)
            {
                Load(luaFile);
            }
        }
        public LuaFile Load(string luaPath)
        {

            var luaFile = new LuaFile(luaPath);

            var parser = new LuaDescentParser(luaFile.StringData);
            var result = parser.Parse();
            /* 
            var options = RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.Singleline;
            var tableRegex = new Regex(@"^\s*\b(?<table>\w+)\s*=\s*{(?<data>.*)}\s*$", options);
            var keyValuesRegex = new Regex(@"^\s*(?<key>\w+)\s*[=:]\s*(?<value>\""?[^\n]*\""?)\s*,\s*$", options);
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
             * 
             * table: 
             * key/values:   ^\s*(?<key>\w+)\s*[=:]\s*(?<value>\"?[^\n]*\"?)\s*,\s*$
             */


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
#endif
}