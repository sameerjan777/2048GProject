using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
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
        // Load the MainMenu scene explicitly
        yield return SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);

        // Load the MainScene
        yield return SceneManager.LoadSceneAsync("2048", LoadSceneMode.Additive);

        // Find the GameManager in the scene
        gameManager = Object.FindObjectOfType<GameManager>();
        tileBoard = gameManager.board;
        gameOver = gameManager.gameOver;
        scoreText = gameManager.scoreText;
        highScoreText = gameManager.HighScoreText;

        // Debug logs for setup
        Debug.Log("GameManager is not null: " + (gameManager != null));
        Debug.Log("TileBoard is not null: " + (tileBoard != null));
        Debug.Log("GameOver CanvasGroup is not null: " + (gameOver != null));
        Debug.Log("Score TextMeshProUGUI is not null: " + (scoreText != null));
        Debug.Log("HighScore TextMeshProUGUI is not null: " + (highScoreText != null));

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
        bool isScoreReset = gameManager.score == 0;
        bool isScoreTextReset = scoreText.text == "0";

        // Debug logs
        Debug.Log("Score after NewGame is 0: " + isScoreReset);
        Debug.Log("ScoreText after NewGame is '0': " + isScoreTextReset);

        // Assert statements
        Assert.AreEqual(0, gameManager.score);
        Assert.AreEqual("0", scoreText.text);

        // Check that the high score is displayed correctly
        bool isHighScoreTextCorrect = highScoreText.text == "10";
        Debug.Log("HighScoreText after NewGame is '100': " + isHighScoreTextCorrect);
        Assert.AreEqual("10", highScoreText.text);

        // Check that the game over canvas group is hidden
        bool isGameOverAlphaZero = Mathf.Approximately(gameOver.alpha, 0f);
        bool isGameOverInteractableFalse = !gameOver.interactable;

        Debug.Log("GameOver alpha after NewGame is 0: " + isGameOverAlphaZero);
        Debug.Log("GameOver interactable after NewGame is false: " + isGameOverInteractableFalse);

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
        bool isGameOverAlphaOne = Mathf.Approximately(gameOver.alpha, 1f);
        bool isGameOverInteractableTrue = gameOver.interactable;

        Debug.Log("GameOver alpha after GameOver is 1: " + isGameOverAlphaOne);
        Debug.Log("GameOver interactable after GameOver is true: " + isGameOverInteractableTrue);

        // Assert statements
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
        bool isScoreUpdated = gameManager.score == 10;
        bool isScoreTextUpdated = scoreText.text == "10";

        Debug.Log("Score after IncreaseScore is 10: " + isScoreUpdated);
        Debug.Log("ScoreText after IncreaseScore is '10': " + isScoreTextUpdated);

        // Assert statements
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
        bool isScoreTextInitialStateCorrect = scoreText.text == "50";
        bool isHighScoreTextInitialStateCorrect = highScoreText.text == PlayerPrefs.GetInt("highScore", 0).ToString();
        bool isGameOverAlphaInitialStateCorrect = Mathf.Approximately(gameOver.alpha, 0.5f);
        bool isGameOverInteractableInitialStateCorrect = gameOver.interactable;

        Debug.Log("ScoreText after SetInitialState is '50': " + isScoreTextInitialStateCorrect);
        Debug.Log("HighScoreText after SetInitialState is correct: " + isHighScoreTextInitialStateCorrect);
        Debug.Log("GameOver alpha after SetInitialState is 0.5: " + isGameOverAlphaInitialStateCorrect);
        Debug.Log("GameOver interactable after SetInitialState is true: " + isGameOverInteractableInitialStateCorrect);

        // Assert statements
        Assert.AreEqual("50", scoreText.text);
        Assert.AreEqual(PlayerPrefs.GetInt("highScore", 0).ToString(), highScoreText.text);
        Assert.AreEqual(0.5f, gameOver.alpha);
        Assert.IsTrue(gameOver.interactable);

        yield return null;
    }
}
