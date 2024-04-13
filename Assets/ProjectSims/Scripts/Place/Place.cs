using System;
using System.Collections.Generic;
using ProjectSims.Scripts.General;

namespace ProjectSims.Scripts.Place
{
    public enum PlaceType
    {
        Restaurant,
        FoodStall,
        Garden,
        Park,
        AmusementPark
    }

    //Todo: connect this with entity with raycasting
    public class Place
    {
        protected string _name;
        public Guid Guid { get; private set; }
        
        public List<Guid> ListItem { get; private set; }

        public virtual void Initialize()
        {
            Guid = Guid.NewGuid();
            _name = "Place";
            // _dictCustomerData = new Dictionary<Guid, CustomerData>();
            // _customerData = new List<CustomerData>();
            ListItem = new List<Guid>();
            ListItem.Clear();
        }

        public void RegisterItems(ItemSO[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                ItemSO so = items[i];
                Item item = new Item();
                item.SetName(so.Name).SetDescription(so.Description).SetPrice(so.Cost).SetOwner(Guid);
                GameController.RegisterItem(item);
                ListItem.Add(item.Guid);
            }
        }

        // public CustomerData GetCustomerData(Guid guid)
        // {
        //     if (!_dictCustomerData.ContainsKey(guid))
        //         return default;
        //
        //     return _dictCustomerData[guid];
        // }
        //
        // public void BuyItem(Guid customerId, Item item, int amount) // for entity
        // {
        //     //ToDo: track bought item
        //     bool isContains = _dictCustomerData.ContainsKey(customerId);
        //     CustomerData custData = default;
        //     Debug.Log($"Buy Item: {item.Name}");
        //     if (!isContains)
        //     {
        //         custData = new CustomerData(customerId);
        //         custData.AddItem(item);
        //         _dictCustomerData.Add(customerId, custData);
        //         _customerData.Add(custData);
        //     }
        //     else
        //     {
        //         custData = _dictCustomerData[customerId];
        //         var history = custData.GetItemHistory(item);
        //         if (history == null)
        //         {
        //             custData.AddItem(item);
        //             return;
        //         }
        //
        //         history.Add();
        //     }
        // }

        public virtual void Visit(Entity entity)
        {
            //implement each logic in derivative class
        }

        // public string Save()
        // {
        //     SaveDataPlace save = new SaveDataPlace();
        //     save.PlaceName = _name;
        //     save.Guid = Guid.ToString();
        //     save.PlaceType = PlaceType.Restaurant;
        //     save.CustomerDataJson = JsonConvert.SerializeObject(_customerData);
        //     Debug.Log(save.CustomerDataJson);
        //     save.ListItemJson = JsonConvert.SerializeObject(ListItem);
        //
        //     return JsonConvert.SerializeObject(save);
        // }
        //
        // public virtual void Load(string data)
        // {
        //     if (_dictCustomerData == null)
        //         _dictCustomerData = new Dictionary<Guid, CustomerData>();
        //     _dictCustomerData.Clear();
        //
        //     SaveDataPlace save = JsonConvert.DeserializeObject<SaveDataPlace>(data);
        //     Guid = Guid.Parse(save.Guid);
        //     _name = save.PlaceName;
        //     ListItem = JsonConvert.DeserializeObject<List<Guid>>(save.ListItemJson);
        //     _customerData = JsonConvert.DeserializeObject<List<CustomerData>>(save.CustomerDataJson);
        //
        //     for (int i = 0; i < _customerData.Count; i++)
        //     {
        //         var c = _customerData[i];
        //         _dictCustomerData.Add(c._customerID, c);
        //     }
        // }
    }
}