using System.Collections.Generic;
[System.Serializable]
public class MatchSummary
{
    public List<MatchPlayer> players;
    
    public bool radiant_win;
    public int duration;
    public long start_time;
    public long match_id;
    public int tower_status_radiant;
    public int tower_status_dire;
    public int barracks_status_radiant;
    public int barracks_status_dire;
    public int first_blood_time;
    public int game_mode;
    public int radiant_score;
    public int dire_score;
}

