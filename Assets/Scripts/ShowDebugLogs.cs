using UnityEngine;
using System.Collections;

public class ShowDebugLogs : MonoBehaviour
{

    static string myLog = "";
    private string output = "";
    private string stack = "";


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
        output = logString;
        stack = stackTrace;
        myLog += "\n" + output;
    }


    void OnGUI()
    {
        myLog = GUI.TextField(new Rect(0, 0, 200, 300), myLog);
    }
}
