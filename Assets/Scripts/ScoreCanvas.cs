using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreCanvas : MonoBehaviour
{

    [SerializeField] RectTransform PlayAgain;
    [SerializeField] TextMeshProUGUI playerWinsText;
    [SerializeField] CycleController player1;
    [SerializeField] CycleController player2;

    public void PlayAgainClick()
    {
        PlayAgain.gameObject.SetActive(false);
        player1.Reset();
        player2.Reset();
    }

    public void PlayerLoses(CycleController losingPlayer)
    {
        print("Calling:");
        PlayAgain.gameObject.SetActive(true);
        player1.Freeze();
        player2.Freeze();
        if (losingPlayer == player1)
        {
            playerWinsText.text = player2.nameString + " Wins!";
            playerWinsText.color = player2.playerMaterial.color;
        } else
        {
            playerWinsText.text = player1.nameString + " Wins!";
            playerWinsText.color = player1.playerMaterial.color;

        }

    }
}
