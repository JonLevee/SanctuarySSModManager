namespace SanctuarySSLib.Enums
{
    [Flags]
    public enum UnitEnabledEnum
    {
        Enabled = 1 << 1,
        Disabled = 1 << 2,
        MissingAvail = 1 << 3 | Disabled,
    }
}
