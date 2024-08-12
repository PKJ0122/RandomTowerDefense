using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MailBoxUI : UIBase
{
    const string URL = "https://docs.google.com/spreadsheets/d/1q07z9KdAlt8oTwqBHc7ReTS7iCv3WzcW2K7HO5OqYzU/export?format=tsv&range=A2:E";

    static Dictionary<string, MailData> _mailDatas;

    Transform _location;
    RectTransform _content;
    Image _mailPrefab;
    Button _close;


    protected override void Awake()
    {
        base.Awake();

        _location = transform.Find("Panel/Image/Scroll View/Viewport/Content").GetComponent<Transform>();
        _content = _location.GetComponent<RectTransform>();
        _mailPrefab = Resources.Load<Image>("Image - Mail");
        _close = transform.Find("Panel/Image/Button - Close").GetComponent<Button>();
        _close.onClick.AddListener(Hide);
        _close.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));

        if (_mailDatas != null)
        {
            Refresh();
        }
    }

    public override void Show()
    {
        base.Show();

        if (_mailDatas == null)
        {
            StartCoroutine(C_SetMailBox());
        }
        _content.anchoredPosition = Vector2.zero;
    }

    IEnumerator C_SetMailBox()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);

        UIBase loding = UIManager.Instance.Get<LodingUI>();
        loding.Show();

        yield return www.SendWebRequest();

        loding.Hide();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string data = www.downloadHandler.text;

            string[] rows = data.Split('\n');
            _mailDatas = new Dictionary<string, MailData>();

            for (int i = 0; i < rows.Length; i++)
            {
                string[] columns = rows[i].Split("\t");
                MailData mailData = new MailData
                                        (
                                            columns[0],
                                            columns[1],
                                            columns[2],
                                            columns[3],
                                            int.Parse(columns[4])
                                        );

                _mailDatas.Add(columns[0], mailData);
            }

            Refresh();
        }
        else
        {
            Hide();
            UIManager.Instance.Get<PopUpUI>().Show("정보를 불러오는데 실패하였습니다.\n인터넷 연결을 확인해주세요.");
        }
    }

    void Refresh()
    {
        int num = PlayerData.PlayerDataContainer.mailSaveDatas.Count - 1;
        List<MailSaveData> list = PlayerData.PlayerDataContainer.mailSaveDatas;
        for (int i = num; i >= 0; i--)
        {
            string mailName = list[i].mailName;

            if (!_mailDatas.ContainsKey(mailName))
            {
                list.RemoveAt(i);
                PlayerData.MailSaveDatas.Remove(mailName);
            }
        }

        foreach (KeyValuePair<string,MailData> item in _mailDatas)
        {
            DateTime startResive = DateTime.Parse(item.Value.startReceive);
            DateTime endResive = DateTime.Parse(item.Value.endReceive);
            DateTime nowTime = DateTime.Now;

            if (!(startResive.Date <= nowTime.Date && nowTime.Date <= endResive.Date)) continue;

            if (PlayerData.MailSaveDatas.ContainsKey(item.Key)) continue;

            Image mail = Instantiate(_mailPrefab, _location);
            TMP_Text mailName = mail.transform.Find("Text (TMP) - Name").GetComponent<TMP_Text>();
            Button get = mail.transform.Find("Button - Get").GetComponent<Button>();
            TMP_Text diaAmount = get.transform.Find("Text (TMP)").GetComponent<TMP_Text>();
            TMP_Text detail = mail.transform.Find("Text (TMP) - Detail").GetComponent<TMP_Text>();
            TMP_Text endResiveT = mail.transform.Find("Text (TMP) - EndResive").GetComponent<TMP_Text>();

            mailName.text = item.Key;
            diaAmount.text = $"<sprite=23>{item.Value.diamondAmount}";
            detail.text = $"{item.Value.mailDetail}";
            endResiveT.text = item.Value.endReceive;

            get.onClick.AddListener(() =>
            {
                get.interactable = false;
                diaAmount.text = "수령완료";
                PlayerData.Instance.Diamond += item.Value.diamondAmount;
                PlayerData.Instance.SetMailData(item.Key);
            });
            get.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
        }
    }
}