using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanctuarySSModManager.Extensions
{
    internal static class GeneralExtensions
    {
        public static void EnsureDirectoryExists(this string directory)
        {
            var info = new DirectoryInfo(directory);
            if (info.Exists)
            {
                return;
            }
            if (info.FullName.Equals(info.Root.FullName, StringComparison.OrdinalIgnoreCase))
            {
                throw new DirectoryNotFoundException($"Directory root {info.Root.FullName} does not exist");
            }
            EnsureDirectoryExists(info.Parent.FullName);
            Directory.CreateDirectory(info.FullName);
        }
    }
}
