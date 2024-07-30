using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombinationBookButtonUI : UIBase
{
    Button _combinationBook;


    protected override void Awake()
    {
        base.Awake();
        _combinationBook = transform.Find("Button - CombinationBookButton").GetComponent<Button>();
    }

    void Start()
    {
        _combinationBook.onClick.AddListener(() => UIManager.Instance.Get<CombinationBookUI>().Show());
    }
}
