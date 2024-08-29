using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUnit : MonoBehaviour
{
    [SerializeField]
    private CharacterData charData;

    [SerializeField]
    int stars;

    public int num;
    private Button button;

    public Image chrImg;

    public Sprite checkedFrame;
    public Sprite unCheckedFrame;

    private void Start()
    {
        button = GetComponent<Button>();
        chrImg.sprite = charData.characterImg;

    }

    void Update()
    {
        if (transform.GetSiblingIndex() == 0)
        {
            button.image.sprite = checkedFrame;
        }
        else
        {
            button.image.sprite = unCheckedFrame;
        }
    }

    public void CheckThis()
    {
        transform.SetAsFirstSibling();
    }

    public void SetData(CharacterData data)
    {
        charData = data;
    }

    public CharacterData GetData()
    {
        return charData;
    }

}
