using System.Threading.Tasks;
using Net;
using Tools;
using UnityEngine;

[NamedBehavior]
public class DotaMatchLoader : MonoBehaviour
{
    [SerializeField] private string steamWebApiKey; // положи сюда свой Steam Web API Key
    [SerializeField] private ulong matchId;         // целевой match_id

    [ContextMenu("Load")]
    private async void Load()
    {
        try
        {
            MatchDetailsResult match = await Dota2Api.FetchMatchDetailsAsync(matchId, steamWebApiKey, 30);
            Debug.Log($"Match {match.match_id} | radiant_win={match.radiant_win} | duration={match.duration}s");

            if (match.players != null && match.players.Length > 0)
            {
                for (int i = 0; i < match.players.Length; i++)
                {
                    PlayerDetails p = match.players[i];
                    Debug.Log($"P{i} slot={p.player_slot} hero={p.hero_id} KDA={p.kills}/{p.deaths}/{p.assists} item0={p.item_0}");
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }
}