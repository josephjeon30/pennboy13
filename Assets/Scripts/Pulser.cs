using UnityEngine;

public class Pulser : MonoBehaviour
{
	[SerializeField] private float pulseSize = 2f;
	[SerializeField] private float returnSpeed = 5f;
	private Vector3 startSize; 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    	startSize = transform.localScale;   
    	Pulse();
    }

    // Update is called once per frame
    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, startSize, Time.deltaTime * returnSpeed);
    }

    public void Pulse() {
    	transform.localScale = startSize * pulseSize;
    }
}
