using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sound {
    public class SoundManager : MonoBehaviour {
        [SerializeField] private SoundSource backgroundMusicRef;
        [SerializeField] private SoundSource hitEffectRef;

        private AudioSource backgroundMusic;
        private float effectVolume;
        
        private void Awake() {
            backgroundMusic = backgroundMusicRef.Play(transform);
        }

        public void OnPlayerHit(Transform player) {
            hitEffectRef.Play(player, effectVolume);
        }

        public void VolumeChangeMusic(Slider musicSlider) {
            backgroundMusic.volume = musicSlider.value;
        }

        public void VolumeChangeEffects(Slider effectSlider) {
            effectVolume = effectSlider.value;
        }
    }
}