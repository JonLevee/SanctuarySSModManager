﻿using SanctuarySSLib.LuaUtil;
using SanctuarySSModManager.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// <summary>
    /// Interaction logic for UnitDisplay.xaml
    /// </summary>
    public partial class UnitDisplay : UserControl
    {
        private readonly IGameMetadata gameMetadata;
        private readonly JsonObject data;
        // engine\LJ\lua\common\systems\templateUpdater.lua: unitToProjectile
        private static readonly ContentMetadata[] contentMetadata = [
            new ContentMetadata(instance=>instance.DisplayName, "general/displayName"),
            new ContentMetadata(instance=>instance.UnitName, "general/name"),
            new ContentMetadata(instance=>instance.Faction, "general/tpId", GetFaction),
            new ContentMetadata(instance=>instance.TpId, "general/tpId"),
            new ContentMetadata(instance=>instance.Tier, "tags", GetTier),
            new ContentMetadata(instance=>instance.Health, "defence/health/max"),
            new ContentMetadata(instance=>instance.Mass, "economy/cost/alloys"),
            new ContentMetadata(instance=>instance.Energy, "economy/cost/energy"),
            new ContentMetadata(instance=>instance.BuildTime, "economy/buildTime"),
            ];
        private static readonly GridMetadata[] groupMetadata = [
            new GridMetadata("Economy", "Harvest time", "economy/harvestTime"),
            new GridMetadata("Economy", "Alloys", "economy/harvest/alloys"),
            new GridMetadata("Economy", "Energy", "economy/harvest/energy"),
            new GridMetadata("Intel", "Vision radius", "intel/visionRadius"),
            new GridMetadata("Movement", "Acceleration", "movement/acceleration"),
            new GridMetadata("Movement", "Rotation speed", "movement/rotationSpeed"),
            new GridMetadata("Movement", "Air hover", "movement/airHover"),
            new GridMetadata("Movement", "Min speed", "movement/minSpeed"),
            new GridMetadata("Movement", "Speed", "movement/speed"),
            new GridMetadata("Movement", "Type", "movement/type"),
            new GridMetadata("Weapons", "Damage", "weapons/damage"),
            new GridMetadata("Weapons", "Damage radius", "weapons/damageRadius"),
            new GridMetadata("Weapons", "Damage type", "weapons/damageType"),
            new GridMetadata("Weapons", "Range ring type", "weapons/rangeRingType"),
            new GridMetadata("Weapons", "Range min", "weapons/rangeMin"),
            new GridMetadata("Weapons", "Range max", "weapons/rangeMax"),
            new GridMetadata("Weapons", "Reload time", "weapons/reloadTime"),
            new GridMetadata("Orders", "", "general/orders"),
            ];
        

        // orders
        public UnitDisplay(IGameMetadata gameMetadata, JsonObject data)
        {
            InitializeComponent();
            this.gameMetadata = gameMetadata;
            this.data = data;
        }

        public void Load()
        {
            foreach (var metadata in contentMetadata)
            {
                metadata.SetValue(this, data);
            }
            var tpId = TpId.Text;
            //Faction.Text = GetFaction(tpId);

            // first row has text height
            // 2nd row has separator height
            var textHeight = Grid.RowDefinitions[0].Height;
            var sepHeight = Grid.RowDefinitions[1].Height;
            Grid.RowDefinitions.RemoveAt(1);
            var lastMetadata = GridMetadata.Empty;
            foreach (var metadata in groupMetadata)
            {
                if (!data.TryGet(metadata.ValuePath, out object? value))
                    continue;
                Grid.RowDefinitions.Add(new RowDefinition());
                if (lastMetadata.Group != metadata.Group)
                {
                    Grid.RowDefinitions[Grid.RowDefinitions.Count - 1].Height = sepHeight;
                    Grid.Set(new Separator(), 0, colSpan: Grid.ColumnDefinitions.Count);
                    Grid.RowDefinitions.Add(new RowDefinition());
                    Grid.RowDefinitions[Grid.RowDefinitions.Count - 1].Height = textHeight;
                    Grid.Set(new TextBlock { Text = metadata.Group }, 0);
                    lastMetadata = metadata;
                }
                if (string.IsNullOrWhiteSpace(metadata.Name))
                {
                    if (value is IDictionary<string, JsonNode> dictionary)
                    {
                        var items = dictionary
                            .Keys
                            .Where(k => (bool)dictionary[k])
                            .Order()
                            .ToArray();
                        var text = string.Join(", ", items);
                        Grid.Set(new TextBlock {  Text = text }, 1, colSpan: 3);
                        Grid.RowDefinitions[Grid.RowDefinitions.Count - 1].Height = new GridLength(textHeight.Value, GridUnitType.Auto);
                    }
                    else
                        throw new InvalidOperationException();
                }
                else
                {
                    Grid.Set(new Label { Content = metadata.Name }, 1, colSpan: 2);
                    Grid.Set(new Label { Content = data.Get(metadata.ValuePath) }, 3);
                }
            }
            Grid.RowDefinitions.Add(new RowDefinition());
            var imagePath = Path.Combine(gameMetadata.GameRoot, @"engine\Sanctuary_Data\Resources\UI\Gameplay\IconsUnits", tpId + ".png");
            if (!File.Exists(imagePath))
                imagePath = Path.Combine(gameMetadata.GameRoot, @"engine\Sanctuary_Data\Resources\UI\Gameplay\Icons\LogoIcon.png");
            Debug.Assert(imagePath != null);
            UnitImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
        }

        private static readonly string[] techTiers = ["TECH1", "TECH2", "TECH3", "TECH4"];

        private static string GetTier(object node)
        {
            var array = (List<JsonValue>) node;
            foreach (var item in array)
            {
                var text = item.ToString();
                if (techTiers.Contains(text))
                    return text;
            }
            throw new InvalidOperationException();
        }
        private static string GetFaction(object node)
        {
            var tpId = node.ToString();
            switch(tpId[1])
            {
                case 'c':
                    return "Chosen";
                case 'e':
                    return "EDA";
                case 'g':
                    return "Guard";
            }
            throw new InvalidOperationException($"Unknown faction letter: {tpId[1]}");
        }

        private class GridMetadata
        {
            public static readonly GridMetadata Empty = new GridMetadata(string.Empty, string.Empty, string.Empty);

            public string Group { get; }
            public string Name { get; }
            public string ValuePath { get; }

            public GridMetadata(string group, string name, string valuePath)
            {
                Group = group;
                Name = name;
                ValuePath = valuePath;
            }


        }
        private class ContentMetadata
        {
            private readonly Func<UnitDisplay, TextBlock> getTextBlock;
            private readonly string valuePath;
            private readonly Func<object, string>? getText;

            //public ContentMetadata(Func<UnitDisplay, ContentControl> getContentControl, string valuePath)
            //{
            //    setValue = (unitDisplay,data)=> getContentControl(unitDisplay).Content = data.Get(valuePath);
            //    this.valuePath = valuePath;
            //}

            public ContentMetadata(Func<UnitDisplay, TextBlock> getTextBlock, string valuePath, Func<object, string>? getText = null)
            {
                this.getText = getText ?? GetText;
                this.getTextBlock = getTextBlock;
                this.valuePath = valuePath;
            }

            private string GetText(object node)
            {
                var text = node.ToString();
                return text;
            }

            public void SetValue(UnitDisplay unitDisplay, JsonObject data) 
            {
                var textBlock = getTextBlock(unitDisplay);
                var name = textBlock.Name;
                var value = data.Get(valuePath);
                var text = getText(value);
                textBlock.Text = text;
            }
        }
    }

}
