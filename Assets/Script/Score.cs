
using TMPro;
using UnityEngine;

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
        text.text = player.gold.ToString();
    }

}
