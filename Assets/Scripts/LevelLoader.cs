using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    public Animator transition;

    [SerializeField]
    private float transitionTime = 1f;


    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(PlayAnimationCoroutine(sceneIndex));
    }

    private IEnumerator PlayAnimationCoroutine(int sceneIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime); //todo-ck can i get transition time from animation?

        SceneManager.LoadScene(sceneIndex);
    }
}
