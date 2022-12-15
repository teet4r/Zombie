using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

// 점수와 게임 오버 여부를 관리하는 게임 매니저
public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        var randomSpawnPos = Random.insideUnitSphere * 5f;
        randomSpawnPos.y = 0f;

        // 네트워크상의 모든 클라이언트에서 실행.
        // 해당 게임 오브젝트의 주도권은 생성 메서드를 직접 실행한 클라이언트에 있음.
        PhotonNetwork.Instantiate(playerPrefab.name, randomSpawnPos, Quaternion.identity);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

    // 점수를 추가하고 UI 갱신
    public void AddScore(int newScore)
    {
        // 게임 오버가 아닌 상태에서만 점수 증가 가능
        if (isGameover) return;
        // 점수 추가
        score += newScore;
        // 점수 UI 텍스트 갱신
        UIManager.instance.UpdateScoreText(score);
    }

    // 게임 오버 처리
    public void EndGame()
    {
        // 게임 오버 상태를 참으로 변경
        isGameover = true;
        // 게임 오버 UI를 활성화
        UIManager.instance.SetActiveGameoverUI(true);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 로컬 오브젝트라면 쓰기 부분이 실행됨
        if (stream.IsWriting)
            stream.SendNext(score);
        // 리모트 오브젝트라면 읽기 부분이 실행됨
        else
            UIManager.instance.UpdateScoreText((int)stream.ReceiveNext());
    }

    public static GameManager instance = null;
    public GameObject playerPrefab;
    public bool isGameover { get; private set; } // 게임 오버 상태

    int score = 0; // 현재 게임 점수
}