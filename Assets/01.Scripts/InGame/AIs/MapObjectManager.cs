using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapObjectManager : MonoBehaviour
{
    public GameObject character;
    public float act_dist = 100f;
    public List<GameObject> interactable_objects;


    private void Start()
    {

    }

    void Update()
    {
        List<GameObject> objectsToRemove = new List<GameObject>();

        foreach (GameObject _object in interactable_objects)
        {
            float dist = Vector3.Distance(character.transform.position, _object.transform.position);

            if (dist <= act_dist)
            {
                _object.SetActive(true);
                objectsToRemove.Add(_object);
            }
            else
                _object.SetActive(false);
        }

        foreach (GameObject _object in objectsToRemove)
            interactable_objects.Remove(_object);
    }

    public void RegisterMapObjects(GameObject mapSection)
    {
        GameObject[] interactables = mapSection
            .GetComponentsInChildren<Transform>(true)
            .Where(t => t.CompareTag("InteractableMapObject"))
            .Select(t => t.gameObject)
            .ToArray();

        interactable_objects.AddRange(interactables);
    }

    public void DeregisterMapObjects(GameObject mapSection)
    {
    //     GameObject[] interactables = mapSection
    //         .GetComponentsInChildren<Transform>(true)
    //         .Where(t => t.CompareTag("InteractableMapObject"))
    //         .Select(t => t.gameObject)
    //         .ToArray();
        
    //     interactable_objects.RemoveAll(obj => interactables.Contains(obj));
     }
}
