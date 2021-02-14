using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : BaseManager<SceneController>
{
    public void OpenScene(string targetScene, bool isPopup = false)
    {
        LoadSceneMode sceneLoadMode = (isPopup) ? LoadSceneMode.Additive
            : LoadSceneMode.Single;

        SceneManager.LoadSceneAsync(targetScene, sceneLoadMode);
    }

    public void CloseScene(string targetScene)
    {
        if (!SceneManager.GetSceneByName(targetScene).isLoaded)
        {
            Debug.LogWarningFormat("{0} is not loaded");
            return;
        }

        SceneManager.UnloadSceneAsync(targetScene);
    }
}
