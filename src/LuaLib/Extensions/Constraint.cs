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
    }
}
