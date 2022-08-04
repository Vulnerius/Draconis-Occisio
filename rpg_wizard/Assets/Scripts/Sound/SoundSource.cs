
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
        private bool logarithmicRolloff;

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
            sound.rolloffMode = logarithmicRolloff ? AudioRolloffMode.Logarithmic : AudioRolloffMode.Linear;
        }

        public AudioSource Play(Transform parent) {
            //Instantiate(this);
            sound.clip = audioC;
            AudioSource soundSource = Instantiate(sound, parent);
            sound.Play();
            return soundSource;
        }
        public void Play(Transform parent, float effectVolume) {
            //Instantiate(this);
            sound.clip = audioC;
            AudioSource soundSource = Instantiate(sound, parent);
            soundSource.volume = effectVolume;
            soundSource.Play();
        }

        public void Reset() {
            sound.Stop();
        }

    }
}