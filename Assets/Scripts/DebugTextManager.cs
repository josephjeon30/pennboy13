using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugTextManager : MonoBehaviour
{

	public TextMeshProUGUI tmProText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tmProText.fontSize = 36;
        tmProText.color = Color.green;
    }

    public void UpdateText(string newText)
    {
    	if (tmProText != null)
        {
            tmProText.text = newText;
        }
    }
}
