using System;
using UnityEngine;

namespace DungeonEscape.Music
{
    public class Music : MonoBehaviour
    {
        [Serializable]
        public class Track
        {
            public AudioClip audioClip;
            public bool loop = false;
        }

        public Track stageTrack = new Track();
        public Track bossTrack = new Track();
        public AudioSource audioSource;

        private void Start()
        {
            PlayStageTrack();
        }

        public void SetTrack(Track track)
        {
            audioSource.clip = track.audioClip;
            audioSource.loop = track.loop;
        }

        public void PlayStageTrack()
        {
            SetTrack(stageTrack);
            audioSource.Play();
            print(audioSource.clip.length);
            print(audioSource.isPlaying);
        }
    }
}
