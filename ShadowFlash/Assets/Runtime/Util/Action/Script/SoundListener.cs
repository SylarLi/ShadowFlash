using UnityEngine;

namespace Action
{
    public class SoundListener : MonoListener
    {
        public const string NormalizedTime = "NormalizedTime";

        private float mNormalizedTime;

        private AudioSource sound;

        void Start()
        {
            sound = GetComponent<AudioSource>();
        }

        void Update()
        {
            if (sound.isPlaying)
            {
                normalizedTime += Time.deltaTime / sound.clip.length;
            }
        }

        public float normalizedTime
        {
            get
            {
                return mNormalizedTime;
            }
            set
            {
                if (Mathf.Abs(mNormalizedTime - value) > 0.001f)
                {
                    mNormalizedTime = value;
                    DispatchEvent(new Core.Event(NormalizedTime));
                }
            }
        }
    }
}
