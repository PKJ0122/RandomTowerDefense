using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAmountUI : UIBase
{
    const int DIE_AMOUNT = 100;

    Slider _enemyAmountS;
    TMP_Text _enemyAmountT;

    protected override void Awake()
    {
        base.Awake();
        _enemyAmountS = transform.Find("Slider - EnemyAmount").GetComponent<Slider>();
        _enemyAmountT = transform.Find("Slider - EnemyAmount/Text (TMP) - EnemyAmount").GetComponent<TMP_Text>();

        GameManager.Instance.OnEnemyAmountChange += value =>
        {
            _enemyAmountS.value = value;
            _enemyAmountT.text = $"{value} / {DIE_AMOUNT}";
        };
    }
}
