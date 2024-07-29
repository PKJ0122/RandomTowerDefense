using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemEffect : PoolObject
{
    Image _item;
    TMP_Text _itemName;


    void Awake()
    {
        _item = transform.Find("Image - Item").GetComponent<Image>();
        _itemName = transform.Find("Text (TMP) - ItemName").GetComponent<TMP_Text>();
    }

    public ItemEffect Denote(Sprite item, string itemName)
    {
        _item.sprite = item;
        _itemName.text = $"{itemName} 효과 발동";
        StartCoroutine(C_Activity());

        return this;
    }

    YieldInstruction _delay = new WaitForSeconds(2f);

    IEnumerator C_Activity()
    {
        yield return _delay;

        RelasePool();
    }
}