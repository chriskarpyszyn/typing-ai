using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void ChangeAsteroidWord()
    {
        //Debug.Log("ChangeAsteroidWord");
        //GameObject newAsteroid = Instantiate(asteroidPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        //newAsteroid.transform.SetParent(level3GameCanvas.transform);
        //asteroids.Add(newAsteroid);
        ////position it randomly x 10, y 7
        //float randomX = Random.Range(-10f, 10f);
        //float randomY = Random.Range(-7f, 7f);
        //newAsteroid.transform.position = new Vector3(randomX, randomY, -1f);

        //Transform asteroidLetterParentTransform = newAsteroid.transform.Find("LetterParent");
        //GameObject asteroidLetterParent = asteroidLetterParentTransform.gameObject;

        //////add word to asteroid
        ////put the characters into an array so that we can do our input checks (repeated code)
        //wordCharArray = GetAndRemoveNextWord().ToLower().ToCharArray();
        //wordCharArraySize = wordCharArray.Length;
        //CreateGameObjectWordList(wordCharArray, asteroidLetterParent);
    }
}
