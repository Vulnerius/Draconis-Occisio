
using UnityEngine;

namespace Sound {
    [CreateAssetMenu(fileName = "Sound01", menuName = "ScriptableObjects/Audio")]
    public class SoundSource : ScriptableObject {
        
        [SerializeField] public AudioSource sound;
        [SerializeField] public AudioClip audioC;

        [Tooltip("Volume percentage")] [SerializeField] [Range(0, 1)]
        private float volume;

        [SerializeField] private bool mute;
        [SerializeField] private bool loop;
        [SerializeField] private bool _3d;

        [Tooltip("0 - hard pin left : 360 hard pin right")] [SerializeField] [Range(0, 360)]
        private float spread;

        [SerializeField] private int minDistance;
        [SerializeField] private int maxDistance;

        [Tooltip("Sets the roll-off if the player is further away than MaxDistance")] [SerializeField]
        private bool logarithmicRollOff;

        /// <summary>
        /// sets the Properties of AudioSource
        /// </summary>
        private void Awake() {
            sound.dopplerLevel = 0; //set this to 1 in Engine and you'll have a little crippling ear depression
            sound.spatialBlend = _3d ? 1f : .0f;
            sound.spatialize = _3d;
            sound.spread = spread;
            sound.mute = mute;
            sound.loop = loop;
            sound.minDistance = minDistance;
            sound.maxDistance = maxDistance;
            sound.volume = volume;
            sound.rolloffMode = logarithmicRollOff ? AudioRolloffMode.Logarithmic : AudioRolloffMode.Linear;
        }

        /// <summary>
        /// sets the audioClip
        /// instantiates a GameObjects
        /// Plays the Sound
        /// </summary>
        /// <param name="parent">position of the GameObject</param>
        /// <returns>playing AudioSource</returns>
        public AudioSource Play(Transform parent) {
            sound.clip = audioC;
            AudioSource soundSource = Instantiate(sound, parent);
            sound.Play();
            return soundSource;
        }
        
        /// <summary>
        /// sets the audioClip
        /// instantiates a GameObjects
        /// Plays the Sound with the effectVolume
        /// </summary>
        /// <param name="parent">position of the GameObject</param>
        /// <param name="effectVolume">volume of the soundEffect</param>
        public void Play(Transform parent, float effectVolume) {
            sound.clip = audioC;
            AudioSource soundSource = Instantiate(sound, parent);
            soundSource.volume = effectVolume;
            soundSource.Play();
        }

        /// <summary>
        /// stops the sound
        /// </summary>
        public void Reset() {
            sound.Stop();
        }

    }
}