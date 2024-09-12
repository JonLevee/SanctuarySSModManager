
using Microsoft.Extensions.DependencyInjection;
using SanctuarySSLib.Attributes;
using SanctuarySSLib.Enums;
using SanctuarySSLib.LuaUtil;
using SanctuarySSModManager.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SanctuarySSModManager
{
    public class SSSUserSettings
    {
        public FolderModeEnum FolderMode {  get; set; }
        public string ShatteredSunDirectoryRoot { get; set; }
        public string ModRootFolder { get; set; }

        public string FullModRootFolder => Path.Combine(ShatteredSunDirectoryRoot, ModRootFolder);

    }
}
