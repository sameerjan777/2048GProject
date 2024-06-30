using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;


public class BuildScript: MonoBehaviour
{
    [MenuItem("Custom/Build Android")]
    public static void BuildAndroid()
    {
        Debug.Log("Starting Android build...");

        string[] scenes = { "Assets/Scenes/MainMenu.unity" }; // Add your scene paths here
        string buildPath = "Builds/Android/build.apk";

        Debug.Log($"Building scenes: {string.Join(", ", scenes)}");
        Debug.Log($"Build path: {buildPath}");

        // Perform the build
        BuildReport report = BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.Android, BuildOptions.None);

        // Check for build success or failure
        if (report.summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded!");
        }
        else
        {
            Debug.LogError("Build failed!");
            foreach (var step in report.steps)
            {
                foreach (var message in step.messages)
                {
                    Debug.LogError(message.content);
                }
            }
        }
    }
}
