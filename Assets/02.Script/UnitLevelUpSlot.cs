using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitLevelUpSlot : MonoBehaviour
{
    public Button levelUp { get; private set; }
    public TMP_Text level { get; private set; }
    public TMP_Text levelUpNeedGold { get; private set; }
    public Image unitImg { get; private set; }


    void Awake()
    {
    }
}