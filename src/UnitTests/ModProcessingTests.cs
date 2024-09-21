using System.Diagnostics.Contracts;
using System.Diagnostics;
using SanctuarySSModManager;

namespace UnitTests
{
    public class ModProcessingTests : ModProcessingTestBase
    {

        [Test]
        public void ModSetup()
        {
            var combined = DIContainer.Get<SSSCombinedSettings>();

        }
    }
}