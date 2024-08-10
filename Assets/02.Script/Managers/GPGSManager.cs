using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GPGSManager : MonoBehaviour
{
    private const string FILENAME = "PlayerData.dat";


    void Awake()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    public void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            LoadData();
        }
    }

    void LoadData()
    {
        OpenLoadGame();
    }

    void OpenLoadGame()
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        savedGameClient.OpenWithAutomaticConflictResolution(FILENAME,
                                                            DataSource.ReadCacheOrNetwork,
                                                            ConflictResolutionStrategy.UseLastKnownGood,
                                                            LoadGameData);
    }
    void LoadGameData(SavedGameRequestStatus status, ISavedGameMetadata data)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        if (status == SavedGameRequestStatus.Success)
        {
            savedGameClient.ReadBinaryData(data, OnSaveGameDataRead);
        }
    }

    void OnSaveGameDataRead(SavedGameRequestStatus status, byte[] loadedData)
    {
        string data = System.Text.Encoding.UTF8.GetString(loadedData);

        if (data == "")
        {
            PlayerData.PlayerDataContainer = new PlayerDataContainer();
            //SceneManager.LoadScene("Tutorial");
            SceneManager.LoadScene("Lobby");
        }
        else
        {
            PlayerData.PlayerDataContainer = JsonUtility.FromJson<PlayerDataContainer>(data);
            SceneManager.LoadScene("Lobby");
        }
    }
}
