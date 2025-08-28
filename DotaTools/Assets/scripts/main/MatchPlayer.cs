[System.Serializable]
public class MatchPlayer
{
    public long account_id;
    public int player_slot;
    public int team_number;
    public int team_slot;
    public int hero_id;
    public int hero_variant;

    public int item_0;
    public int item_1;
    public int item_2;
    public int item_3;
    public int item_4;
    public int item_5;

    public int backpack_0;
    public int backpack_1;
    public int backpack_2;

    public int item_neutral;
    public int item_neutral2;

    public int kills;
    public int deaths;
    public int assists;
    public int leaver_status;
    public int last_hits;
    public int denies;
    public int gold_per_min;
    public int xp_per_min;
    public int level;
    public int net_worth;

    public int aghanims_scepter;
    public int aghanims_shard;
    public int moonshard;

    public int hero_damage;
    public int tower_damage;
    public int hero_healing;

    public int gold;
    public int gold_spent;

    public int[] ability_upgrades_arr;

    public string personaname;
    public string name;
    public string last_login;
    public string rank_tier;

    public bool is_subscriber;
    public bool radiant_win;

    public long start_time;
    public int duration;
    public int cluster;
    public int lobby_type;
    public int game_mode;
    public bool is_contributor;
    public int patch;
    public int region;
    public bool isRadiant;

    public int win;
    public int lose;

    public int total_gold;
    public int total_xp;
    public float kills_per_min;
    public float kda;
    public int abandons;

    public Benchmarks benchmarks;
}

[System.Serializable]
public class Benchmarks
{
    public BenchValue gold_per_min;
    public BenchValue xp_per_min;
    public BenchValue kills_per_min;
    public BenchValue last_hits_per_min;
    public BenchValue hero_damage_per_min;
    public BenchValue hero_healing_per_min;
    public BenchValue tower_damage;
}

[System.Serializable]
public class BenchValue
{
    public float raw;
    public float pct;
}