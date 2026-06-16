using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DialogLog : MonoBehaviour
{
    public static DialogLog Instance;
    public static List<string[]> LogList = new(); 
    private void OnEnable()
    {
        Instance = this;
    }
    public static void Log(string dialog)
    {
        Log(string.Empty, dialog);
    }
    public static void Log(string character, string dialog)
    {
        string[] entry = { character, dialog };
        LogList.Add(entry);
        OutputLog();
    }
    public static List<string[]> GetLog()
    {
        return LogList;
    }
    public static void ClearLog()
    {
        LogList.Clear();
    }
    public static void ReplaceLast(string replacementDialog)
    {
        LogList[LogList.Count - 1][1] = replacementDialog;
        OutputLog();
    }
    public static void OutputLog()
    {
        foreach (string[] dialogEntry in GetLog())
        {
            Debug.Log($"\U0001F4AC {dialogEntry[0]}<({dialogEntry[1]})");
        }
    }
}
