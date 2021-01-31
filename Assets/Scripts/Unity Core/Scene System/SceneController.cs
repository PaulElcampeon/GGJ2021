
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityCore.Menu; //Yay! we get to use our menu class


namespace UnityCore
{

    namespace Scene
    {
        public class SceneController: MonoBehaviour
        {
            //This is going to define the callback signature for the function that will get called back when the scene is actually loaded.
            public delegate void SceneLoadDelegate(SceneType _scene);

            public static SceneController instance;

            public bool debug;

            private PageController m_Menu;                  //From the menu class
            private SceneType m_TargetScene;                //For reference 
            private PageType m_LoadingPage;                 //We will hold a reference to it as we are switching scenes and loading pages
            private SceneLoadDelegate m_SceneLoadDelegate;  //Reference to the current load delegate, so we can call it after the scene is loaded
            private bool m_SceneIsLoading;                  //Bool to show us if the scene is loading

            private PageController menu
            {
                get
                {
                    //If the menu doesnt exist, lets initialise it, and if it doesn't initialise it logs a warning.
                    if (m_Menu == null)
                    {
                        m_Menu = PageController.instance;
                    }
                    if (m_Menu == null)
                    {
                        LogWarning("You are trying to access the PageController, but no instance was found");
                    }
                    return m_Menu;
                }
            }

            private string currentSceneName
            {
                get
                {
                    return SceneManager.GetActiveScene().name;
                }
            }

            #region Unity Functions
            private void Awake()
            {
                //Singleton and inizialisation
                if (!instance)
                {
                    Configure();
                    DontDestroyOnLoad(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }

            private void OnDisable()
            {
                Dispose();
            }
            #endregion

            #region Public Functions

            public void Load(SceneType _scene, SceneLoadDelegate _sceneLoadDelegate = null, bool _reload = false, PageType _loadingPage = PageType.None)
            {
                //First it checks if we have a loading page, if the scene is ok to be loaded, and then starting off the coroutine to utilise the loading page and then load the scene.
                if (_loadingPage != PageType.None && !menu)
                {
                    return;
                }

                if (!SceneCanBeLoaded(_scene, _reload))
                {
                    return;
                }

                //initialising some class members that we will be using within the scope
                m_SceneIsLoading = true;
                m_TargetScene = _scene;
                m_LoadingPage = _loadingPage;
                m_SceneLoadDelegate = _sceneLoadDelegate;
                StartCoroutine("LoadScene");

            }

            public void LoadSimplified(string _scene)
            {
                
                Load(StringToSceneType(_scene));
            }

            #endregion

            #region Private Functions
            //Being called in awake, singleton pattern
            private void Configure()
            {
                instance = this;
                SceneManager.sceneLoaded += OnSceneLoaded; //subscribes to the event
            } 
            //Unsubscribes from the event
            private void Dispose ()
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
            //This is a callback for UnityEngine.SceneManagement
            private async void OnSceneLoaded (UnityEngine.SceneManagement.Scene _scene, LoadSceneMode _mode)
            {
                //We check if the target scene has no type, which would mean its a scene brought up by another system
                if (m_TargetScene == SceneType.None)
                {
                    return;
                }
                //This is a very edge case to prevent the wrong scene from loading
                SceneType _sceneType = StringToSceneType(_scene.name);
                if (m_TargetScene != _sceneType)
                {
                    return;
                }
                //Checking if the callback is not equal to null, and if it exists we try to call it
                if (m_SceneLoadDelegate != null)
                {
                    try
                    {
                        m_SceneLoadDelegate(_sceneType); // it is possible the delegate is no longer accessible(in case the object gets deleted before the scene is loaded)
                    }
                    catch (System.Exception)
                    {
                        LogWarning("Unable to respond with sceneLoadDelegate after scene [" + _sceneType + "] loaded.");
                    }
                }
                //Checking if we have a loading page, we wait 1 second then turns the loading page off. the 1000 is for testing, but in the futre we can get rid of the requirement.
                if (m_LoadingPage != PageType.None)
                {
                    await Task.Delay(1000);
                    menu.TurnPageOff(m_LoadingPage);
                }

                m_SceneIsLoading = false;
            }
            //Handles any waiting for the loading scene and the menu management to pull the loading page.
            private IEnumerator LoadScene ()
            {
                //Checks if we have a loading page
                if (m_LoadingPage != PageType.None)
                {
                    menu.TurnPageOn(m_LoadingPage);
                    while (!menu.PageIsOn(m_LoadingPage))
                    {
                        yield return null;
                    }
                }

                string _targetSceneName = SceneTypeToString(m_TargetScene);
                SceneManager.LoadScene(_targetSceneName);  
                
            }
            //Checks if 1/ the target and current scenes are the same and if we want to reload, 2/if the target scene is valid and 3/if a scene is already loading
            private bool SceneCanBeLoaded(SceneType _scene, bool _reload)
            {
                string _targetSceneName = SceneTypeToString(_scene);
                if (currentSceneName == _targetSceneName && !_reload)
                {
                    LogWarning("You are trying to load a scene [" + _scene + "] which is already active.");
                    return false;
                }
                else if (_targetSceneName == string.Empty)
                {
                    LogWarning("The scene you are trying to load [" + _scene + "] is not valid.");
                    return false;
                }
                else if (m_SceneIsLoading)
                {
                    LogWarning("Unable to load scene [" + _scene + "]. Another scene [" + m_TargetScene + "] is already loading.");
                    return false;
                }
                return true;
            }
            //Converts scene types to scene names
            private string SceneTypeToString (SceneType _scene)
            {
                switch (_scene)
                {
                    case SceneType.TemplateLevel: return "TemplateLevel";
                    case SceneType.Level1: return "Level1";
                    case SceneType.Level2: return "Level2";
                    case SceneType.Level3: return "Level3";
                    case SceneType.Level4: return "Level4";
                    case SceneType.Level5: return "Level5";
                    case SceneType.Level6: return "Level6";
                    case SceneType.Level7: return "Level7";
                    case SceneType.Level8: return "Level8";
                    case SceneType.Level9: return "Level9";
                    case SceneType.Level10: return "Level10";
                    case SceneType.Level11: return "Level11";
                    case SceneType.Level12: return "Level12";
                    case SceneType.Level13: return "Level13";
                    case SceneType.Level14: return "Level14";
                    case SceneType.Level15: return "Level15";
                    case SceneType.Level16: return "Level16";
                    case SceneType.Menu: return "Menu";
                    default:
                        LogWarning("Scene [" + _scene + "] does not contain a string for a valid scene.");
                        return string.Empty;
                }
            }
            //Converts scene names to scene types
            private SceneType StringToSceneType(string _scene)
            {
                switch (_scene)
                {
                    case "TemplateLevel": return SceneType.TemplateLevel;
                    case "Level1": return SceneType.Level1;
                    case "Level2": return SceneType.Level2;
                    case "Level3": return SceneType.Level3;
                    case "Level4": return SceneType.Level4;
                    case "Level5": return SceneType.Level5;
                    case "Menu": return SceneType.Menu;
                    default:
                        LogWarning("Scene [" + _scene + "] does not contain a type for a valid scene.");
                        return SceneType.None;
                }
            }

            private void Log(string _msg)
            {
                if (!debug) return;
                Debug.Log("[SceneController]" + _msg);
            }

            private void LogWarning(string _msg)
            {
                if (!debug) return;
                Debug.LogWarning("[SceneController]" + _msg);
            }
            #endregion  
        }

    }
}
