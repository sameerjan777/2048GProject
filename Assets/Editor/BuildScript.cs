using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.IO;
using System;

public class Build
{
    [MenuItem("Build/Android Build - Development")]
    public static void BuildAndroid_Development()
    {
        BuildAndroid(BuildOptions.Development | BuildOptions.ConnectWithProfiler, "-dev");
    }

    [MenuItem("Build/Android Build - Release")]
    public static void BuildAndroid_Release()
    {
        BuildAndroid(BuildOptions.None,"-release");
    }

    public static void BuildAndroid(BuildOptions buildOptions = BuildOptions.None,string nameAppender = "")
    {
        // Check if the current platform is Android
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
        {
            bool switchToAndroid = EditorUtility.DisplayDialog(
                "Switch Platform",
                "The current platform is not set to Android. Switching the platform may take some time. Continue?",
                "Continue",
                "Cancel");

            if (!switchToAndroid)
            {
                Debug.Log("Build process canceled.");
                return;
            }

            // Switch to Android platform
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        }

        string[] scenePaths = EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();

        // Modify the paths and keystore information according to your project
        string keystorePath = "user.keystore";
        string keystorePassword = "123456";
        string keyaliasName = "signed";
        string keyaliasPassword = "123456";
        
        // Specify the APK output path
        string outputPath = "Builds/Android/"+ DateTime.Now.ToString("yyyyMMddHHmm") + "_" +PlayerSettings.productName +"_" + PlayerSettings.bundleVersion + nameAppender + ".apk";

        // Build settings
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = scenePaths,
            locationPathName = outputPath,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        // Set keystore information
        PlayerSettings.Android.keystoreName = keystorePath;
        PlayerSettings.Android.keystorePass = keystorePassword;
        PlayerSettings.Android.keyaliasName = keyaliasName;
        PlayerSettings.Android.keyaliasPass = keyaliasPassword;

        // Build the player
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build completed successfully: " + summary.totalSize + " bytes");
            // Open the folder where the build was created
            OpenFolder(outputPath);
        }
        else
        {
            Debug.LogError("Build failed with errors: " + summary.totalErrors);
        }
    }
    private static void OpenFolder(string path)
    {
        // Open the folder using the default file explorer
        string fullPath = Path.GetFullPath(path);
        fullPath = Path.GetDirectoryName(fullPath);
        Application.OpenURL("file://" + fullPath);
    }
}
