using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null) 
            instance = this;
    }

    private List<Item> item_list = new List<Item>();

    public void handleItem(Item item)
    {
        item.obtain();

        if (!item.is_storable)
            item.use();
        else
            StoreItem(item);

        Destroy(item);
    }

    public void StoreItem(Item item)
    {
        item_list.Add(item);
    }
    
    public void UseItemWithIndex(int index)
    {
        if (index < 0 || index >= item_list.Count)
        {
            Debug.Log("Item Use: Wrong Index");
            return;
        }

        Item item = item_list[index];
        item.use();
        item_list.RemoveAt(index);
    }

    public void UseItemWithName(string id)
    {
        for (int i = 0; i < item_list.Count; i++)
        {
            if (item_list[i].name == id)
            { 
                UseItemWithIndex(i);
                return;
            }
        }
        Debug.Log("Item Use: No matching id with given id");
    }


    //TODO: (if applicable) Make storable item in game in real time.
    //We don't provide item storage function in game, so using storable item function is made yet   
}
