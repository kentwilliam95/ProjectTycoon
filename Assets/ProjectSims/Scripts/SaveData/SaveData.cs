using ProjectSims.Scripts.General;
using ProjectSims.Scripts.Place;

namespace ProjectSims.Scripts.SaveData
{
    public struct SaveDataPlace
    {
        public string Guid;
        public string PlaceName;
        public string ListItemJson;
        public string CustomerDataJson;

        public PlaceType PlaceType;
    }

    public struct SaveDataDetailItem
    {
        public string PlaceGuid;
        public Item Item;
    }

    public struct SaveDataGameController
    {
        public SaveDataPlace[] Places;
        public Item[] Items;
        public string[] entityGuids;
    }
}