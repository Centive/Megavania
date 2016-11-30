using UnityEngine;
using System.Collections;

public class ShowDebugLogs : MonoBehaviour
{

    static string myLog = "";
    public int logCap = 20;
    private int curLogs = 0;
    private string output = "";


    void OnEnable()
    {
        Application.RegisterLogCallback(HandleLog);
    }


    void OnDisable()
    {
        // Remove callback when object goes out of scope
        Application.RegisterLogCallback(null);
    }


    void HandleLog(string logString, string stackTrace, LogType type)
    {
        //reset logs if hit cap
        if (curLogs == logCap)
        {
            myLog = "";
            curLogs = 0;
        }

        //
        myLog += "\n [" + type + "] : " + logString;
        curLogs++;
    }


    void OnGUI()
    {
        //display logs
        GUILayout.Label(myLog);
    }
}
