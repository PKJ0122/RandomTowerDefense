using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    static QuestDatas s_questDatas;
    public static QuestDatas QuestDatas
    {
        get
        {
            if (s_questDatas == null)
            {
                s_questDatas = Resources.Load<QuestDatas>("QuestDatas");
            }
            return s_questDatas;
        }
    }

    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        foreach (QuestBase item in QuestDatas.questDatas)
        {
            if (item.category.ToString() == sceneName)
            {
                item.Init();
            }
        }
    }
}
