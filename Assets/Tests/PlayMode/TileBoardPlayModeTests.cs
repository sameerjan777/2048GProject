//using NUnit.Framework;
//using UnityEngine;
//using UnityEngine.TestTools;
//using System.Collections;

//[TestFixture]
//public class TileBoardPlayModeTests
//{
//    [UnityTest]
//    public IEnumerator TestSomeRuntimeLogic()
//    {
//        // Arrange
//        GameObject gameObject = new GameObject();
//        gameObject.AddComponent<Rigidbody>();

//        // Act
//        yield return new WaitForSeconds(1.0f);

//        // Assert
//        Assert.IsNotNull(gameObject.GetComponent<Rigidbody>());
//    }
//}
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManagerTests
{
    private GameManager gameManager;
    private TileBoard tileBoard;
    private CanvasGroup gameOver;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI highScoreText;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Output the names of all scenes in the build settings
        //LogSceneNames();

        // Load the MainMenu scene explicitly
        yield return SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);

        // Output the names of all scenes in the build settings after loading
        //LogSceneNames();

        // Load the MainScene
        yield return SceneManager.LoadSceneAsync("2048", LoadSceneMode.Additive);

        // Output the names of all scenes in the build settings after loading
        //LogSceneNames();

        // Find the GameManager in the scene
        gameManager = Object.FindObjectOfType<GameManager>();
        tileBoard = gameManager.board;
        gameOver = gameManager.gameOver;
        scoreText = gameManager.scoreText;
        highScoreText = gameManager.HighScoreText;

        // Remove duplicate Event Systems and Audio Listeners
        //RemoveDuplicateComponents<EventSystem>();
        //RemoveDuplicateComponents<AudioListener>();

        // Assert that essential components are not null
        Assert.IsNotNull(gameManager, "GameManager object is not found in the scene.");
        Assert.IsNotNull(tileBoard, "TileBoard object is not found in the scene.");
        Assert.IsNotNull(gameOver, "GameOver CanvasGroup is not found in the scene.");
        Assert.IsNotNull(scoreText, "Score TextMeshProUGUI is not found in the scene.");
        Assert.IsNotNull(highScoreText, "HighScore TextMeshProUGUI is not found in the scene.");

        // Start a new game for each test case
        gameManager.NewGame();
    }


    [UnityTest]
    public IEnumerator NewGame_ResetsScoreAndHighScoreText()
    {
        // Set up a high score for testing
        PlayerPrefs.SetInt("highScore", 100);

        // Start a new game
        gameManager.NewGame();

        // Check that the score is reset
        Assert.AreEqual(0, gameManager.score);
        Assert.AreEqual("0", scoreText.text);

        // Check that the high score is displayed correctly
        Assert.AreEqual("100", highScoreText.text);

        // Check that the game over canvas group is hidden
        Assert.AreEqual(0f, gameOver.alpha);
        Assert.IsFalse(gameOver.interactable);

        yield return null;
    }

    [UnityTest]
    public IEnumerator GameOver_ShowsGameOverCanvasGroup()
    {
        // Start a new game and then trigger game over
        gameManager.NewGame();
        gameManager.GameOver();
        yield return new WaitForSeconds(2f);
        // Check that the game over canvas group is shown

        Assert.AreEqual(1f, gameOver.alpha);
        Assert.IsTrue(gameOver.interactable);

        yield return null;
    }

    [UnityTest]
    public IEnumerator IncreaseScore_UpdatesScoreText()
    {
        // Start a new game
        gameManager.NewGame();

        // Increase the score
        gameManager.IncreaseScore(10);

        // Check that the score is updated
        Assert.AreEqual(10, gameManager.score);
        Assert.AreEqual("10", scoreText.text);

        yield return null;
    }

    [UnityTest]
    public IEnumerator SetInitialState_SetsCorrectValues()
    {
        // Set an initial state
        gameManager.SetInitialState(50, 0.5f, true);

        // Check that the initial state is set correctly
        Assert.AreEqual("50", scoreText.text);
        Assert.AreEqual(PlayerPrefs.GetInt("highScore", 0).ToString(), highScoreText.text); // Assuming no high score is saved
        Assert.AreEqual(0.5f, gameOver.alpha);
        Assert.IsTrue(gameOver.interactable);

        yield return null;
    }
}

