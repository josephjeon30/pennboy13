using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
	public Transform spawnPos;
	public Transform exitPos;
	public float delay;
	public float timeElapsed;
	public LinkedList<Note> dead_pool;
	public LinkedList<Note> alive_pool;
	public string dirOfNote;
	public string typeOfNote;

	public NoteManager nm;

	public Image img;


    // Update is called once per frame
    void Update()
    {
    	if (timeElapsed < delay + 0.2f)
    	{
    		transform.position = Vector3.LerpUnclamped(spawnPos.position, exitPos.position, timeElapsed / delay);
        	timeElapsed += Time.deltaTime;
    	}
    	else
    	{
    		//nm.playMissSFX();
    		Die();
    	}
    }

    public float TimeLeft()
	{
		return delay - timeElapsed;
	}

    public void Die()
    {
    	alive_pool.Remove(this);
		dead_pool.AddLast(this);
		gameObject.SetActive(false);
    }

    public void ResetColor()
    {
    	img.color = Color.white;
    }

    public void GrayOut()
    {
    	img.color = Color.gray;
    }
}
