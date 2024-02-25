using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Lobby UI, 테스트를 위해 임시로 사용
// 최초 작성자 : 김기홍
// 수정자 : 
// 최종 수정일 : 2024-01-08
public class TempLobbyUI : MonoBehaviour
{
    [SerializeField] NetworkManager networkManager;

    [Header("Lobby UI")]
    [SerializeField] Button joinLobbyBtn;

    [SerializeField] GameObject lobby;
    [SerializeField] TMP_Text roomCount;

    private void Awake()
    {
        lobby.SetActive(false);
    }

    public void UpdateRoomCount(int _roomCount)
    {
        roomCount.text = "Room Count :";
        roomCount.text += _roomCount;
    }
    //////////////////////////
    /// Join Lobby
    //////////////////////////
    public void JoinLobby()
    {
        networkManager.ConnectToLobby("Test99");
        joinLobbyBtn.gameObject.SetActive(false);
    }
    public void OnSuccessToJoinLobby()
    {
        lobby.SetActive(true);
    }
    public void OnFailedToJoinLobby()
    {
        joinLobbyBtn.gameObject.SetActive(true);
    }
}
