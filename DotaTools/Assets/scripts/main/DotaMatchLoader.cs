using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Net;
using Newtonsoft.Json;
using Tools;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

[NamedBehavior]
public class DotaMatchLoader : MonoBehaviour
{
    public int awaitTimeSec = 3;

    public string playerID = "";

    public List<string> matchIds;

    public List<MatchSummary> matches;

    private const string matchSummaryUrl = "https://api.opendota.com/api/matches/";

    [ContextMenu("Download matches list")]
    public async void DownloadMatches()
    {
        Debug.Log("Downloading matches...");
        matchIds = (await OpenDotaMatchList.GetPlayerMatchIdsLastYearAsync(playerID)).Select(x => x.ToString()).ToList();
    }

    [ContextMenu("Download and parse matches")]
    public async void DownloadAndParse()
    {
        Debug.Log("Downloading and parsing...");

        int countSuccess = 0;
        int countFail = 0;

        for (int i = 0; i < matchIds.Count; i++)
        {
            Debug.Log($"Downloading match [{i + 1}/{matchIds.Count}]   [{(i + 1) / matchIds.Count * 100:0.00}%]");
            string url = matchSummaryUrl + matchIds[i];
            MatchSummary match = await DownloadMatchAsync(url);
            if (match != null)
            {
                Debug.Log($"Match with ID [{matchIds[i]}] - OK");
                countSuccess++;
                matches.Add(match);
            }
            else
            {
                Debug.Log($"Match with ID [{matchIds[i]}] - FAIL");
                countFail++;
            }
            await Task.Delay(awaitTimeSec * 1000);
        }
        Debug.Log($"Done. Success: {countSuccess}, Fail: {countFail}");
    }

    private async Task<MatchSummary> DownloadMatchAsync(string url)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            req.timeout = 20;
            req.downloadHandler = new DownloadHandlerBuffer();

            UnityWebRequestAsyncOperation op = req.SendWebRequest();
            while (!op.isDone)
            {
                await Task.Yield();
            }

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[DotaMatchLoader] HTTP {req.responseCode}: {req.error}\nURL: {url}");
                return null;
            }

            string json = req.downloadHandler.text;

            if (string.IsNullOrEmpty(json) || json.Length < 2)
            {
                Debug.LogError("[DotaMatchLoader] Empty response");
                return null;
            }

            try
            {
                MatchSummary r = JsonConvert.DeserializeObject<MatchSummary>(json);
                Debug.Log($"[DotaMatchLoader] JSON parse success");
                return r;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[DotaMatchLoader] JSON parse error: {ex.Message}");
            }
        }
        return null;
    }
}