using Microsoft.Extensions.DependencyInjection;
using SanctuarySSLib.Attributes;
using SanctuarySSLib.Enums;
using SanctuarySSLib.LuaUtil;
using SanctuarySSLib.MiscUtil;
using SanctuarySSModManager;
using SanctuarySSModManager.Extensions;
using System.Text.Json.Nodes;

namespace SanctuarySSLib.Models
{
    [SingletonService]
    public class ShatteredSunModel
    {
        private readonly LuaTableToJsonLoader loader;

#pragma warning disable CS8603, CS8602 // Possible null reference return. Dereference of a possibly null reference.
        public JsonArray Factions => loader.Root["FactionsData"].AsArray();
        public JsonObject AvailableUnits => loader.Root["AvailableUnits"].AsObject();
        public JsonObject Units => loader.Root["UnitTemplate"].AsObject();

        public bool IsUnitEnabled(string unitId) => ((bool?)AvailableUnits[unitId]) ?? false;

        public string GetJson() => loader.ToString();

        public ShatteredSunModel(LuaTableToJsonLoader loader)
        {
            this.loader = loader;
        }

        public async Task Load()
        {
            loader.Load("common/systems/factions.lua", "FactionsData");
            loader.Load("common/units/availableUnits.lua", "AvailableUnits");
            var gameMetadata = DIContainer.Get<IGameMetadata>();
            var rootPath = gameMetadata.GetFullPath("common/units/unitsTemplates");
            foreach (var file in Directory.GetFiles(rootPath, "*.santp", SearchOption.AllDirectories))
            {
                var relativePath = file.Substring(gameMetadata.LuaPath.Length + 1);
                loader.Load(relativePath, "UnitTemplate", node => node["general"]["tpId"].ToString());
            }
            // 
            var root = loader.Root;
            File.WriteAllText("root.json", loader.ToString());
            await Task.CompletedTask;
        }
    }
#pragma warning restore CS8603, CS8602
}
