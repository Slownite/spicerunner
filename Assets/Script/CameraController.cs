using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    int DistanceAway = 10;
    private void Update()
    {
        Vector3 PlayerPOS = GameObject.Find("Player").transform.transform.position;
        GameObject.Find("Main Camera").transform.position = new Vector3(PlayerPOS.x, 0, PlayerPOS.z - DistanceAway);
    }
    
}