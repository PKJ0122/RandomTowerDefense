using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionUI : UIBase
{
    const int CLEAR_GOLD = 100;

    Transform _location;
    MissionDatas _missionDatas;
    GameObject _missionPrefab;

    RectTransform _scrollLocation;
    Vector2 _scrollEarlyLocation;

    Button _close;

 
    protected override void Awake()
    {
        base.Awake();
        _location = transform.Find("Image - Mission/Scroll View - Mission/Viewport/Content").GetComponent<Transform>();
        _missionDatas = Resources.Load<MissionDatas>("MissionDatas");
        _missionPrefab = _missionDatas.missionPrefeb;
        _close = transform.Find("Image - Mission/Button - CloseButton").GetComponent<Button>();
        _close.onClick.AddListener(Hide);
        _scrollLocation = _location.GetComponent<RectTransform>();
        _scrollEarlyLocation = new Vector2(_scrollLocation.anchoredPosition.x, 0);
    }

    private void Start()
    {
        foreach (MissionBase missionData in _missionDatas.missionDatas)
        {
            GameObject missionObject = Instantiate(_missionPrefab, _location);
            TMP_Text name = missionObject.transform.Find("Text (TMP) - Name").GetComponent<TMP_Text>();
            TMP_Text detail = missionObject.transform.Find("Text (TMP) - Detail").GetComponent<TMP_Text>();
            TMP_Text condition = missionObject.transform.Find("Text (TMP) - Condition").GetComponent<TMP_Text>();
            Transform clear = missionObject.transform.Find("MissionClear").GetComponent<Transform>();
            name.text = missionData.missionName;
            detail.text = missionData.detail;
            missionData.onProgressChange += value =>
            {
                condition.text = $"( {value} / {missionData.condition} )";
                if (missionData.Progress >= missionData.condition) missionData.IsClear = true ;
            };
            missionData.onIsClearChange += value =>
            {
                clear.gameObject.SetActive(value);
                if (value) GameManager.Instance.Gold += CLEAR_GOLD;
            };
            missionData.Init();
        }
    }

    public override void Show()
    {
        base.Show();
        _scrollLocation.anchoredPosition = _scrollEarlyLocation;
    }
}