using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    private const string PLAYER_ID_PREFIX = "Player ";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer(string NetID, Player player) {
        string playerId = PLAYER_ID_PREFIX + NetID;
        players.Add(playerId, player);

        player.transform.name = playerId;
        Transform head = player.transform.Find("Sphere");
        head.name = "Head " + NetID;
        Transform body = player.transform.Find("Capsule");
        body.name = "Body " + NetID;
    }

    public static void UnRegisterPlayer(string playerId) {
        players.Remove(playerId);
    }

    public static Player GetPlayer(string playerId) {
        return players[playerId];
    }

}
