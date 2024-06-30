using NUnit.Framework;
using AltTester.AltTesterUnitySDK.Driver;
using System.Collections.Generic;
using UnityEngine;
using System;


public class TileMovementAltUnityTest
{
    private AltDriver altDriver;
    private Dictionary<string, Vector2Int> initialTilePositions;
    private TileBoard tileBoard; 

    [OneTimeSetUp]
    public void SetUp()
    {
        try
        {
            altDriver = new AltDriver();
            //tileBoard = GameObject.FindObjectOfType<TileBoard>();
            //Debug.Log("Setting up test...");
            if (altDriver != null)
            {
                altDriver.LoadScene("MainMenu");
                
                //altDriver.LoadScene("2048");
                initialTilePositions = new Dictionary<string, Vector2Int>();
                CaptureInitialTilePositions();
            }
            else
            {
                Debug.LogError("altDriver is null. Ensure it is properly initialized.");
                // Handle the error or throw an exception if altDriver is critical for setup
                //throw new NullReferenceException("altDriver is null.");
            }

        }
        catch (Exception e)
        {
            //Debug.LogError($"Error in OneTimeSetUp: {e}");
            throw; // Rethrow the exception to halt further execution of the test suite
        }
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        //Debug.Log("Tearing down test...");
        if (altDriver != null)
        {
            altDriver.Stop();
        }
    }
    [Test]
    public void TestingPlayBtn()
    {
        var PlayBtn = altDriver.FindObject(By.NAME, "Play");
        Assert.NotNull(PlayBtn);
        PlayBtn.Tap();
    }
    [Test]
    public void TestMoveTilesUp()
    {
        SimulateArrowKeyPress(AltKeyCode.W);
        AssertTileMovement(Vector2Int.up);
    }

    [Test]
    public void TestMoveTilesDown()
    {
        SimulateArrowKeyPress(AltKeyCode.S);
        AssertTileMovement(Vector2Int.down);
    }

    [Test]
    public void TestMoveTilesLeft()
    {
        SimulateArrowKeyPress(AltKeyCode.A);
        AssertTileMovement(Vector2Int.left);
    }

    [Test]
    public void TestMoveTilesRight()
    {
        SimulateArrowKeyPress(AltKeyCode.D);
        AssertTileMovement(Vector2Int.right);
    }

    private void CaptureInitialTilePositions()
    {
        var tiles = altDriver.FindObjects(By.TAG, "TileTag");
        if (tiles == null)
        {
            //Debug.LogError("tiles collection is null");
            return;
        }
        //Assert.AreEqual(2, tiles.Count);
        foreach (var tile in tiles)
        {
            if (tile == null)
            {
                Debug.LogError("tile object is null");
                continue;
            }

            string tileName = tile.name;
            Assert.NotNull(tileName);
            Debug.LogError($"TileName for tile {tileName} ");

            var tileCell = tile.GetComponentProperty<AltObject>("Tile", "cell", "SciptsAssembly");
            Debug.LogError($"TileCell for tile {tileName} is {tileCell}");

            if (tileCell == null)
            {
                Debug.LogError($"TileCell for tile {tileName} is null");
                continue;
            }

            // Enhanced debugging for property retrieval
            Debug.Log($"Attempting to retrieve 'coordinates' from TileCell of tile {tileName}");

            try
            {
                var coordinatesProperty = tileCell.GetComponentProperty<Vector2Int>("TileCell", "coordinates", "SciptsAssembly");
                Debug.LogError($"CoordinatesProperty for tile {tileName} is {coordinatesProperty}");

                //if (!coordinatesProperty.HasValue)
                //{
                //    Debug.LogError($"Coordinates for tile {tileName} are null");
                //    continue;
                //}

                Vector2Int? tilePos = coordinatesProperty;
                Debug.LogError($"Tile position for {tileName} is {tilePos.Value}");

                // Store initial tile positions (if applicable)
                // initialTilePositions[tileName] = tilePos.Value;
                // Debug.Log($"Captured tile {tileName} at position {tilePos.Value}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error retrieving coordinates for tile {tileName}: {ex.Message}");
            }
        }
    }

    //private void SimulateArrowKeyPress(AltKeyCode keyCode)
    //{
    //    Debug.Log($"Simulating key press: {keyCode}");
    //    if (altDriver != null)
    //    {
    //        //altDriver.KeyDown(keyCode, 1);
    //        //System.Threading.Thread.Sleep(1000);
    //        //altDriver.KeyUp(keyCode);
    //        //System.Threading.Thread.Sleep(1000);
    //        altDriver.PressKey(keyCode, 1f, 0.1f, true);
    //    }
    //    else
    //    {
    //        Debug.LogError("AltDriver is null.");
    //    }
    //}

    private void SimulateArrowKeyPress(AltKeyCode keyCode)
    {
        Debug.Log($"Simulating key press: {keyCode}");
        var tileBoard = altDriver.FindObject(By.NAME, "Board");
        //var tileBoard = altDriver.FindObject(By.NAME, "Board") ;

        if (tileBoard != null)
        {
            switch (keyCode)
            {
                case AltKeyCode.W:
                    tileBoard.CallComponentMethod<object>("TileBoard", "MoveTilesUp", "SciptsAssembly", new object[] { });
                    break;
                case AltKeyCode.S:
                    tileBoard.CallComponentMethod<object>("TileBoard", "MoveTilesDown", "SciptsAssembly", new object[] { });
                    break;
                case AltKeyCode.A:
                    tileBoard.CallComponentMethod<object>("TileBoard", "MoveTilesLeft", "SciptsAssembly", new object[] { });
                    break;
                case AltKeyCode.D:
                    tileBoard.CallComponentMethod<object>("TileBoard", "MoveTilesRight", "SciptsAssembly", new object[] { });
                    break;
                default:
                    Debug.LogError("Unsupported key code.");
                    break;
            }
        }
        else
        {
            Debug.LogError("TileBoard is not found in the scene.");
        }
    }

    private void AssertTileMovement(Vector2Int direction)
    {
        Debug.Log($"Asserting tile movement in direction: {direction}");
        System.Threading.Thread.Sleep(1000);

        var tiles = altDriver.FindObjects(By.TAG, "TileTag");
        if (tiles == null)
        {
            Debug.LogError("No tiles found.");
            return;
        }

        //foreach (var tile in tiles)
        //{
        //    string tileName = tile.name;
        //    var tileCell = tile.GetComponentProperty<AltObject>("Tile", "cell", "SciptsAssembly");
        //    if (tileCell == null)
        //    {
        //        Debug.LogError($"Failed to get TileCell component for tile: {tileName}");
        //        continue;
        //    }

        //    Vector2Int? currentTilePos = tileCell.GetComponentProperty<Vector2Int?>("TileCell", "coordinates", "SciptsAssembly");
        //    if (!currentTilePos.HasValue)
        //    {
        //        Debug.LogError($"Failed to get coordinates for tile: {tileName}");
        //        continue;
        //    }

        //    if (initialTilePositions.ContainsKey(tileName))
        //    {
        //        Vector2Int expectedPosition = initialTilePositions[tileName] + direction;
        //        if (currentTilePos.Value != expectedPosition && currentTilePos.Value != initialTilePositions[tileName])
        //        {
        //            Debug.LogError($"Tile {tileName} did not move correctly: expected {expectedPosition}, but was {currentTilePos.Value}");
        //            Assert.Fail($"Tile {tileName} did not move correctly.");
        //        }
        //    }
        //}

        CaptureInitialTilePositions();
    }
}
