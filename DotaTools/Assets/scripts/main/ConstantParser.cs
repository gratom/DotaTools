using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public static class ConstantParser
{
    public static Dictionary<int, DotaHero> ParseHeroes(string json)
    {
        json = json.Replace("null", "0");
        Dictionary<string, DotaHero> raw = JsonConvert.DeserializeObject<Dictionary<string, DotaHero>>(json);
        Dictionary<int, DotaHero> result = new Dictionary<int, DotaHero>(raw.Count);
        foreach (KeyValuePair<string, DotaHero> kv in raw)
        {
            if (int.TryParse(kv.Key, out int id))
            {
                if (kv.Value != null)
                {
                    kv.Value.id = id;
                }
                result[id] = kv.Value;
            }
        }
        return result;
    }
}