using NUnit.Framework;
using AltTester.AltTesterUnitySDK.Driver;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MyFirstTest
{
    public AltDriver altDriver;
    public AltObject gridObject;
    public int gridHeight;
    public int gridWidth;
    Dictionary<string, Vector2Int> initialTilePositions;

    [Test]
    public void Test()
    {
        try
        {
            altDriver.LoadScene("MainMenu");

            // Add your test logic here...

            Assert.Pass("Test passed successfully");
        }
        catch (Exception ex)
        {
            Debug.LogError("Test failed: " + ex.Message);
            throw;
        }
    }
    [Test]
    public void Test1()
    {
        try
        {
            altDriver.LoadScene("MainMenu");

            // Call a static method from an assembly
            altDriver.CallStaticMethod<string>("Namespace.TypeName", "MethodName", "AssemblyName", new object[] { /* parameters */ });

            // Add other test actions...

            Assert.Pass("Test passed successfully");
        }
        catch (Exception ex)
        {
            Debug.LogError("Test failed: " + ex.Message);
            throw;
        }
    }


    [OneTimeSetUp]
    public void SetUp()
    {
        altDriver = new AltDriver();
        altDriver.LoadScene("2048"); // Load the scene where the game board is located

        // Find the grid object in the scene
        gridObject = altDriver.FindObject(By.NAME, "Grid");
        if (gridObject != null)
        {
            // Fetch grid height and width
            gridHeight = gridObject.GetComponentProperty<int>("TileGrid", "height", "Assembly-CSharp");
            gridWidth = gridObject.GetComponentProperty<int>("TileGrid", "width", "Assembly-CSharp");
        }
        else
        {
            Assert.Fail("Grid object not found in the scene.");
        }

        initialTilePositions = new Dictionary<string, Vector2Int>();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        altDriver.Stop();
    }

    [Test]
    public void TestMoveTiles()
    {
        Assert.IsNotNull(gridObject, "Grid object not found.");

        // Capture initial tile positions
        CaptureInitialTilePositions();

        // Simulate moving tiles in different directions and check positions
        CallMoveTiles(Vector2Int.down, 0, 1, gridHeight - 2, -1);
        CallMoveTiles(Vector2Int.up, 0, 1, 1, 1);
        CallMoveTiles(Vector2Int.left, 1, 1, 0, 1);
        CallMoveTiles(Vector2Int.right, gridWidth - 2, -1, 0, 1);
    }

    private void CaptureInitialTilePositions()
    {
        var tiles = altDriver.FindObjects(By.NAME, "Tile");
        foreach (var tile in tiles)
        {
            var tileName = tile.GetComponentProperty<string>("Tile", "name", "Assembly-CSharp");
            var tilePos = tile.GetComponentProperty<Vector2Int>("Tile", "cell.coordinates", "Assembly-CSharp");
            initialTilePositions[tileName] = tilePos;
        }
    }

    private void CallMoveTiles(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        altDriver.CallStaticMethod<System.Object>("TileBoard", "MoveTiles", "Assembly-CSharp",
            new object[] { direction, startX, incrementX, startY, incrementY });

        Assert.IsTrue(CheckTilePositionsAfterMove(direction));
    }

    private bool CheckTilePositionsAfterMove(Vector2Int direction)
    {
        var tiles = altDriver.FindObjects(By.NAME, "Tile");
        foreach (var tile in tiles)
        {
            var tileName = tile.GetComponentProperty<string>("Tile", "name", "Assembly-CSharp");
            var currentTilePos = tile.GetComponentProperty<Vector2Int>("Tile", "cell.coordinates", "Assembly-CSharp");

            Vector2Int expectedPosition = initialTilePositions[tileName] + direction;

            if (currentTilePos != expectedPosition && currentTilePos != initialTilePositions[tileName])
            {
                return false;
            }
        }
        return true;
    }
}
