using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GPGSManager : MonoBehaviour
{
    public TMP_Text text;
    public Button button;


    public void Start()
    {
        button.onClick.AddListener(GPGS_LogIn);
    }

    public void GPGS_LogIn()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            text.text = "로그인 성공";
        }
        else
        {
            text.text = "로그인 실패";
        }
    }

    void SaveData()
    {

    }

    void OpenSaveGame()
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        savedGameClient.OpenWithAutomaticConflictResolution
    }
}
