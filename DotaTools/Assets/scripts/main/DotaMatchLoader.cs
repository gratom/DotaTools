using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Net;
using Newtonsoft.Json;
using Tools;
using UnityEngine;
using UnityEngine.Networking;

[NamedBehavior]
public class DotaMatchLoader : MonoBehaviour
{
    public string matchId;

    public MatchSummary matchSummary;

    private const string matchSummaryUrl = "https://api.opendota.com/api/matches/";

    [ContextMenu("Download and parse")]
    public void DownloadAndParse()
    {
        string url = matchSummaryUrl + matchId;
        StartCoroutine(DownloadMatchCoroutine(url));
    }

    private IEnumerator DownloadMatchCoroutine(string url)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            req.timeout = 20;
            req.downloadHandler = new DownloadHandlerBuffer();

            UnityWebRequestAsyncOperation op = req.SendWebRequest();
            while (!op.isDone)
            {
                yield return null;
            }

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[DotaMatchLoader] HTTP {req.responseCode}: {req.error}\nURL: {url}");
                yield break;
            }

            string json = req.downloadHandler.text;

            // Простая проверка на "пустой"/короткий ответ
            if (string.IsNullOrEmpty(json) || json.Length < 2)
            {
                Debug.LogError("[DotaMatchLoader] Empty response");
                yield break;
            }

            try
            {
                matchSummary = JsonConvert.DeserializeObject<MatchSummary>(json);
                Debug.Log("[DotaMatchLoader] Parsed OK");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[DotaMatchLoader] JSON parse error: {ex.Message}");
            }
        }
    }
}