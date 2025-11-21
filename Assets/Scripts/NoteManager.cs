using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NoteManager : MonoBehaviour
{
	public static event UnityAction catchFish;
	public static event UnityAction loseFish;
	[HideInInspector] public Transform noteSpawnerTransform;
	[HideInInspector] public Transform noteReceiverTransform;
	private Transform noteCanvasTransform;
	private Transform noteCanvasTransformLINES;

	public AudioSource audioSource;
	public AudioClip hitSound;
	public AudioClip missSound;

	private float delay;

	private LinkedList<NoteGroup> noteList;
	public LinkedList<Note> alive_pool_LINES = new LinkedList<Note>();
	public LinkedList<Note> alive_pool = new LinkedList<Note>();
	private LinkedList<Note> dead_pool = new LinkedList<Note>();
	private LinkedList<Note> dead_pool_LINES = new LinkedList<Note>();
	public GameObject notePrefab;
	public GameObject linePrefab;

	private int currentBeat = 0;

	private int combo = 0;
	private int score = 0;
	private int totalNotesCount = 0;

	[SerializeField] private GameObject connector;
	private List<RectTransform> connectorTransforms = new List<RectTransform>();

	// 	time radius of judgements
	//	if no notes are "detected" during a click, no punishment or reward

	float PERFECT = 0.050f;
	float GREAT = 0.100f;
	float GOOD = 0.150f;
	float MISS = 0.300f;

	[SerializeField] public NoteGroup[] noteGroup;

	public TouchDraw td;

	public DebugTextManager dtm;
	
	void Start()
    {
        noteSpawnerTransform = GameObject.Find("NoteSpawner").transform;
        noteReceiverTransform = GameObject.Find("NoteReceiver").transform;
		noteCanvasTransform = GameObject.Find("Image").transform;
		noteCanvasTransformLINES = GameObject.Find("Image").transform;
        
        delay = 8 * 60f/200f;

		noteList = new LinkedList<NoteGroup>(noteGroup);
		totalNotesCount = noteList.Count;
		//DrawConnectors();
    }

    void Update()
    {
    	if (LeftClicked() || RightClicked())
        {
        	td.StartLine();
            MouseDownJudge();
        }
        if (LeftUnclicked() || RightUnclicked())
        {
        	string dir = td.FinishLine();
        	MouseUpJudge(dir);
        }
        dtm.UpdateText("Combo: " + combo);
    }

    public void DrawConnectors()
    {
    	int counter = 0;
    	for (LinkedListNode<Note> node = alive_pool.First; node != null; node = node.Next)
    	{
    		if (node.Value.typeOfNote == "StartHold")
    		{
    			while (counter >= connectorTransforms.Count)
    			{
    				connectorTransforms.Add(Instantiate(connector, noteCanvasTransform).GetComponent<Image>().GetComponent<RectTransform>());
    			}
    			connectorTransforms[counter].offsetMin = new Vector2(node.Value.transform.position.x, connectorTransforms[counter].offsetMin.y);
    			node = node.Next;
    			if (node != null)
    			{
	    			if (node.Value)
	    			{
	    				connectorTransforms[counter].offsetMax = new Vector2(node.Value.transform.position.x, connectorTransforms[counter].offsetMin.y);
	    			}
	    			else
	    			{
	    				connectorTransforms[counter].offsetMax = new Vector2(noteSpawnerTransform.transform.position.x, connectorTransforms[counter].offsetMin.y);
	    			}
	    		}
    		}
    		else
    		{
    			while (counter >= connectorTransforms.Count)
    			{
    				
    				connectorTransforms.Add(Instantiate(connector, noteCanvasTransform).GetComponent<Image>().GetComponent<RectTransform>());
    			}
	    		connectorTransforms[counter].offsetMin = new Vector2(noteReceiverTransform.position.x, connectorTransforms[counter].offsetMin.y);
	    		connectorTransforms[counter].offsetMax = new Vector2(node.Value.transform.position.x, connectorTransforms[counter].offsetMax.y);
	    	}
	    	counter++;
    	}
    	connectorTransforms = connectorTransforms.GetRange(0, counter);
    }

    public void MouseUpJudge(string dir)
    {
    	bool note_processed = false;
        for (LinkedListNode<Note> node = alive_pool.First; !note_processed && node != null; node = node.Next)
        {
        	if (node.Value.typeOfNote == "StopHold" && Mathf.Abs(node.Value.TimeLeft()) < MISS)
        	{
        		float dev = Mathf.Abs(node.Value.TimeLeft());
        		node.Value.Die();
        		note_processed = true;	
        		if (dev <= GOOD)
        		{
        			playHitSFX();
        			combo++;
        			if (dev <= PERFECT)
        			{
						Debug.Log("PERFECT");
						score += 10;
        			}
        			else if (dev <= GREAT)
        			{
						Debug.Log("GREAT");
						score += 6;
        			}
        			else
        			{
						Debug.Log("GOOD");
						score += 3;
        			}
        			if (dir == node.Value.dirOfNote)
        			{
        				Debug.Log("MATCH!");
        			}
        			else
        			{
        				Debug.Log("DOES NOT MATCH!");
        				combo = 0;
        				playMissSFX();
        			}
        		}
        		else
        		{
        			combo = 0;
        			playMissSFX();
        			Debug.Log("MISS");
        		}
        	}
        } 
    }

    public void MouseDownJudge()
    {
    	bool note_processed = false;
        for (LinkedListNode<Note> node = alive_pool.First; !note_processed && node != null; node = node.Next)
        {
        	if (node.Value.typeOfNote == "StartHold" && Mathf.Abs(node.Value.TimeLeft()) < MISS)
        	{
        		float dev = Mathf.Abs(node.Value.TimeLeft());
        		node.Value.Die();
        		note_processed = true;	
        		if (dev <= GOOD)
        		{
        			playHitSFX();
        			combo++;
        			if (dev <= PERFECT)
        			{
						Debug.Log("PERFECT");
						score += 10;
        			}
        			else if (dev <= GREAT)
        			{
						Debug.Log("GREAT");
						score += 6;
        			}
        			else
        			{
						Debug.Log("GOOD");
						score += 3;
        			}
        		}
        		else
        		{
        			combo = 0;
        			playMissSFX();
        			//Debug.Log("MISS");
        		}
        	}
        } 
    }

    public bool LeftClicked()
    {
    	return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.W);
    }

    public bool RightClicked()
    {
    	return Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.D);
    }

    public bool LeftUnclicked()
    {
    	return Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.W);
    }

    public bool RightUnclicked()
    {
    	return Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.D);
    }

    public void Proceed()
    {
    	if (noteList.Count > 0 && currentBeat == noteList.First.Value.startTime)
		{
			Debug.Log(noteList.Count);
			float timeToNextNote = 0f;
			if(noteList.First.Next != null && noteList.Count%2 == 0)
            {
				Debug.Log("MAKE LINE");
				timeToNextNote = noteList.First.Next.Value.startTime - noteList.First.Value.startTime;
				Spawn("RIGHT", noteList.First.Value.typeOfNote, timeToNextNote, true);
            }
    		Spawn(noteList.First.Value.dirOfNote, noteList.First.Value.typeOfNote, timeToNextNote, false);
    		noteList.RemoveFirst();
    	}
		if (noteList.Count <= 0 && alive_pool.Count <= 0)
		{
			if (score > (totalNotesCount*.8*3))
            {
                catchFish?.Invoke();
            }
			else
            {
				loseFish?.Invoke();
            }
    
        }

    	currentBeat++;
    }


    public void Spawn(string dirOfNote, string typeOfNote, float timeToNext, bool isLine)
	{
		if (typeOfNote == "StartHold" && isLine)
		{
			Note newLine;
			newLine = Instantiate(linePrefab, noteCanvasTransformLINES).GetComponent<Note>();
			newLine.nm = this;
			newLine.dead_pool = dead_pool;
			newLine.alive_pool = alive_pool_LINES;
			newLine.spawnPos = noteSpawnerTransform;
			newLine.exitPos = noteReceiverTransform;
			alive_pool_LINES.AddLast(newLine);
			newLine.timeElapsed = 0;
			newLine.delay = delay;
			newLine.dirOfNote = "RIGHT";
			newLine.typeOfNote = typeOfNote;
			newLine.ResetColor();
			newLine.posOffset = timeToNext * 20f;
			Vector3 scaleVec = new Vector3(timeToNext * .6f, 0.5f, 1f);
			newLine.transform.localScale = scaleVec;


		}
		else if (!isLine)
		{
			Note note;
			if (false & dead_pool.Count > 0)
			{
				note = dead_pool.First.Value;
				dead_pool.RemoveFirst();
				note.gameObject.SetActive(true);

			}
			else
			{
				note = Instantiate(notePrefab, noteCanvasTransform).GetComponent<Note>();
				note.nm = this;
				note.dead_pool = dead_pool;
				note.alive_pool = alive_pool;
				note.spawnPos = noteSpawnerTransform;
				note.exitPos = noteReceiverTransform;
			}
			alive_pool.AddLast(note);
			note.timeElapsed = 0;
			note.delay = delay;
			note.posOffset = 0f;
			note.dirOfNote = dirOfNote;
			note.typeOfNote = typeOfNote;
			note.ResetColor();

			float newRotation = 0f;
			switch (dirOfNote)
			{
				case "RIGHT":
					newRotation = 0f;
					break;
				case "UPRIGHT":
					newRotation = 45f;
					break;
				case "UP":
					newRotation = 90f;
					break;
				case "UPLEFT":
					newRotation = 135f;
					break;
				case "LEFT":
					newRotation = 180f;
					break;
				case "DOWNLEFT":
					newRotation = 225f;
					break;
				case "DOWN":
					newRotation = 270f;
					break;
				case "DOWNRIGHT":
					newRotation = 315f;
					break;
			}
			note.transform.rotation = Quaternion.Euler(0, 0, newRotation);
		}
		//Debug.Log(note.typeOfNote);
    }

    public void playHitSFX()
	{
		audioSource.PlayOneShot(hitSound, 0.7F);
	}

	public void playMissSFX()
	{
		audioSource.PlayOneShot(missSound, 0.7F);
	}

}

[System.Serializable]
public class NoteGroup
{
	[SerializeField] public int startTime;
	[SerializeField] public string dirOfNote;
	[SerializeField] public string typeOfNote;
	[SerializeField] public int trackNumber = 0;
}
