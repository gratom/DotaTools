using System.Collections.Generic;
[System.Serializable]
public class DotaHero
{
    public int id;
    public string name;
    public string primary_attr;
    public string attack_type;
    public List<string> roles;
    public string img;
    public string icon;
    public int base_health;
    public float base_health_regen;
    public int base_mana;
    public float base_mana_regen;
    public float base_armor;
    public int base_mr;
    public int base_attack_min;
    public int base_attack_max;
    public int base_str;
    public int base_agi;
    public int base_int;
    public float str_gain;
    public float agi_gain;
    public float int_gain;
    public int attack_range;
    public int projectile_speed;
    public float attack_rate;
    public int base_attack_time;
    public float attack_point;
    public int move_speed;
    public float turn_rate;
    public bool cm_enabled;
    public int legs;
    public int day_vision;
    public int night_vision;
    public string localized_name;
}
