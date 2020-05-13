using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Playniax.Ignition.Framework;

public class lifes : MonoBehaviour
{
    private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Lives: " + PlayerData.Get(0).lives.ToString();
    }
}
