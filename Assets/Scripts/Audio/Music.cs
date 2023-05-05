using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Audio
{
    public class Music : MonoBehaviour
    {
        private AudioManager audioManager;
        private void Awake()
        {
            audioManager = GetComponent<AudioManager>();
            DontDestroyOnLoad(this);
        }
        
        // called first
        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
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
