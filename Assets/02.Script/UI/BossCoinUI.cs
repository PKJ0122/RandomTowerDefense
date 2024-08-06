using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossCoinUI : UIBase
{
    TMP_Text _key;

    Button _open;
    Button _close;

    Button _epicUse;
    Button _uniqueUse;
    Button _legendaryUse;

    TMP_Text _epicT;
    TMP_Text _uniqueT;
    TMP_Text _legendaryT;

    Image _boxTop;
    TMP_Text _itemT;
    Vector3 _itemTLocation;
    bool _isInit;

    int _epic;
    public int Epic
    {
        get => _epic;
        set
        {
            _epic = value;
            _epicT.text = value.ToString();
            _epicUse.interactable = value > 0;
        }
    }
    int _unique;
    public int Unique
    {
        get => _unique;
        set
        {
            _unique = value;
            _uniqueT.text = value.ToString();
            _uniqueUse.interactable = value > 0;
        }
    }
    int _legendary;
    public int Legendary
    {
        get => _legendary;
        set
        {
            _legendary = value;
            _legendaryT.text = value.ToString();
            _legendaryUse.interactable = value > 0;
        }
    }

    Coroutine _effect;


    protected override void Awake()
    {
        base.Awake();
        _key = transform.Find("Panel/Image/Image - Key/Text (TMP) - Amount").GetComponent<TMP_Text>();
        _open = transform.Find("Panel/Image/Button - Open").GetComponent<Button>();
        _close = transform.Find("Panel/Image/Button - Close").GetComponent<Button>();
        _close.onClick.AddListener(Hide);

        Transform selectTransform = transform.Find("Panel/Image/Image - Select").GetComponent<Transform>();

        _epicT = selectTransform.Find("Image - Epic/Text (TMP) - Amount").GetComponent<TMP_Text>();
        _uniqueT = selectTransform.Find("Image - Unique/Text (TMP) - Amount").GetComponent<TMP_Text>();
        _legendaryT = selectTransform.Find("Image - Legendary/Text (TMP) - Amount").GetComponent<TMP_Text>();
        _epicUse = selectTransform.Find("Image - Epic/Button - Use").GetComponent<Button>();
        _uniqueUse = selectTransform.Find("Image - Unique/Button - Use").GetComponent<Button>();
        _legendaryUse = selectTransform.Find("Image - Legendary/Button - Use").GetComponent<Button>();
        _boxTop = transform.Find("Panel/Image/Image - Box/Image - top").GetComponent<Image>();
        _itemT = transform.Find("Panel/Image/Image - Box/Text (TMP) - Item").GetComponent<TMP_Text>();

        _open.onClick.AddListener(() =>
        {
            if (GameManager.Instance.Key <= 0) return;
            if (_effect != null) StopCoroutine(_effect);

            GameManager.Instance.Key--;
            _effect = StartCoroutine(C_Effect());
        });
        GameManager.Instance.OnGameStart += () =>
        {
            Epic = 0;
            Unique = 0;
            Legendary = 0;
        };
        GameManager.Instance.OnKeyChange += v =>
        {
            _key.text = v.ToString();
        };
    }

    void Start()
    {
        UnitSelectSpawnUI ui = UIManager.Instance.Get<UnitSelectSpawnUI>();
        _epicUse.onClick.AddListener(() => UseButtonClick(UnitRank.Epic));
        _uniqueUse.onClick.AddListener(() => UseButtonClick(UnitRank.Unique));
        _legendaryUse.onClick.AddListener(() => UseButtonClick(UnitRank.Legendary));
        UIManager.Instance.Get<UnitSelectSpawnUI>().OnIsUse += unit =>
        {
            if (unit.Rank == UnitRank.Epic) Epic--;
            else if (unit.Rank == UnitRank.Unique) Unique--;
            else if (unit.Rank == UnitRank.Legendary) Legendary--;
        };
    }

    public override void Show()
    {
        base.Show();

        if (_isInit) return;

        _isInit = true;
        _itemTLocation = _itemT.transform.position;
    }

    void UseButtonClick(UnitRank rank)
    {
        Slot slot = SlotManager.IsVacancy();
        if (slot == null)
        {
            return;
        }

        UIManager.Instance.Get<UnitSelectSpawnUI>().Show(rank, slot);
    }

    WaitForSecondsRealtime _delay = new WaitForSecondsRealtime(0.3f);

    IEnumerator C_Effect()
    {
        _itemT.transform.position = _itemTLocation;
        _boxTop.enabled = true;
        RandomSpawnItem();
        yield return _delay;
        _boxTop.enabled = false;
        yield return _delay;

        float t = 0;

        while (t <= 1f)
        {
            t += Time.deltaTime;
            _itemT.transform.position = Vector3.Lerp(_itemTLocation, _itemTLocation + new Vector3(0, 70f, 0), t / 1f);
            yield return null;
        }

        _itemT.transform.position = _itemTLocation;
        _boxTop.enabled = true;
        _effect = null;
    }

    void RandomSpawnItem()
    {
        int RandomNum = Random.Range(0, 100);

        UnitRank unitRank = UnitRank.Epic;

        if (0 <= RandomNum && RandomNum < 10)
        {
            unitRank = UnitRank.Legendary;
            Legendary++;
        }
        else if (10 <= RandomNum && RandomNum < 30)
        {
            unitRank = UnitRank.Unique;
            Unique++;
        }
        else
        {
            Epic++;
        }

        UnitRankData rankData = UnitRepository.UnitRankDatas[unitRank];
        _itemT.text = rankData.unitRankName;
        _itemT.color = rankData.unitRankColor;
    }
}
