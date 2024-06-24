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
        levelUp = GetComponent<Button>();
        unitImg = transform.Find("Image - Unit").GetComponent<Image>();
        level = transform.Find("Text (TMP) - Level").GetComponent<TMP_Text>();
        levelUpNeedGold = transform.Find("Text (TMP) - LevelUpGold").GetComponent<TMP_Text>();
    }
}