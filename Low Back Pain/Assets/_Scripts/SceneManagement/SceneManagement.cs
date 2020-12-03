using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public GameObject loader;
    public GameObject mainLoader;
   

    private void OnEnable()
    {
        mainLoader.SetActive(true);
    }

    public void LoadScene(string _scene)
    {
        StartCoroutine(Load(_scene));
    }


    public IEnumerator Load(string scene)
    {
        loader.SetActive(true);
       
       // yield return new WaitForSeconds(1f);
        //Load next scene 
        AsyncOperation result = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);

        result.allowSceneActivation = false;

        int progress;

        while (!result.isDone)
        {
            progress = (int)Mathf.Clamp01(result.progress / 0.9f);

            if (progress >= 0.9f)
            {
                //yield return new WaitForSeconds(1f);
                GetComponent<Fade>().enabled = true;
                GetComponent<Fade>().alpha = 1.0f;
                GetComponent<Fade>().DebutFade(-1);

                result.allowSceneActivation = true;
            }

            yield return null;
        }

        SetActiveScene(scene);

        yield return new WaitForSeconds(0.5f);

        loader.SetActive(false);
    }

   

    public void SetActiveScene(string _scene)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(_scene));
    }

    public void UnloadScene(string _scene)
    {
        SceneManager.UnloadSceneAsync(_scene);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
