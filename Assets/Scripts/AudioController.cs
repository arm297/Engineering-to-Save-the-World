using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
	Generic Audio GameControl
	Contains playlist of songs to cycle through
*/
public class AudioController : MonoBehaviour {

	public List<AudioSource> PlayList = new List<AudioSource>();
	public List<float> PlayListLengths = new List<float>(); // dim should match above
	public List<AudioSource> SoundEffects = new List<AudioSource>();
	public int SoundEffect = -1; // -1 for no action, otherwise provide index of sound effect
	public int track = 0;
	public float timer = 0;
	public int firstTrackOnLoop = 1; // For when the first few tracks should not be repeated

	// Use this for initialization
	void Start () {
		PlayList[track].Play();
	}

	// Update is called once per frame
	void Update () {
		if(SoundEffect != -1){
			PlayList[track].Pause();
			SoundEffects[SoundEffect].Play();
			PlayList[track].Play();
			SoundEffect = -1;
		}
 		timer += Time.deltaTime;
		if(timer >= PlayListLengths[track]){
			timer = 0;
			track += 1;
			if(track >= PlayListLengths.Count){
				track = firstTrackOnLoop;
			}
			PlayList[track].Play();
		}
	}

}
