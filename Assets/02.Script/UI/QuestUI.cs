using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : UIBase
{
    GameObject _questPrefab;
    Transform _questLocation;
    RectTransform _questContent;

    Button _close;


    protected override void Awake()
    {
        base.Awake();
        _questPrefab = Resources.Load<GameObject>("Image - QuestSlot");
        _questLocation = transform.Find("Panel/Image/Scroll View/Viewport/Content").GetComponent<Transform>();
        _questContent = _questLocation.GetComponent<RectTransform>();
        foreach (QuestBase item in QuestManager.QuestDatas.questDatas)
        {
            GameObject questPrefab = Instantiate(_questPrefab, _questLocation);
            QuestSaveData questSaveData = PlayerData.Instance.GetQuestData(item.questName);
            TMP_Text questDetail = questPrefab.transform.Find("Text (TMP) - Detail").GetComponent<TMP_Text>();
            questDetail.text = item.questName;
            Slider questShameS = questPrefab.transform.Find("Slider - Shame").GetComponent<Slider>();
            TMP_Text questShameT = questShameS.transform.Find("Text (TMP) - Shame").GetComponent<TMP_Text>();
            questShameS.maxValue = item.requirements;
            questShameS.value = questSaveData.Amount;
            questShameT.text = $"{questSaveData.Amount} / {item.requirements}";
            Button rewardButton = questPrefab.transform.Find("Button - GetReward").GetComponent<Button>();
            TMP_Text reward = rewardButton.transform.Find("Text (TMP) - Reward").GetComponent<TMP_Text>();
            reward.text = item.reward.ToString();
            rewardButton.interactable = questSaveData.Amount >= item.requirements;
            rewardButton.onClick.AddListener(() =>
            {
                PlayerData.Instance.Diamond += item.reward;
                questSaveData.Amount -= item.requirements;
            });
            questSaveData.OnAmountChange += value =>
            {
                rewardButton.interactable = value >= item.requirements;
                questShameS.value = value;
                questShameT.text = $"{value} / {item.requirements}";
            };
        }
        _close = transform.Find("Panel/Image/Button - Close").GetComponent<Button>();
        _close.onClick.AddListener(Hide);
    }

    public override void Show()
    {
        base.Show();
        _questContent.anchoredPosition = Vector2.zero;
    }

    void OnDisable()
    {
        foreach (QuestBase item in QuestManager.QuestDatas.questDatas)
        {
            QuestSaveData questSaveData = PlayerData.Instance.GetQuestData(item.questName);
            questSaveData.Reset();
        }
    }
}
