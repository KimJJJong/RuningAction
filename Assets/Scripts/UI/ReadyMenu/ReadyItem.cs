using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReadyItem : MonoBehaviour
{
    public List<GameObject> stars = new List<GameObject>();

    public void SetStar(IItemData data)
    {
        for (int i = 0; i < data.GetStarRate(); i++)
        {
            stars[i].SetActive(true);
        }
    }
}
