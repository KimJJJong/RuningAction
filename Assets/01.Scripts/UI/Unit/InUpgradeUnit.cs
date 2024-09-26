using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InUpgradeUnit : MonoBehaviour
{
    [SerializeField]
    public InUpgradeData upgradeData;

    public int level;

    private Button button;

    public Image upgradeImg;

    public Sprite checkedFrame;
    public Sprite unCheckedFrame;

    private void Start()
    {
        button = GetComponent<Button>();
        upgradeImg.sprite = upgradeData.upgradeImg;
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

    public void SetData(InUpgradeData data)
    {
        upgradeData = data;
    }

    public InUpgradeData GetData()
    {
        return upgradeData;
    }
}
