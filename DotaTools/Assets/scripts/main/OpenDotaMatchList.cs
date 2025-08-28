using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public static class OpenDotaMatchList
{
    private const string Base = "https://api.opendota.com/api/players/";

    [Serializable]
    private class PlayerMatchRow
    {
        public long match_id;
        public long start_time;
    }

    public static async Task<List<long>> GetPlayerMatchIdsLastYearAsync(string accountId)
    {
        const int days = 365;
        const int pageSize = 500;
        int offset = 0;

        List<long> ids = new List<long>(2048);

        while (true)
        {
            string url = $"https://api.opendota.com/api/players/{accountId}/matches" +
                         "?date=365" +
                         "&significant=0" +
                         "&project=match_id,start_time,game_mode" +
                         "&limit=500&offset=" + offset +
                         "&game_mode=18";
            Debug.Log($"[OpenDotaMatchList] Requesting {url}");
            string json = await GetAsync(url);

            List<PlayerMatchRow> rows = JsonConvert.DeserializeObject<List<PlayerMatchRow>>(json) ?? new List<PlayerMatchRow>();
            if (rows.Count == 0)
            {
                Debug.LogWarning($"[OpenDotaMatchList] SMTH wrong");
                break;
            }

            Debug.Log($"[OpenDotaMatchList] Found {rows.Count} matches, adding...");
            foreach (PlayerMatchRow r in rows)
            {
                ids.Add(r.match_id);
            }

            // защита от бесконечного цикла: если пришло меньше, чем страница — это конец
            if (rows.Count < pageSize)
            {
                Debug.Log($"[OpenDotaMatchList] Found {ids.Count} matches");
                Debug.Log($"[OpenDotaMatchList] Done");
                break;
            }

            offset += pageSize;
            await Task.Delay(3000);
        }

        return ids;
    }

    private static async Task<string> GetAsync(string url)
    {
        using UnityWebRequest req = UnityWebRequest.Get(url);
        req.timeout = 20;
        req.downloadHandler = new DownloadHandlerBuffer();

        UnityWebRequestAsyncOperation op = req.SendWebRequest();
        while (!op.isDone)
        {
            await Task.Yield();
        }

        if ((int)req.responseCode == 429)
        {
            Debug.LogWarning($"[OpenDotaMatchList] HTTP 429: Too many requests.");
            return null;
        }

        if (req.result != UnityWebRequest.Result.Success)
        {
            throw new Exception($"HTTP {(int)req.responseCode} {req.error} at {url}");
        }

        return req.downloadHandler.text;
    }
}