using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{
    public BeatManager bm;

    private void Start() 
    {
    	StartCoroutine(ExecuteAfterDelay(1.0f)); 
    }

    IEnumerator ExecuteAfterDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime); 

        bm.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
