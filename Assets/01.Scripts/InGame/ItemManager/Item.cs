using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public string id;
    public float duration;
    public bool is_storable;

    public virtual void obtain()
    {
        Debug.Log("Item Pick Up");
    }

    public abstract void use();

    public virtual void expire() { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (ItemManager.instance != null)
                ItemManager.instance.HandleItem(this);
            else
                Debug.Log("Item: No item manager instance");
            

            Destroy(gameObject);
        }
    }
}

