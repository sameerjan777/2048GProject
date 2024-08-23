using System;
using System.IO;
using System.Text;
using UnityEngine;

public class ConsoleLogManager : Singleton<ConsoleLogManager>
{
    public string CurrentFileName => currentFileName;
    private string currentFileName = "";
    public string PreviousFileName => previousFileName;
    private string previousFileName = "";
    private StreamWriter fileStream;
    private StringBuilder logStringBuilder = new StringBuilder();

    private void OnEnable()
    {
        currentFileName = Path.Combine(Application.persistentDataPath, "ConsoleLogs.txt");
        previousFileName = Path.Combine(Application.persistentDataPath, "PreviouseConsoleLogs.txt");
        Application.logMessageReceived += Handle_logMessageReceived;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= Handle_logMessageReceived;
    }

    protected override void OnStart()
    {
        base.OnStart();
        Init();
    }

    public void Init()
    {
        if (File.Exists(previousFileName))
        {
            File.Delete(previousFileName);
        }
        if (File.Exists(currentFileName))
        {
            File.Move(currentFileName, previousFileName);
        }
        fileStream = File.CreateText(currentFileName);
        Debug.Log("ConsoleLogManager Initialized: " + currentFileName);
    }

    private void Handle_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        string appendValue = "";
        if (type == LogType.Error || type == LogType.Exception)
            appendValue = stackTrace;
        string temp = $"{type.ToString()}#&{condition} {appendValue}|?";
        fileStream.Write(temp);
        logStringBuilder.Append(temp);
    }

    public string GetFormattedText(string inputString = "")
    {
        string newText = string.IsNullOrEmpty(inputString) ? logStringBuilder.ToString() : inputString;
        newText = newText.Replace("|?", "\n");
        newText = newText.Replace($"{LogType.Log.ToString()}#&", "<color=\"purple\">-> <color=\"white\">");
        newText = newText.Replace($"{LogType.Warning.ToString()}#&", "<color=\"purple\">-> <color=\"yellow\">");
        newText = newText.Replace($"{LogType.Error.ToString()}#&", "<color=\"purple\">-> <color=\"red\">");
        newText = newText.Replace($"{LogType.Exception.ToString()}#&", "<color=\"purple\">-> <color=\"orange\">");
        return newText;
    }

    // Other methods unchanged...

    protected override bool ShouldPersist()
    {
        return true;
    }
}
