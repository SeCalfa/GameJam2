using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Code.Logic.Music
{
    public class PlayMusic : MonoBehaviour
    {
        [SerializeField] private AudioClip[] musicTracks = new AudioClip[2];
        [SerializeField] private float volume = 0.5f;
        private AudioSource audioSource;

        void Start()
        {
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            audioSource.loop = true;
            audioSource.volume = volume;

            // Play random track on start
            if (musicTracks.Length == 0 || musicTracks[0] == null)
            {
                Debug.LogWarning("No music tracks assigned to PlayMusic component.");
                return;
            }
            else if (musicTracks.Length == 1)
            {
                audioSource.clip = musicTracks[0];
            }
            else
            {
                int randomIndex = Random.Range(0, musicTracks.Length);
                audioSource.clip = musicTracks[randomIndex];
            }
            audioSource.Play();
        }

        void Update()
        {

        }

        public void PlayTrack(int trackIndex)
        {
            if (trackIndex >= 0 && trackIndex < musicTracks.Length && musicTracks[trackIndex] != null)
            {
                audioSource.clip = musicTracks[trackIndex];
                audioSource.Play();
                Debug.Log($"Playing music track {trackIndex + 1}");
            }
        }

        public void StopMusic()
        {
            audioSource.Stop();
        }

        public void SetVolume(float newVolume)
        {
            volume = Mathf.Clamp01(newVolume);
            audioSource.volume = volume;
        }
                            // НЕ ЗАБУТЬ В ГЕЙМЕНЕДЖЕР ДОДАТИ ЦИХ 2 МЕТОДА
        public void TransferToContainer(Transform container)
        {
            if (container != null)
            {
                transform.SetParent(container);
            }
        }

        public void RemoveFromDontDestroy()
        {
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
        }
    }
}