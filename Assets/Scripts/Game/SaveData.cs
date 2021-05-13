using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    
    Vector3Int playerPosition;

    public void Initialize(Vector3Int playerPos){
        playerPosition = playerPos;
    }

    public void SaveState(Vector3Int playerPos){
        playerPosition = playerPos;
    }

    public Vector3Int getPlayerPosition(){
        return playerPosition;
    }
}
