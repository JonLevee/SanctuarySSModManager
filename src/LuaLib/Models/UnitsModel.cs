using NLua;
using SanctuarySSLib.Attributes;
using SanctuarySSLib.LuaUtil;
using SanctuarySSLib.MiscUtil;
using System;
using System.Text;

namespace SanctuarySSLib.Models
{
    [SingletonService]
    public class UnitsModel : Dictionary<string, LuaTable>
    {
        public void Load(IGameMetadata gameMetadata, LuaValueLoader luaValueLoader)
        {
            var luaUnitRoot = gameMetadata.GetFullPath(RelativePath);
            foreach (var luaFile in Directory.GetFiles(luaUnitRoot, "*.santp", SearchOption.AllDirectories))
            {
                var luaFileName = Path.GetFileNameWithoutExtension(luaFile);
                var luaParentName = Path.GetFileName(Path.GetDirectoryName(luaFile));
                if (string.Compare(luaFileName, luaParentName) != 0)
                {
                    throw new InvalidOperationException($"lua file {luaFileName} != parent {luaParentName}");
                }
                using (Lua lua = new Lua())
                {
                    lua.State.Encoding = Encoding.UTF8;
                    lua.DoFile(luaFile);
                    var table = (LuaTable)lua[TableName];
                    Add(luaFileName, table);
                }

            }
        }

        public string RelativePath => "common/units/unitsTemplates";

        public string TableName => "UnitTemplate";

    }
}
