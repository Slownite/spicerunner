
using TMPro;
using UnityEngine;
using Playniax.Ignition.Framework;

public class Score : MonoBehaviour
{

    public Player player;
    private TextMeshProUGUI text;
    // Update is called once per frame
    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        text.text = PlayerData.Get(0).scoreboard.ToString();
    }

}
