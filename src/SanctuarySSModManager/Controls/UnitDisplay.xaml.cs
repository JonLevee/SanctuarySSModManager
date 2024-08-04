using SanctuarySSLib.LuaUtil;
using SanctuarySSModManager.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Xceed.Wpf.Toolkit.Panels;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using static System.Net.Mime.MediaTypeNames;

namespace SanctuarySSModManager.Controls
{
    [DebuggerDisplay("{TpId.Text} {Faction.Text} {Tier.Text}")]
    /// <summary>
    /// Interaction logic for UnitDisplay.xaml
    /// </summary>
    public partial class UnitDisplay : UserControl
    {
        private readonly IGameMetadata gameMetadata;
        private readonly JsonObject data;

        public UnitDisplayNumber HarvestTime => new("Economy", "Harvest time", "economy/harvestTime");
        public UnitDisplayNumber EconomyAlloys => new("Economy", "Alloys", "economy/harvest/alloys");
        public UnitDisplayNumber EconomyEnergy => new("Economy", "Energy", "economy/harvest/energy");
        public UnitDisplayNumber VisionRadius => new("Intel", "Vision radius", "intel/visionRadius");
        public UnitDisplayNumber Acceleration => new("Movement", "Acceleration", "movement/acceleration");
        public UnitDisplayNumber RotationSpeed => new("Movement", "Rotation speed", "movement/rotationSpeed");
        public UnitDisplayBoolean AirHover => new("Movement", "Air hover", "movement/airHover");
        public UnitDisplayNumber MinSpeed => new("Movement", "Min speed", "movement/minSpeed");
        public UnitDisplayNumber Speed => new("Movement", "Speed", "movement/speed");
        public UnitDisplayText MovementType => new("Movement", "Type", "movement/type");
        public UnitDisplayNumber Damage => new("Weapons", "Damage", "weapons/damage");
        public UnitDisplayNumber DamageRadius => new("Weapons", "Damage radius", "weapons/damageRadius");
        public UnitDisplayText DamageType => new("Weapons", "Damage type", "weapons/damageType");
        public UnitDisplayText RangeRingType => new("Weapons", "Range ring type", "weapons/rangeRingType");
        public UnitDisplayNumber RangeMin => new("Weapons", "Range min", "weapons/rangeMin");
        public UnitDisplayNumber RangeMax => new("Weapons", "Range max", "weapons/rangeMax");
        public UnitDisplayNumber ReloadTime => new("Weapons", "Reload time", "weapons/reloadTime");
        public UnitDisplayOrders Orders => new("Orders", "", "general/orders");

        // engine\LJ\lua\common\systems\templateUpdater.lua: unitToProjectile
        public UnitDisplay(IGameMetadata gameMetadata, JsonObject data)
        {
            InitializeComponent();

            this.gameMetadata = gameMetadata;
            this.data = data;
            Load();
        }

        private void Load()
        {
            var instances =
                GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(p => p.FieldType.IsAssignableTo(typeof(UnitDisplayText)))
                .Select(p => p.GetValue(this))
                .Cast<UnitDisplayText>()
                .Concat(
                    GetType()
                    .GetProperties()
                    .Where(p => p.PropertyType.IsAssignableTo(typeof(UnitDisplayText)))
                    .Select(p => p.GetValue(this))
                    .Cast<UnitDisplayText>()
                ).ToList();
            var previousGroup = UnitDisplayText.Empty;
            foreach (var instance in instances)
            {
                Debug.Assert(instance != null);
                if (instance.Initialize(Grid, data, previousGroup))
                {
                    if (instance.AddToGrid)
                        Grid.RowDefinitions.Add(new RowDefinition());
                };
                previousGroup = instance;
            }
            var tpId = TpId.Text;
            var imagePath = Path.Combine(gameMetadata.GameRoot, @"engine\Sanctuary_Data\Resources\UI\Gameplay\IconsUnits", tpId + ".png");
            if (!File.Exists(imagePath))
                imagePath = Path.Combine(gameMetadata.GameRoot, @"engine\Sanctuary_Data\Resources\UI\Gameplay\Icons\LogoIcon.png");
            Debug.Assert(imagePath != null);
            UnitImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
        }

        private static readonly string[] techTiers = ["TECH1", "TECH2", "TECH3", "TECH4"];

        private static string GetTier(object node)
        {
            var array = (List<JsonValue>)node;
            foreach (var item in array)
            {
                var text = item.ToString();
                if (techTiers.Contains(text))
                    return text;
            }
            throw new InvalidOperationException();
        }
    }
}
