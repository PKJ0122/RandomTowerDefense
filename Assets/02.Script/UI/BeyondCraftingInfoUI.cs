using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BeyondCraftingInfoUI : UIBase
{
    const int PRICE = 3000;

    Image _unit;
    TMP_Text _unitName;
    Image[] _materials = new Image[3];
    Button _buy;

    Button _close;


    protected override void Awake()
    {
        base.Awake();
        _unit = transform.Find("Panel/Image/Image - Unit/Image - Unit").GetComponent<Image>();
        _unitName = transform.Find("Panel/Image/Image - UnitName/Text (TMP)").GetComponent<TMP_Text>();
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i] = transform.Find($"Panel/Image/Image - Material{i}").GetComponent<Image>();
        }
        _buy = transform.Find("Panel/Image/Button - Buy").GetComponent<Button>();
        _close = transform.Find("Panel/Image/Button - Close").GetComponent<Button>();
        _close.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
        _close.onClick.AddListener(Hide);
    }

    public void Show(BeyondCraftingMethod method, BeyondCraftingData craftingData)
    {
        base.Show();
        UnitData unitData = UnitRepository.UnitKindDatas[method.unitKind];
        _unit.sprite = unitData.unitImg;
        _unitName.text = unitData.unitName;
        for (int i = 0; i < _materials.Length; i++)
        {
            Image materialImg = _materials[i].transform.Find("Image - Unit").GetComponent<Image>();
            TMP_Text materialRank = _materials[i].transform.Find("Text (TMP) - Rank").GetComponent<TMP_Text>();
            UnitInfo material = method.beyondCraftingMaterials[i];
            materialImg.sprite = UnitRepository.UnitKindDatas[material.unitKind].unitImg;
            UnitRankData unitRankData = UnitRepository.UnitRankDatas[material.unitRank];
            materialRank.text = unitRankData.unitRankName;
            materialRank.color = unitRankData.unitRankColor;
        }
        bool ishave = craftingData.IsHave;

        _buy.gameObject.SetActive(!ishave);

        if (ishave) return;

        _buy.onClick.RemoveAllListeners();
        _buy.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
        _buy.onClick.AddListener(() =>
        {
            if (PlayerData.Instance.Diamond < PRICE)
            {
                SoundManager.Instance.PlaySound(SFX.Fail);
                return;
            }

            PlayerData.Instance.Diamond -= PRICE;
            craftingData.IsHave = true;
            Hide();
        });
    }
}
