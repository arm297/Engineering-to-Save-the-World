using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Drills {

    /**
     *  Drill timer. Displays the time left, and plays the warning audio clip
     *  when the time left is small. 
     */
    public class Timer : MonoBehaviour
    {

        // Whether the current game is active.
        [HideInInspector]
        public bool isActive = false;

        // Whether or not the current game is paused.
        [HideInInspector]
        public bool isPaused = false;

        // 
        public delegate void OnTimerEnd();

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

        // Text object for drawing the timer.
        private Text clock;

        // Whether the warning has been played yet.
        private bool playedWarning = false;

        // Use this for initialization
        void Awake()
        {
            timeRemaining = initialTime;
            clock = GetComponent<Text>();
            clock.text = timeRemaining.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            if (isActive && !isPaused)
            {
                timeRemaining -= Time.deltaTime;
                if (timeRemaining <= warningTime && !playedWarning)
                {
                    AudioSource.PlayClipAtPoint(warningClip, transform.position);
                    playedWarning = true;
                }
                if (timeRemaining <= 0)
                {
                    timeRemaining = 0;
                    isActive = false;
                    OnTimerEnd();
                }
                clock.text = timeRemaining.ToString();
            }
        }
    }
}