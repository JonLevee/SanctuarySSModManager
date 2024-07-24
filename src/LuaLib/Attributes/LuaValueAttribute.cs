namespace SanctuarySSLib.Attributes
{
    public class LuaValueAttribute : Attribute
    {
        public LuaValueAttribute(string? key = null)
        {
            Key = key;
        }

        public string? Key { get; }
    }
}
