using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	private static GameManager _singletonInstance;
	public static GameManager Instance{
		get{return _singletonInstance;}
	}

	public TouristicPlace[] Places;
	public float MaxTimeToMemorize;
	public float TimeBeforeShow;
	public CameraFollower CameraFollower;
	public float TimeToFindNextObjective;


	private float _timeToStart;
	private bool _gameStarted;
	private TouristicPlace _currentPlaceToFind;
	private int _currentPlaceIndex;
	private bool _gamePaused;
	private bool _gameEnded;
	private bool _gameLost;
	private float _timeLeft;


	public TouristicPlace CurrentPlaceToFind
	{
		get{
			return _currentPlaceToFind;
		}
	}

	public bool IsGameEnded {
		get{return _gameEnded;}
	}

	public bool IsGamePaused {
		get{return _gamePaused;}
	}


	void Awake()
	{
		_singletonInstance = this;
	}

	void Start ()
	{
		_timeToStart = MaxTimeToMemorize;
		_gameStarted = false;
		_gameEnded = false;
		_gameLost = false;
		_gamePaused = true;

		// shuffle places list
		for (int i = 0; i < Places.Length; i++) {
			TouristicPlace temp = Places[i];
			int randomIndex = Random.Range(i, Places.Length);
			Places[i] = Places[randomIndex];
			Places[randomIndex] = temp;
		}

		_currentPlaceIndex = 0;	
		_currentPlaceToFind = Places [_currentPlaceIndex]; // first one!
	}
	
	void Update () 
	{
		if (false == _gameStarted) {
			_timeToStart -= Time.deltaTime;
			_gameStarted = (_timeToStart <= 0.0f);

			if (_timeToStart <= TimeBeforeShow) {
				UIManager.Instance.ShowTimeToStart (_timeToStart);
			}

			if (_gameStarted) {
				// here starts the game!!!
				UIManager.Instance.HideTimeToStart ();
				this.CameraFollower.StartFollowing ();
				UIManager.Instance.ShowClueImage (_currentPlaceToFind.Key);
				_timeLeft = TimeToFindNextObjective;
				_gamePaused = false;

				Debug.Log (_currentPlaceToFind.Key);
			}
		} else if(false == _gamePaused){
			_timeLeft -= Time.deltaTime;
			UIManager.Instance.ShowTimeLeft(_timeLeft);

			if(_timeLeft <= 0.0f){
				CameraFollower.DoScenarioPan();
				UIManager.Instance.ShowLooserMessage();
			}
		}
	}


	public void PlaceFound()
	{
		_currentPlaceIndex++;

		if (_currentPlaceIndex == 3)
			NightFall.Instance.StartFalling = true;

		UIManager.Instance.HideTimeLeft ();

		if (_currentPlaceIndex >= Places.Length) {
			_gameEnded = true;
			_gamePaused = true;
		}
		else {
//			this.CameraFollower.StartFollowing();
			_gamePaused = true;
			_currentPlaceToFind = Places[_currentPlaceIndex];

			Debug.Log (_currentPlaceToFind.Key);
		}
	}

	public void ResumeGame()
	{
		_gamePaused = false;
		_timeLeft = TimeToFindNextObjective;
	}

	public void GameWin()
	{
		_gameEnded = true;

		CameraFollower.DoScenarioPan ();
		UIManager.Instance.ShowWinnerMessage ();
	}
}
