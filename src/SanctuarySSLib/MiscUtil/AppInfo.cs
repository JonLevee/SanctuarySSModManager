using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace SanctuarySSLib.MiscUtil
{
    public class AppInfo
    {
        public string AppName { get; }
        public string ShatteredSunInstallRoot { get; }
        public string ShatteredSunSteamName { get; }

        public static AppInfo CreateInstance(IServiceProvider serviceProvider)
        {
            var appName = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyProductAttribute>()?.Product;
            Contract.Assert(appName != null);
            var steamInfo = serviceProvider.GetService<ISteamInfo>();
            Contract.Assert(steamInfo != null);
            var shatteredSunSteamName = "Sanctuary Shattered Sun Demo";
            var shatteredSunInstallRoot = steamInfo.GetRoot(shatteredSunSteamName);
            return new AppInfo(appName, shatteredSunInstallRoot, shatteredSunSteamName);
        }
        private AppInfo(string appName, string shatteredSunInstallRoot, string shatteredSunSteamName) 
        {
            AppName = appName;
            ShatteredSunInstallRoot = shatteredSunInstallRoot;
            ShatteredSunSteamName = shatteredSunSteamName;
        }
    }
}
