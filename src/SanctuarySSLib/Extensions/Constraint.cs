using System.Diagnostics.CodeAnalysis;

namespace SanctuarySSModManager.Extensions
{
    public static class Constraint
    {
        public static void NotNull([NotNull] object? o, string msg = "")
        {
            if (o == null)
            {
                throw new Exception("Object cannot be null " + msg);
            }
        }

        public static IEnumerable<TSource> WhereNotNull<TSource>(this IEnumerable<TSource> source)
        {
            return source.Where(x => x != null);
        }

        public static void FileExists(this string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"file: '{path}'");
        }

        public static void DirectoryExists(this string path)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException($"directory: '{path}'");
        }
    }
}