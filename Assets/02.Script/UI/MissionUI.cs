using TMPro;
using UnityEngine;

public class MissionUI : UIBase
{
    const int CLEAR_GOLD = 100;

    Transform _location;
    MissionDatas _missionDatas;
    GameObject _missionPrefeb;


    protected override void Awake()
    {
        base.Awake();
        _location = transform.Find("Image - Mission/Scroll View - Mission/Viewport/Content").GetComponent<Transform>();
        _missionDatas = Resources.Load<MissionDatas>("MissionDatas");
        _missionPrefeb = _missionDatas.missionPrefeb;
    }

    private void Start()
    {
        foreach (MissionData missionData in _missionDatas.missionDatas)
        {
            GameObject missionObject = Instantiate(_missionPrefeb, _location);
            TMP_Text detail = missionObject.transform.Find("Text (TMP) - Detail").GetComponent<TMP_Text>();
            TMP_Text condition = missionObject.transform.Find("Text (TMP) - Condition").GetComponent<TMP_Text>();
            GameObject clear = missionObject.transform.Find("MissionClear").GetComponent<GameObject>();
            detail.text = missionData.detail;
            missionData.onProgressChange += value =>
            {
                condition.text = $"( {value} / {missionData.condition} )";
            };
            missionData.onIsClearChange += value =>
            {
                clear.SetActive(value);
                if (value) GameManager.Instance.Gold += CLEAR_GOLD;
            };
            missionData.Init();
        }
    }
}