using Microsoft.Extensions.DependencyInjection;
using SanctuarySSLib.LuaUtil;
using SanctuarySSLib.MiscUtil;
using SanctuarySSModManager;

namespace SanctuarySSLib.Models
{
    public class FactionCollectionModel : Dictionary<string, FactionModel>
    {
        public string RelativePath => @"common\systems\factions.lua";
        public string FullPath { get; }
        public string TableName => "FactionsData";

        public FactionCollectionModel()
        {
            var gameMetadata = DIContainer.Services.GetService<IGameMetadata>();
            FullPath = gameMetadata.GetFullPath(RelativePath);
        }
        public void Load()
        {
            var loader = DIContainer.Services.GetService<LuaValueLoader>();
            var factionData = loader.GetTableFromFile(RelativePath, TableName);
            throw new NotImplementedException();
            //foreach (LuaDictionary table in factionData.ArrayItems)
            //{
            //    var faction = new FactionModel(table);
            //    Add(faction.Key, faction);
            //}
        }
    }

    public class FactionModel
    {
        public string KeyField => "name";

    }
}
