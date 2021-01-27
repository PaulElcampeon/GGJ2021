using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCore
{
    namespace Menu
    {
        public class Page : MonoBehaviour
        {
            
            //public states to use in animator controllers
            public static readonly string FLAG_ON = "On";
            public static readonly string FLAG_OFF = "Off";
            public static readonly string FLAG_NONE = "None";

            public PageType type;
            public bool debug;
            public bool useAnimation; //Bool to see if we want or not to use animation or transitions
            public string targetState { get; private set; }

            private Animator m_Animator; //In case we want to use animation this will be the animator
            private bool m_isOn;

            public bool isOn
            {
                get
                {
                    return m_isOn;
                }
                private set
                {
                    m_isOn = value;
                }
            }

            #region Unity Functions

            //We will check whether or not we need a reference for the animator and if we do, set the value
            private void OnEnable()
            {
                CheckAnimatorIntegrity();
            }
            #endregion

            #region Public Functions

            public void Animate (bool _on)
            {
                if (useAnimation)
                {
                    //When we animate we are setting a bool called on (true when turning the page on, false when turning the page off)
                    m_Animator.SetBool("on", _on);

                    StopCoroutine("AwaitAnimation"); //stopping coroutines to avoid memory leaks
                    StartCoroutine("AwaitAnimation", _on);
                } else
                {
                    if(!_on)
                    {
                        gameObject.SetActive(false);
                        isOn = false;
                    }
                    else
                    {
                        isOn = true;
                    }
                }
            }

            #endregion

            #region Private Functions

            //Waits for an animator to finish before setting a state
            private IEnumerator AwaitAnimation (bool _on)
            {
                targetState = _on ? FLAG_ON : FLAG_OFF;

                //Wait for the animator to reach the target state
                while(!m_Animator.GetCurrentAnimatorStateInfo(0).IsName(targetState))
                {
                    yield return null;
                }
                //Wait for the animator to finish animating
                while (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
                {
                    yield return null;
                }

                targetState = FLAG_NONE;
                Log("Page [" +type+"] finished transitioning to" + (_on ? "on" : "off"));
                if(!_on)
                {
                    isOn = false;
                    gameObject.SetActive(false);
                }
                else
                {
                    isOn = true;
                }

            }

            //This is the function that gets called in OnEnable, Checks if we need a reference for the animator
            private void CheckAnimatorIntegrity()
            {
                if (useAnimation)
                {
                    m_Animator = GetComponent<Animator>();
                    if(!m_Animator)
                    {
                        LogWarning("You tried to animate a page " + type + " but no animator component exists on the object.");
                    }
                }
            }


            //Functions to log messages and errors
            private void Log(string _msg)
            {
                if (!debug) return;
                Debug.Log("[Page]" + _msg);
            }

            private void LogWarning(string _msg)
            {
                if (!debug) return;
                Debug.LogWarning("[Page]" + _msg);
            }

            #endregion

        }

    }
}
