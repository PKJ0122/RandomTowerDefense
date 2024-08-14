using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GPGSManager : MonoBehaviour
{
    const string FILENAME = "PlayerData.dat";
    const string UNSAVED_SAVE_DATA = "/storage/emulated/0/Android/data/com.KJParkCompany.DragonDefense/files/Dummy.json";
    const string SAVE_HASH = "/storage/emulated/0/Android/data/com.KJParkCompany.DragonDefense/files/Hash.json";


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
        else
        {
            StartCoroutine(C_NonGPGS());
        }
    }

    IEnumerator C_NonGPGS()
    {
        yield return new WaitForSeconds(1f);
        UIManager.Instance.Get<GPGSDownloadUI>().Show();
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
        string data = Encoding.UTF8.GetString(loadedData);

        if (data == "")
        {
            PlayerData.PlayerDataContainer = new PlayerDataContainer();
            //SceneManager.LoadScene("Tutorial");
            SceneManager.LoadScene("Lobby");
        }
        else
        {
            if (File.Exists(UNSAVED_SAVE_DATA))
            {
                byte[] unsaveData = File.ReadAllBytes(UNSAVED_SAVE_DATA);

                if (unsaveData.Length == 0)
                {
                    PlayerData.PlayerDataContainer = JsonUtility.FromJson<PlayerDataContainer>(data);
                    SceneManager.LoadScene("Lobby");
                    return;
                }
                else
                {
                    if (!File.Exists(SAVE_HASH))
                    {
                        PlayerData.PlayerDataContainer = JsonUtility.FromJson<PlayerDataContainer>(data);
                        SceneManager.LoadScene("Lobby");
                        return;
                    }

                    string currentHash = LocalDataChecker.ComputeChecksum(UNSAVED_SAVE_DATA);
                    string saveHash = File.ReadAllText(SAVE_HASH);

                    if (currentHash == saveHash)
                    {
                        string localData = Encoding.UTF8.GetString(unsaveData);
                        PlayerData.PlayerDataContainer = JsonUtility.FromJson<PlayerDataContainer>(localData);
                        SceneManager.LoadScene("Lobby");
                    }
                    else
                    {
                        PlayerData.PlayerDataContainer = JsonUtility.FromJson<PlayerDataContainer>(data);
                        SceneManager.LoadScene("Lobby");
                    }
                }
            }
            else
            {
                PlayerData.PlayerDataContainer = JsonUtility.FromJson<PlayerDataContainer>(data);
                SceneManager.LoadScene("Lobby");
            }
        }
    }
}
