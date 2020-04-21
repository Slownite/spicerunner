using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameProgress
{
    public int gold;
    public float[] position;
    public int level;
    public GameProgress(Player player, int currentLevel)
    {
        gold = player.gold;
        level = currentLevel;
  
    }
}
