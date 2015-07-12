using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour 
{
	private static UIManager _singletonInstance;

	public static UIManager Instance {
		get{return _singletonInstance;}
	}

	public UnityEngine.UI.Image _flash;
	public float DelayToTakeFlash;
	public UnityEngine.UI.Image _imageA;
	public UnityEngine.UI.Image _imageB;
	public UnityEngine.UI.Text TextTimeToStart;
	public UnityEngine.UI.Image ClueImage;
	public UnityEngine.UI.Text TextWinner;
	public UnityEngine.UI.Text TextLooser;
	public UnityEngine.UI.Text TextTimeLeft;
	public AudioSource CameraAudioA;
	public AudioSource CameraAudioB;

	private float _secondsUntilFlashA;
	private bool _waitingForFlashA;
	private float _secondsUntilFlashB;
	private bool _waitingForFlashB;


	private string _currentKey;
	private float _flashLevel;


	public bool IsWaitingForTakePhotos {
		get {
			return (_waitingForFlashA || _waitingForFlashB);
		}
	}

	void Awake()
	{
		_singletonInstance = this;
		_waitingForFlashA = false;
	}


	void Start () 
	{
		_flashLevel = 0.0f;
		HideTimeToStart ();
	}
	
	void Update () 
	{
		if (_waitingForFlashA) {
			_secondsUntilFlashA -= Time.deltaTime;
			
			if (_secondsUntilFlashA <= 0.0f) {
				_flashLevel = 1.0f;
				_waitingForFlashA = false;

				_imageA.transform.parent.gameObject.SetActive (true);
				_imageA.sprite = (Sprite)Resources.Load<Sprite> ("Photos/"+_currentKey+"A");

				CameraAudioA.Play();
			}
		}

		if (_waitingForFlashB) {
			_secondsUntilFlashB -= Time.deltaTime;
			
			if (_secondsUntilFlashB <= 0.0f) {
				_flashLevel = 1.0f;
				_waitingForFlashB = false;
				
				_imageB.transform.parent.gameObject.SetActive (true);
				_imageB.sprite = (Sprite)Resources.Load<Sprite> ("Photos/"+_currentKey+"B");

				CameraAudioB.Play();
			}
		}

		_flash.color = new Color (255, 255, 255, _flashLevel);

		_flashLevel -= Time.deltaTime/2.0f;
		_flashLevel = Mathf.Max (0.0f, _flashLevel);
	}


	public void TakeFlash(string key)
	{
		_secondsUntilFlashA = DelayToTakeFlash;
		_waitingForFlashA = true;

		_waitingForFlashB = true;
		_secondsUntilFlashB = DelayToTakeFlash + 1.0f;

		_currentKey = key;
	}


	public void HidePhotos()
	{
		_imageA.transform.parent.gameObject.SetActive (false);
		_imageB.transform.parent.gameObject.SetActive (false);
	}


	public void ShowTimeToStart(float timeToStart)
	{
		if (false == TextTimeToStart.enabled) 
			TextTimeToStart.enabled = true;

		int plus = (int)(timeToStart * 100);

		TextTimeToStart.text = "Tiempo Restante: 0" + (int)(plus / 100) + "." + plus % 100;
	}


	public void HideTimeToStart()
	{
		TextTimeToStart.enabled = false;
	}

	public void ShowClueImage(string key)
	{
		ClueImage.transform.parent.gameObject.SetActive (true);
		ClueImage.sprite = (Sprite)Resources.Load<Sprite> ("Photos/"+key+"Pista");
	}

	public void HideClueImage()
	{
		ClueImage.transform.parent.gameObject.SetActive (false);
		ClueImage.sprite = null;
	}

	public void ShowWinnerMessage()
	{
		TextWinner.enabled = true;
	}

	public void ShowLooserMessage() 
	{
		TextLooser.enabled = true;
	}

	public void ShowTimeLeft(float time)
	{
		if (false == TextTimeLeft.enabled) 
			TextTimeLeft.enabled = true;
		
		int plus = (int)(time * 100);
		
		string str = "" + (int)(plus / 100) + "." + (plus % 100) + " secs.";
		
		TextTimeLeft.text = str;
	}

	public void HideTimeLeft()
	{
		TextTimeLeft.enabled = false;
	}
}
