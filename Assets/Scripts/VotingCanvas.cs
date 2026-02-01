using TMPro;
using Unity.Netcode;
using UnityEngine;

public class VotingCanvas : NetworkBehaviour
{
    RoundManager roundManager;
    RoundManager RoundManager
    {
        get
        {
            if(roundManager == null)
            {
                roundManager = RoundManager.instance;
            }
            return roundManager;
        }
    }
    [SerializeField] TMP_Text themeText;
    [SerializeField] TMP_Text promptText;

    [SerializeField] PlayerTile playerTilePrefab; 

    PlayerTile[] tileChildren;
    [SerializeField] Transform tileChildrenParent;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }
    public void StartVotingRound()
    {
        var players = RoundManager.session.Players;

        if (tileChildren == null)
        {
            tileChildren = new PlayerTile[players.Count];
            for(int i = 0; i < tileChildren.Length; i++)
            {
                tileChildren[i] = Instantiate(playerTilePrefab, tileChildrenParent);
            }
        }
        for(int i = 0; i< tileChildren.Length; ++i)
        {
            var response = PlayerDataManager.instance.GetResponse(players[i].Id);
            tileChildren[i].SetDetails(players[i].Id, response[0], response[1], response[2], response[3]);
        }
        
    }
}
