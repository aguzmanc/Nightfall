using UnityEngine;
using System.Collections;

public class PlaceActivator : MonoBehaviour 
{
	private static GameObject _player;

	private TouristicPlace _place;
	private ParticleSystem _particles;

	private AudioSource _audio;

	public float DistanceToActivate;
	public bool IsThisPlaceSearchingTo 
	{
		get {
			return GameManager.Instance.CurrentPlaceToFind == GetComponentInParent<TouristicPlace>();
		}
	}

	void Awake()
	{
		_place = GetComponentInParent<TouristicPlace> ();
		_particles = GetComponent<ParticleSystem> ();
		_audio = GetComponent<AudioSource> ();
	}

	void Start () 
	{
		if (_player == null)
			_player = GameObject.FindGameObjectWithTag ("Player");

		if (DistanceToActivate == 0.0f)
			DistanceToActivate = 10.0f; // default
	}
	
	void Update () 
	{
		float distanceToPlayer = Vector3.Distance (_player.transform.position, transform.position); 
		bool isThisActivator = (distanceToPlayer < DistanceToActivate) && IsThisPlaceSearchingTo;

		_particles.enableEmission = isThisActivator;

//		_audio.enabled = isThisActivator;

		// quick fix!
		float MIN_DISTANCE = 100.0f;
		if (isThisActivator)
			Debug.Log (distanceToPlayer);

		if (distanceToPlayer <= MIN_DISTANCE && IsThisPlaceSearchingTo) {
			_audio.volume = 1.0f - (distanceToPlayer / MIN_DISTANCE);
			
		} else 
			_audio.volume = 0.0f;
	}

	void OnTriggerEnter(Collider coll)
	{
		if (coll.tag == "Player" && IsThisPlaceSearchingTo) {
			_place.ActivatePlace();
		}
	}



}
