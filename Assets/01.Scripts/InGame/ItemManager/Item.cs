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
        Debug.Log("Item acquired");

        if (other.CompareTag("Player"))
        {
            Debug.Log("Item acquired2");

            if (ItemManager.instance != null)
                ItemManager.instance.handleItem(this);
            else
                Debug.Log("Item: No item manager instance");
            

            Destroy(gameObject);
        }
    }
}

