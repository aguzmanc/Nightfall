using UnityEngine;
using System.Collections;

public class Flashing : MonoBehaviour 
{
	public UnityEngine.UI.Text Text;
	public float Freq;

	private Color _color;
	private float _timeToStop;

	void Start () 
	{
		_color = Text.color;
		_timeToStop = 4.0f;
		Text.enabled = true;
	}
	
	void Update () 
	{
		if (_timeToStop >= 0.0f) {

			Text.color = new Color (_color.r, _color.g, _color.b, Mathf.Sin (Time.time * Freq) * 0.5f + 1.0f);
			Text.transform.localScale = Vector3.one * (Mathf.Sin (Time.time * Freq) * 0.5f + 1.0f);

			_timeToStop -= Time.deltaTime;
		} else
			Text.enabled = false;


	}
}
