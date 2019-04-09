using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace zebaroo
{

    public class MenuHandler : MonoBehaviour
    {
        // public GameObject PauseScreen;
        public float LocalTimeScale;
        public bool isPaused = false;

        // Start is called before the first frame update
        void Start()
        {
            isPaused = false;
        }

        public void ToggleScreen(GameObject go)
        {
            Debug.Log(isPaused);
            isPaused = !isPaused;
            go.SetActive(isPaused);
            Time.timeScale = isPaused ? 0f : 1f;
        }

        public void LoadScene(string sceneName)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}
