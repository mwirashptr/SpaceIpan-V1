using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameUI : NetworkBehaviour
{
    private Player player1;
    private Player player2;
    public TextMeshProUGUI scoreTextP1;
    public TextMeshProUGUI scoreTextP2;
    // Start method to initialize the players

    private void Awake()
    {
        var players = FindObjectsOfType<Player>();
        foreach (var player in players)
        {
            if (player.OwnerClientId == 0) player1 = player;
            else if (player.OwnerClientId == 1) player2 = player;
        }

        player1.score.OnValueChanged += UpdateScoreUI;
        player2.score.OnValueChanged += UpdateScoreUI;
    }

    private void Update()
    {
        if (player1 != null && player2 != null)
        {
            scoreTextP1.text = player1.score.Value.ToString();
            scoreTextP2.text = player2.score.Value.ToString();
        }
        else Debug.Log("Erorrroror");
    }

    // This function will be called when score changes
    private void UpdateScoreUI(int oldScore, int newScore)
    {
        // Update the UI Text elements
        if (player1 != null && player2 != null)
        {
            scoreTextP1.text = player1.score.Value.ToString();
            scoreTextP2.text = player2.score.Value.ToString();
        }
    }
}