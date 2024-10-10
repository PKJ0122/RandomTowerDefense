using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameEndUI : UIBase
{
    Dictionary<RewerdRank, RewerdData> _rewerdDatas = new Dictionary<RewerdRank, RewerdData>();

    const int GAME_CLEAR_WAVE = 51;

    Transform _gameOver;
    Transform _gameClear;
    Button _reStart;
    Button _lobby;

    Transform _rewerdLocation;

    public event Action OnReStartButtonClick;
    public event Action OnLobbyButtonClick;
    Action _onHide;

    protected override void Awake()
    {
        base.Awake();
        _gameOver = transform.Find("Panel/Text (TMP) - GameOver").GetComponent<Transform>();
        _gameClear = transform.Find("Panel/Text (TMP) - GameClear").GetComponent<Transform>();
        _reStart = transform.Find("Panel/Button - ReStart").GetComponent<Button>();
        _lobby = transform.Find("Panel/Button - Lobby").GetComponent<Button>();
        _reStart.onClick.AddListener(() =>
        {
            Hide();
            OnReStartButtonClick?.Invoke();
            GameManager.Instance.GameReStart();
        });
        _reStart.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
        _lobby.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            OnLobbyButtonClick?.Invoke();
            SceneManager.LoadScene("Lobby");
        });
        _lobby.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
        _rewerdLocation = transform.Find("Panel/Image - Rewerd").GetComponent<Transform>();

        RewerdDatas rewerdDatas = Resources.Load<RewerdDatas>("RewerdDatas");
        foreach (RewerdData rewerdData in rewerdDatas.rewerdDatas)
        {
            _rewerdDatas.Add(rewerdData.rewerdRank, rewerdData);
        }
    }

    public void Show(int wave)
    {
        base.Show();
        bool isClear = wave == GAME_CLEAR_WAVE;
        _gameClear.gameObject.SetActive(isClear);
        _gameOver.gameObject.SetActive(!isClear);
        Rewerd(wave);
        Time.timeScale = 0f;
    }

    public override void Hide()
    {
        base.Hide();
        _gameClear.gameObject.SetActive(false);
        _gameOver.gameObject.SetActive(false);
        _onHide?.Invoke();
        _onHide = null;
    }

    void Rewerd(int wave)
    {
        int chestAmount = (wave / 10);
        for (int i = 0; i < chestAmount; i++)
        {
            int randomChest = Random.Range(0, Enum.GetValues(typeof(RewerdRank)).Length);
            float randomrewerd = Random.Range(0, 100);
            float rewerdChacker1 = 0;
            float rewerdChacker2 = 0;
            RewerdList rewerd = RewerdList.Gold;
            int rewerdAmount = 0;
            RewerdData chest = _rewerdDatas[(RewerdRank)randomChest];
            for (int j = 0; j < Enum.GetValues(typeof(RewerdRank)).Length; j++)
            {
                rewerdChacker2 += chest.rewerdPercentageDatas[j].Percentage;
                if (rewerdChacker1 <= randomrewerd && randomrewerd < rewerdChacker2)
                {
                    rewerd = chest.rewerdPercentageDatas[j].rewerdList;
                    int min = chest.rewerdPercentageDatas[j].MinAmount;
                    int max = chest.rewerdPercentageDatas[j].MaxAmount + 1;
                    rewerdAmount = Random.Range(min, max);
                    break;
                }
                rewerdChacker1 = rewerdChacker2;
            }
            GameObject chestObject = Instantiate(chest.rewerPrefab, _rewerdLocation);
            _onHide += () =>
            {
                chestObject.SetActive(false);
            };

            Button chestButton = chestObject.GetComponent<Button>();
            chestButton.onClick.AddListener(() =>
            {
                chestObject.transform.Find("Chest").gameObject.SetActive(false);
                chestObject.transform.Find($"Image - {rewerd}").gameObject.SetActive(true);
                TMP_Text amount = chestObject.transform.Find("Text (TMP) - Amount").GetComponent<TMP_Text>();
                amount.gameObject.SetActive(true);
                amount.text = rewerdAmount.ToString();
            });
            chestButton.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));

            switch (rewerd)
            {
                case RewerdList.Diamond:
                    PlayerData.Instance.Diamond += rewerdAmount;
                    break;
                case RewerdList.Gold:
                    PlayerData.Instance.Gold += rewerdAmount;
                    break;
                case RewerdList.Relics:
                    PlayerData.Instance.ItemSummons += rewerdAmount;
                    break;
            }
        }
    }

}
