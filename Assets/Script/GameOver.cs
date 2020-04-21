using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(waitCoroutine());
    }

    IEnumerator waitCoroutine()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(0);
    }
}
