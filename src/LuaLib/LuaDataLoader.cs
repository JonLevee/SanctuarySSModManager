
using System.Text;

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
            int index = 0;
            if (TrySkipWhitespace(luaFile.StringData, ref index))
            {
                var start = index;
                while(TryGetNextToken(luaFile.StringData, ref index, out string token))
                {

                }
            }
            return luaFile;
        }

        private bool TryGetNextToken(StringBuilder sb, ref int index, out string token)
        {
            static delims = 
            while (index < sb.Length && char.IsWhiteSpace(sb[index]))
            {
                index++;
            }
        }

        private bool TrySkipWhitespace(StringBuilder sb, ref int index)
        {
            while(index < sb.Length && char.IsWhiteSpace(sb[index]))
            {
                ++index;
            }
            return index < sb.Length;
        }
    }

    public class  LuaFile
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