using MoreLinq;
using SanctuarySSLib.MiscUtil;
using System.Diagnostics;
using System.Reflection;

namespace UnitTests
{
    public class SteamInfoTest : ISteamInfo
    {
        private readonly string testRoot;
        private readonly string workingRoot;
        private readonly string backupRoot;
        private readonly string[] extensionsToCopy;
        public SteamInfoTest(SteamInfo steamInfo)
        {
            extensionsToCopy = ["*.lua", "*.santp"];
            var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            testRoot = Path.Combine(location, "SanctuaryTestData");
            backupRoot = Path.Combine(testRoot, "Backup");
            workingRoot = Path.Combine(testRoot, "Working");
            EnsureBackupInitialized(steamInfo);
            EnsureWorkingInitialized(steamInfo);
        }
        private enum Action
        {
            None,
            Create,
            Delete,
            Copy
        }
        private struct ActionInfo
        {
            public Action action;
            public string backup;
            public string working;
        }
        private void EnsureWorkingInitialized(SteamInfo steamInfo)
        {
            Directory.CreateDirectory(workingRoot);
            var mergedDirectories = Directory
                .GetDirectories(backupRoot, "*", SearchOption.AllDirectories)
                .OrderedMerge(
                    Directory.GetDirectories(workingRoot, "*", SearchOption.AllDirectories),
                    key => key.Substring(backupRoot.Length),
                    key => key.Substring(workingRoot.Length),
                    result => new ActionInfo { action = Action.Create, working = workingRoot + result.Substring(backupRoot.Length) },
                    result => new ActionInfo { action = Action.Delete, working = result },
                    (_, _) => new ActionInfo { action = Action.None }
                )
                .ToList();
            foreach (var item in mergedDirectories)
            {
                switch (item.action)
                {
                    case Action.Create:
                        Directory.CreateDirectory(item.working);
                        break;
                    case Action.Delete:
                        Directory.Delete(item.working);
                        break;
                }
            }


            var mergedFiles = extensionsToCopy
                .SelectMany(ext => Directory.GetFiles(backupRoot, ext, SearchOption.AllDirectories))
                .OrderedMerge(
                    extensionsToCopy.SelectMany(ext => Directory.GetFiles(workingRoot, ext, SearchOption.AllDirectories)),
                    key => key.Substring(backupRoot.Length),
                    key => key.Substring(workingRoot.Length),
                    backup => new ActionInfo { action = Action.Create, working = workingRoot + backup.Substring(backupRoot.Length) },
                    working => new ActionInfo { action = Action.Delete, working = working },
                    (backup, working) => new ActionInfo
                    {
                        action = Action.None
                    }
                )
                .ToList();
        }

        private void EnsureBackupInitialized(SteamInfo steamInfo)
        {
            if (Directory.Exists(backupRoot))
            {
                //Directory.Delete(backupRoot, true);
                return;
            }
            var realRoot = steamInfo.GetRoot(AppInfo.DefaultShatteredSunSteamName);
            foreach (var extension in extensionsToCopy)
            {
            }
            var files = extensionsToCopy
                .SelectMany(ext => Directory.GetFiles(realRoot, ext, SearchOption.AllDirectories))
                .Order()
                .ToList();
            var directories = files.Select(Path.GetDirectoryName).Distinct().Order().ToList();
            foreach (var dir in directories)
            {
                var target = backupRoot + dir.Substring(realRoot.Length);
                Directory.CreateDirectory(target);
            }
            foreach (var file in files)
            {
                var target = backupRoot + file.Substring(realRoot.Length);
                File.Copy(file, target);
            }
        }

        public string GetRoot(string appName)
        {
            throw new NotImplementedException();
        }
    }
}