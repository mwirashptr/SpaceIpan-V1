using UnityEngine;
using Unity.Netcode;
using TMPro;

public class LeaderboardUI : NetworkBehaviour
{
    private Player player1;
    private Player player2;
    public TextMeshProUGUI scoreTextP1;
    public TextMeshProUGUI scoreTextP2;

    private void Awake()
    {
        var players = FindObjectsOfType<Player>();
        foreach (var player in players)
        {
            if (player.OwnerClientId == 0) player1 = player;
            else if (player.OwnerClientId == 1) player2 = player;
        }
        if (player1 != null && player2 != null)
        {
            if (player1.score.Value > player2.score.Value)
            {
                scoreTextP1.color = Color.green;
                scoreTextP2.color = Color.red;
            }
            else if (player1.score.Value < player2.score.Value)
            {
                scoreTextP1.color = Color.red;
                scoreTextP2.color = Color.green;
            }
            else
            {
                scoreTextP1.color = Color.white;
                scoreTextP2.color = Color.white;
            }
            scoreTextP1.text = "Player 1 : " + player1.score.Value.ToString();
            scoreTextP2.text = "Player 2 : " + player2.score.Value.ToString();
        }
    }

    public void OnClick()
    {
        Application.Quit();
    }
}