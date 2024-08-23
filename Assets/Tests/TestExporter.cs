using UnityEditor;
using UnityEngine;
using UnityEditor.TestTools.TestRunner.Api;
using System.IO;

public class SimpleTestResultExporter : ICallbacks
{
    [MenuItem("Tools/Run Tests and Export Results")]
    public static void RunTestsAndExport()
    {
        var api = ScriptableObject.CreateInstance<TestRunnerApi>();
        var filter = new Filter()
        {
            testMode = TestMode.PlayMode, // Or TestMode.EditMode depending on what you need
        };

        api.Execute(new ExecutionSettings(filter));
        api.RegisterCallbacks(new SimpleTestResultExporter());
    }

    public void RunFinished(ITestResultAdaptor result)
    {
        // Export test results to an XML file
        string path = Path.Combine(Application.persistentDataPath, "TestResults.txt");

        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine("<TestResults>");
            ExportResults(result, writer);
            writer.WriteLine("</TestResults>");
        }

        Debug.Log($"Test results exported to {path}");
    }

    public void TestFinished(ITestResultAdaptor result) { }
    public void TestStarted(ITestAdaptor test) { }
    public void RunStarted(ITestAdaptor testsToRun) { }

    private void ExportResults(ITestResultAdaptor result, StreamWriter writer)
    {
        writer.WriteLine($"\t<Test name=\"{result.Name}\" result=\"{result.ResultState}\" duration=\"{result.Duration}\" />");

        foreach (var childResult in result.Children)
        {
            ExportResults(childResult, writer);
        }
    }
}
