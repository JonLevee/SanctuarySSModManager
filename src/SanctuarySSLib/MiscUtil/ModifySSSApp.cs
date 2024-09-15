using SanctuarySSModManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanctuarySSLib.MiscUtil
{
    public class ModifySSSApp
    {
        private record FileChange(string fileName, Action modify, Action revert);
        private readonly SSSModManagerSettings modManagerSettings;
        private readonly SSSUserSettings userSettings;
        private readonly AppInfo appInfo;
        private readonly IUserInteraction userInteraction;
        private readonly Dictionary<string, FileChange> fileChanges;

        public ModifySSSApp(
            SSSModManagerSettings modManagerSettings,
            SSSUserSettings userSettings,
            AppInfo appInfo,
            IUserInteraction userInteraction) 
        {
            this.modManagerSettings = modManagerSettings;
            this.userSettings = userSettings;
            this.appInfo = appInfo;
            this.userInteraction = userInteraction;
            fileChanges = new FileChange[] {
                new FileChange("import.lua", null, null),
                }.ToDictionary(fc=>fc.fileName, StringComparer.OrdinalIgnoreCase);

        }

        public void ModManagerEnablement(bool enable)
        {
            if (enable && !userSettings.DisplayedSetupMessage)
            {
                var message = new StringBuilder();
                message.Append($"In order to manage mods for {appInfo.ShatteredSunSteamName}, ");
                message.AppendLine("some changes need to be made to the following files: ");
                foreach(var file in fileChanges.Keys)
                {
                    message.AppendLine($"  {file}");
                }
                message.AppendLine("The files will be backed up and the originals will be replaced when you disable.");
                message.AppendLine();
                message.AppendLine("Enable now?");
                var result = userInteraction.MessageBox(message.ToString(), appInfo.AppName, UserInteractionButton.YesNo);
                if (result == UserInteractionResult.Yes)
                {
                    userSettings.DisplayedSetupMessage = true;
                } else {
                    return;
                }
            }
            modManagerSettings.ModManagerEnabled = enable;
        }
    }
}
