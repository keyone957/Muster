using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
//Player 역할 관련 Role enum값으로 설정, 테스트용 역할부여
//최초 작성자: 홍원기
//수정자: 김기홍
//최종 수정일: 2024-02-05
public abstract class PlayerManager : MonoBehaviour
{
    public enum Role
    {
        None,
        Idol,
        Staff,
        Audience
    }

    protected Role playerRole;
    protected int playerID;
    protected int teamID = 0;
    protected int entryOrder = 0; // 0부터 시작

    protected Vector3 intermissionPosition = new Vector3(0, 100, 0);

    public Role GetRole() { return playerRole; }
    public void SetPlayerID(int id) { this.playerID = id; }
    public int GetPlayerID() { return playerID; }
    public void SetTeamID(int id) { this.teamID = id; }
    public int GetTeamID() { return teamID; }
    public void SetEntryOrder(int order) { this.entryOrder = order; }

    public abstract void MovePlayerToIntermission();
    public abstract void MovePlayerToStage();
}
