using DiffMatchPatch;
using SanctuarySSModManager;
using System.Text;

namespace UnitTests
{
    // https://github.com/google/diff-match-patch/wiki/API

    public class PatchingTests
    {
        private StringBuilder contents1 = new StringBuilder();
        private StringBuilder contents2 = new StringBuilder();
        private diff_match_patch patcher;
        //var patch = new diff_match_patch();

        [SetUp]
        public void Setup()
        {
            contents1 = new StringBuilder(@"
What is Lorem Ipsum?
Lorem Ipsum is simply dummy text of the printing and typesetting industry. 
Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. 
It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. 
It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.
");
            contents2 = new StringBuilder(contents1.ToString());

            patcher = new diff_match_patch();
        }

        [Test]
        public void NoChanges()
        {
            var modPatcher = new ModPatcher();
            var patches = patcher.patch_make(contents1.ToString(), contents2.ToString());
            Assert.IsEmpty(patches);
        }

        [Test]
        public void SingleChange()
        {
            contents2 = contents2.Replace("desktop", "mobile");
            var patches = patcher.diff_main(contents1.ToString(), contents2.ToString());
            patcher.diff_cleanupSemantic(patches);
            var patch = patcher.patch_make(patches);
            var text = patcher.patch_toText(patch);
            Assert.That(text, Does.Contain("\n ith \n-desktop\n+mobile\n  pub\n"));
        }

        [Test]
        public void CanPatchSingleChange()
        {
            contents2 = contents2.Replace("desktop", "mobile");
            var patches = patcher.diff_main(contents1.ToString(), contents2.ToString());
            patcher.diff_cleanupSemantic(patches);
            var patch = patcher.patch_make(patches);
            var text = patcher.patch_toText(patch);
            Assert.That(text, Does.Contain("\n ith \n-desktop\n+mobile\n  pub\n"));
            patch = patcher.patch_make(contents1.ToString(), patches);
            text = patcher.patch_toText(patch);
            Assert.That(text, Does.Contain("\n ith \n-desktop\n+mobile\n  pub\n"));
        }
    }
}