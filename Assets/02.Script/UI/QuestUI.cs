using UnityEngine;
using UnityEngine.UI;

public class QuestUI : UIBase
{
    QuestDatas _questDatas;
    GameObject _questPrefab;
    Transform _questLocation;


    protected override void Awake()
    {
        base.Awake();
        _questDatas = Resources.Load<QuestDatas>("QuestDatas");
        foreach (QuestBase item in _questDatas.questDatas)
        {
            GameObject questPrefab = Instantiate(_questPrefab, _questLocation);
            QuestSaveData questSaveData = PlayerData.Instance.GetQuestData(item.questName);
            questSaveData.OnAmountChange += value =>
            {
                Button button = questPrefab.transform.Find("").GetComponent<Button>();
                questSaveData.OnAmountChange += value =>
                {
                    button.interactable = value >= item.requirements;
                };
                button.onClick.AddListener(() =>
                {
                    PlayerData.Instance.Diamond += questSaveData.Amount;
                });
            };
        }
    }

    void OnDisable()
    {
        foreach (QuestBase item in _questDatas.questDatas)
        {
            QuestSaveData questSaveData = PlayerData.Instance.GetQuestData(item.questName);
            questSaveData.Reset();
        }
    }
}
