
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
  public void LoadScenes()
    {
        SceneManager.LoadScene("2048");
    }
}
