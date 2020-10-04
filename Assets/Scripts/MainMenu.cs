using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] MainMenu mainMenu;
    [SerializeField] CycleController player1;
    [SerializeField] CycleController player2;
    [SerializeField] ScoreCanvas scoreCanvas;

    public void StartGame()
    {
        mainMenu.gameObject.SetActive(false);
        player1.gameObject.SetActive(true);
        player2.gameObject.SetActive(true);
        scoreCanvas.gameObject.SetActive(true);
    }
}
