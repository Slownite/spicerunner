using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info : MonoBehaviour
{
    public int gold;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
