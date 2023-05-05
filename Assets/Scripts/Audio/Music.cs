using UnityEngine;
using UnityEngine.SceneManagement;

namespace Audio
{
    public class Music : MonoBehaviour
    {
        private AudioManager audioManager;
        private void Awake()
        {
            var musicObjects = FindObjectsOfType<Music>();
            if (musicObjects.Length > 1)
            {
                Destroy(gameObject);
                return;
            }
            audioManager = GetComponent<AudioManager>();
            DontDestroyOnLoad(this);
        }
        
        // called first
        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        // called second
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Menu")
            {
                audioManager.Play("MenuMusic");
            }
            else
            {
                audioManager.Stop("MenuMusic");
                audioManager.Play("AmbMusic");
            }
            
        }

        public void ClickButtonSound()
        {
            audioManager.Play(("ClickSound"));
        }
        public void HoverButtonSound()
        { 
            audioManager.Play(("HoverSound"));
        }   

    }
}
