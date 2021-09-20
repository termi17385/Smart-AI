using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine;

public class BasicMenu : MonoBehaviour
{
   private void Awake() => Time.timeScale = 1;
   public void Changelevel(string _level) => SceneManager.LoadScene(_level);
   public void QuitGame()
   {
   #if UNITY_EDITOR
      EditorApplication.ExitPlaymode();
   #endif
      Application.Quit();
   }
}
