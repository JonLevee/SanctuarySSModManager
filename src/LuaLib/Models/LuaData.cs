namespace SanctuarySSLib.Models
{

    public class LuaData
    {
        public Dictionary<string, LuaTableData> Data { get; }

        public LuaData()
        {
            Data = new Dictionary<string, LuaTableData>();
        }
    }
}