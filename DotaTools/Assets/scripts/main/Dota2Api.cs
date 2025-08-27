// Assets/Scripts/Net/Dota2Api.cs

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Net
{
    public static class Dota2Api
    {
        // Базовый эндпоинт официального API:
        // https://api.steampowered.com/IDOTA2Match_570/GetMatchDetails/V001/?key=...&match_id=...
        private const string BaseUrl = "https://api.steampowered.com/IDOTA2Match_570/GetMatchDetails/V001/";

        public static async Task<MatchDetailsResult> FetchMatchDetailsAsync(ulong matchId, string steamWebApiKey, int timeoutSeconds = 30)
        {
            if (string.IsNullOrEmpty(steamWebApiKey))
            {
                throw new ArgumentException("Steam Web API key is required.", nameof(steamWebApiKey));
            }

            StringBuilder sb = new StringBuilder(BaseUrl);
            sb.Append("?key=").Append(UnityWebRequest.EscapeURL(steamWebApiKey));
            sb.Append("&match_id=").Append(matchId.ToString());

            using (UnityWebRequest req = UnityWebRequest.Get(sb.ToString()))
            {
                req.downloadHandler = new DownloadHandlerBuffer();
                req.timeout = timeoutSeconds;

                // Подставляем "браузерные" заголовки на всякий случай
                req.SetRequestHeader("User-Agent", "UnityPlayer/2021 (Dota2MatchClient)");
                req.SetRequestHeader("Accept", "application/json");

#if UNITY_2020_2_OR_NEWER
                UnityWebRequestAsyncOperation op = req.SendWebRequest();
                while (!op.isDone) { await Task.Yield(); }
#else
                // На старых версиях Unity — через awaiter оболочку
                UnityWebRequestAsyncOperation op = req.SendWebRequest();
                while (!op.isDone) { await Task.Yield(); }
#endif

#if UNITY_2020_2_OR_NEWER
                bool hasNetworkError = req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError;
#else
                bool hasNetworkError = req.isNetworkError || req.isHttpError;
#endif
                if (hasNetworkError)
                {
                    throw new Exception($"HTTP error: {(int)req.responseCode} - {req.error}");
                }

                string json = req.downloadHandler.text;

                // Ответ имеет вид { "result": { ... поля матча ... } }
                MatchDetailsResponse parsed = JsonUtility.FromJson<MatchDetailsResponse>(json);
                if (parsed == null || parsed.result == null)
                {
                    throw new Exception("Malformed response or empty result.");
                }
                return parsed.result;
            }
        }
    }

    // ---------- DATA MODELS (JsonUtility-совместимые) ----------

    [Serializable]
    public class MatchDetailsResponse
    {
        public MatchDetailsResult result;
    }

    [Serializable]
    public class MatchDetailsResult
    {
        public ulong match_id;
        public bool radiant_win;
        public int duration; // в секундах
        public int start_time; // unix time (UTC)
        public ulong match_seq_num;
        public int tower_status_radiant;
        public int tower_status_dire;
        public int barracks_status_radiant;
        public int barracks_status_dire;
        public int cluster;
        public int first_blood_time;
        public int lobby_type;
        public int human_players;
        public int leagueid;
        public int positive_votes;
        public int negative_votes;
        public int game_mode;
        public int flags; // может отсутствовать
        public int engine;
        public int picks_bans_version; // может отсутствовать

        public PlayerDetails[] players; // 10 игроков

        // Иногда приходят дополнительные поля, оставляем зарезервированный блок:
        public string patch; // может отсутствовать
        public int region; // может отсутствовать
    }

    [Serializable]
    public class PlayerDetails
    {
        // Идентификаторы игрока и героя
        public uint account_id; // 32-bit steam id (если профиль открыт)
        public int player_slot; // 0-127 radiant, 128-255 dire
        public int hero_id;

        // Основная статистика
        public int kills;
        public int deaths;
        public int assists;
        public int last_hits;
        public int denies;
        public int gold_per_min;
        public int xp_per_min;
        public int level;
        public int leaver_status;

        // Нетворс/экономика (могут отсутствовать в старых матчах)
        public int gold;
        public int gold_spent;

        // Предметы (слоты 0-5) + рюкзак (0-2) + нейтральный
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

        // Скиллбилд по уровням: ability id, time (сек), level (порядковый номер апгрейда)
        public AbilityUpgrade[] ability_upgrades;

        // Доп. поля, которые иногда присутствуют
        public int hero_damage;
        public int hero_healing;
        public int tower_damage;
        public int scaled_hero_damage;
        public int scaled_tower_damage;
        public int scaled_hero_healing;
    }

    [Serializable]
    public class AbilityUpgrade
    {
        public int ability; // ID способности (не скилл-слот, а глобальный ability id)
        public int time; // секунда игры, когда взят уровень
        public int level; // порядковый номер апгрейда (1..25)
    }
}
#endif