using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class NoteManager : MonoBehaviour
{
	public static event UnityAction catchFish;
	private Transform noteSpawnerTransform;
	private Transform noteReceiverTransform;
	private Transform noteCanvasTransform;

	private float delay;

	private LinkedList<NoteGroup> noteList;
	private LinkedList<Note> alive_pool = new LinkedList<Note>();
	private LinkedList<Note> dead_pool = new LinkedList<Note>();
	public GameObject notePrefab;

	private int currentBeat = 0;
	private int lastNote = 0;

	private int combo = 0;

	// 	time radius of judgements
	//	if no notes are "detected" during a click, no punishment or reward

	float PERFECT = 0.050f;
	float GREAT = 0.100f;
	float GOOD = 0.150f;
	float MISS = 0.300f;
	
	void Start()
    {
        noteSpawnerTransform = GameObject.Find("NoteSpawner").transform;
        noteReceiverTransform = GameObject.Find("NoteReceiver").transform;
        noteCanvasTransform = GameObject.Find("Image").transform;
        
        delay = 8 * 60f/200f;

        NoteGroup[] tempList = 
        {
        	new NoteGroup(0, "DEFAULT"),
        	new NoteGroup(4, "DEFAULT"),
        	new NoteGroup(4, "DEFAULT"),
        	new NoteGroup(8, "DEFAULT"),
        	new NoteGroup(4, "DEFAULT"),
        	new NoteGroup(4, "DEFAULT"),
        	new NoteGroup(8, "DEFAULT"),
        	new NoteGroup(4, "DEFAULT"),
        	new NoteGroup(4, "DEFAULT"),
        	new NoteGroup(8, "DEFAULT")
        };

        noteList = new LinkedList<NoteGroup>(tempList);
        
    }

    void Update()
    {
    	if (Input.GetKeyDown("space"))
        {
            bool note_processed = false;
            for (LinkedListNode<Note> node = alive_pool.First; !note_processed && node != null; node = node.Next)
            {
            	if (Mathf.Abs(node.Value.TimeLeft()) < MISS)
            	{
            		float dev = Mathf.Abs(node.Value.TimeLeft());
            		node.Value.Die();
            		note_processed = true;
            		if (dev <= GOOD)
            		{
            			combo++;
            			if (dev <= PERFECT)
            			{
            				Debug.Log("PERFECT");
            			}
            			else if (dev <= GREAT)
            			{
            				Debug.Log("GREAT");
            			}
            			else
            			{
            				Debug.Log("GOOD");
            			}
            		}
            		else
            		{
            			combo = 0;
            			Debug.Log("MISS");
            		}
            	}
            } 
        }
    }



    public void Proceed()
    {
    	if (noteList.Count > 0 && currentBeat == lastNote +  noteList.First.Value.delay)
    	{
    		Spawn(noteList.First.Value.typeOfNote);
    		noteList.RemoveFirst();
    		lastNote = currentBeat;
    	}
		if (noteList.Count <= 0 && alive_pool.Count <= 0)
        {
			Debug.Log("OVER!!");
            catchFish?.Invoke();
        }

    	currentBeat++;
    }

    public void Spawn(string typeOfNote){
    	Note note;
    	if (dead_pool.Count > 0) 
    	{
    		note = dead_pool.First.Value;
    		dead_pool.RemoveFirst();
    		note.gameObject.SetActive(true);
    	}
    	else 
    	{
    		note = Instantiate(notePrefab, noteCanvasTransform).GetComponent<Note>();
    		note.dead_pool = dead_pool;
    		note.alive_pool = alive_pool;
    		note.spawnPos = noteSpawnerTransform;
        	note.exitPos = noteReceiverTransform;
    	}

    	alive_pool.AddLast(note);
    	note.timeElapsed = 0;
        note.delay = delay;
    }

}

public struct NoteGroup
{
	public NoteGroup(int _delay, string _typeOfNote)
	{
		delay = _delay;
		typeOfNote = _typeOfNote;
	}

	public int delay {get;}
	public string typeOfNote {get;}
}
