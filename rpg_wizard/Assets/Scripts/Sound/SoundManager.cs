using UnityEngine;
using UnityEngine.UI;

namespace Sound {
    /// <summary>
    /// Sound Manager class
    /// </summary>
    public class SoundManager : MonoBehaviour {
        [SerializeField] private SoundSource backgroundMusicRef;

        private AudioSource backgroundMusic;
        private float effectVolume = .5f;
        
        private void Awake() {
            backgroundMusic = backgroundMusicRef.Play(transform);
        }

        public void OnSoundEffect(Transform position, SoundSource hitEffectRef) {
            hitEffectRef.Play(position, effectVolume);
        }

        public void VolumeChangeMusic(Slider musicSlider) {
            backgroundMusic.volume = musicSlider.value;
        }

        public void VolumeChangeEffects(Slider effectSlider) {
            effectVolume = effectSlider.value;
        }
    }
}