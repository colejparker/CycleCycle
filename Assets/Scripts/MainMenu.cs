using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] MainMenu mainMenu;
    [SerializeField] CycleController player1;
    [SerializeField] CycleController player2;
    [SerializeField] ScoreCanvas scoreCanvas;

    [SerializeField] TMP_InputField player1Input;
    [SerializeField] TMP_InputField player2Input;


    public void StartGame()
    {
        mainMenu.gameObject.SetActive(false);
        player1.gameObject.SetActive(true);
        player2.gameObject.SetActive(true);
        player1.nameString = player1Input.text;
        player2.nameString = player2Input.text;
        scoreCanvas.gameObject.SetActive(true);
    }
}
