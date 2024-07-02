using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsoleManager : Singleton<DebugConsoleManager>
{
    [SerializeField]
    public TMP_Text console;

    public static void Log(string message)
    {
        GetInstance().InternalLog(message);
    }

    private void InternalLog(string message)
    {
        if (console == null)
        {
            return;
        }
        console.text += "\n" + "> " + message;
    }

}
