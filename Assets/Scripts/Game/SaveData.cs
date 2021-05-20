using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    
    Vector3Int playerPosition;
    int playerPowers;

    public void Initialize(Vector3Int playerPos){
        playerPosition = playerPos;
    }

    public void SaveState(Vector3Int playerPos, int powerStatus){
        playerPosition = playerPos;
        playerPowers = powerStatus;
        Debug.Log("Powers saved as: " + powerStatus);
    }

    public Vector3Int GetPlayerPosition(){
        return playerPosition;
    }

    public int GetPlayerPowers(){
        Debug.Log("Power loaded as: " + playerPowers);
        return playerPowers;
    }
}
