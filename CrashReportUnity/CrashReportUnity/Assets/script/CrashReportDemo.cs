using UnityEngine;
using System.Collections;

public class CrashReportDemo : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

        var crashReport = new CrashReportUnity();

        string gameId = "71";
        string cpId = "5";
        string cpKey = "tLJy&sk3k94Q";
        string szId = "sz_123";
        string userId = "11";
        crashReport.init(gameId, cpId, cpKey, szId, userId);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
