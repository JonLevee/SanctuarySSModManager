using System.Diagnostics.Contracts;
using System.Diagnostics;
using SanctuarySSModManager;

namespace UnitTests
{
    public class ModProcessingTests : ModProcessingTestBase
    {

        [Test]
        public void ModSetup_Clean()
        {
            var combined = DIContainer.Get<SSSCombinedSettings>();
            Assert.That(combined.LuaFolder, Is.EqualTo("engine"));
            Assert.That(combined.FullModRootFolder, Does.Exist);
            Assert.That(combined.ModManagerEnabled, Is.False);

        }
    }
}