using System.Collections.Generic;
using UnityEngine;

namespace Game.Code.Logic.Music
{
    [RequireComponent(typeof(AudioSource))]
    public class PlaySoundEffect : MonoBehaviour
    {
        public static PlaySoundEffect Instance { get; private set; }
        
        private AudioSource audioSource;
        private static Dictionary<string, AudioClip> soundEffects = new Dictionary<string, AudioClip>();
        
        [Header("Sound Effects")]
        [SerializeField] private AudioClip cardDropSound;
        [SerializeField] private AudioClip EndTurnSound;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                audioSource = GetComponent<AudioSource>();
                InitializeSoundEffects();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            // AudioSource now initialized in Awake()
        }

        private void InitializeSoundEffects()
        {
            soundEffects.Clear();
            
            // Add predefined sound effects
            if (cardDropSound != null)
                soundEffects["CardDrop"] = cardDropSound;
                
            if (EndTurnSound != null)
                soundEffects["EndTurn"] = EndTurnSound;
            
            Debug.Log($"Loaded {soundEffects.Count} sound effects: {string.Join(", ", soundEffects.Keys)}");
        }
        
        public static void PlaySound(string soundKey)
        {
            if (Instance != null && soundEffects.ContainsKey(soundKey))
            {
                Instance.audioSource.PlayOneShot(soundEffects[soundKey]);
            }
            else
            {
                Debug.LogWarning($"Sound effect not found: {soundKey}. Available sounds: {string.Join(", ", soundEffects.Keys)}");
            }
        }
        
        public static void PlayCardDrop()
        {
            PlaySound("CardDrop");
        }
        
        // Add more specific sound effect methods here
        public static void PlayEndTurn()
        {
            PlaySound("EndTurn");
        }
       

        void Update()
        {

        }
    }
}
