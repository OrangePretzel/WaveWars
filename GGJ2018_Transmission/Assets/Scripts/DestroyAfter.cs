using UnityEngine;
using System.Collections;

public class DestroyAfter : MonoBehaviour
{
    public float timeToDestroy = 1;
    
    void Start()
    {
        StartCoroutine(DelayToDestroy());
    }

    IEnumerator DelayToDestroy()
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(gameObject);
    }

}
