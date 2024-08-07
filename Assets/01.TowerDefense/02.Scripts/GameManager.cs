using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 싱글톤 인스턴스를 위한 정적 변수
    private static GameManager _instance;

    // GameManager 인스턴스를 반
    public static GameManager Instance
    {
        get
        {
            // 인스턴스가 없으면 찾거나 새로 생성
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("GameManager");
                    _instance = singletonObject.AddComponent<GameManager>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

    // Awake 메서드에서 싱글톤 패턴 적용
    private void Awake()
    {
        // 인스턴스가 존재하지 않으면 이 인스턴스를 설정하고 파괴되지 않도록 설정
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 인스턴스가 이미 존재하면 이 게임 오브젝트를 파괴
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    // 필요한 게임 관리 기능 추가
    public void StartGame()
    {
        Debug.Log("게임 시작 >>>>>>>> ");
        // 게임 시작 로직 추가
    }

    public void EndGame()
    {
        Debug.Log("게임 종료 >>>>>>>>");
        // 게임 종료 로직 추가
    }

    // 씬 이동 기능 추가
    public void LoadScene(string sceneName)
    {
        Debug.Log("씬이동 이름 : " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

}