using SanctuarySSLib.MiscUtil;
using System.Reflection;

namespace UnitTests
{
    public class SteamInfoTest : ISteamInfo
    {
        public string TestRoot { get; set; }
        public SteamInfoTest(SteamInfo steamInfo) 
        {
            var location = Assembly.GetExecutingAssembly().Location;
            TestRoot = Path.Combine(location, "SanctuaryTestData");
        }
        public string GetRoot(string appName)
        {
            throw new NotImplementedException();
        }
    }
}