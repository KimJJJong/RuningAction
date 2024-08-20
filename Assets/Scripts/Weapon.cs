using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    GameObject hitBox;

    private void Start()
    {
        hitBox = GameObject.Find("HitBox");
        hitBox.SetActive(false);
    }


    public void Triger()
    {
        Batting();
    }

    void Batting()
    {


        hitBox.SetActive(true);
        hitBox.transform.position = gameObject.transform.position + new Vector3(0, 1f, 1.5f);

    }

}
