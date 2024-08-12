using UnityEngine;
using UnityEngine.UI;

public class GPGSDownloadUI : UIBase
{
    const string GPG_URL = "market://details?id=com.google.android.play.games&hl=ko";

    Button _download;
    Button _gameEnd;


    protected override void Awake()
    {
        base.Awake();
        _download = transform.Find("Panel/Image/Button - Download").GetComponent<Button>();
        _gameEnd = transform.Find("Panel/Image/Button - GameEnd").GetComponent<Button>();
        _download.onClick.AddListener(() => Application.OpenURL(GPG_URL));
        _gameEnd.onClick.AddListener(() => Application.Quit());
    }
}