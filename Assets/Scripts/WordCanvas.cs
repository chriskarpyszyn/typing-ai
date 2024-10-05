using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordCanvas : MonoBehaviour
{
    
    public GameObject Create(float x, float y, float z)
    {
        Vector3 pos = new Vector3(x,y,z);
        return Instantiate(this.gameObject, pos, Quaternion.identity);
    }
    
    public void DestroyCanvas(GameObject gameObject)
    {
        Destroy(gameObject);
    }
}
