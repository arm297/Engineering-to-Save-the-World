using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    // Initial number of seconds set for timer.
    [SerializeField]
    private float initialTime;

    // Seconds left until the warning soud plays.
    [SerializeField]
    private float warningTime;

    // Warning audio clip.
    [SerializeField]
    private AudioClip warningClip;

    // Seconds remaining until time expires.
    private float timeRemaining;

    // Set up timer for drills.
    void Awake () {
        timeRemaining = initialTime;

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
