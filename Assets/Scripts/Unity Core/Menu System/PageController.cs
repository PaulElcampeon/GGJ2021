using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UnityCore
{
    namespace Menu
    {
        public class PageController : MonoBehaviour
        {
            public static PageController instance; //singleton

            public bool debug;
            public PageType entryPage; //page that loads at the beginning of the game.
            public Page[] pages;

            private Hashtable m_Pages; //Utilise the relationship between the pagetype and the objects themselves



            #region Unity Functions
            private void Awake()
            {
                if(!instance)
                {
                    instance = this;
                    m_Pages = new Hashtable();
                    RegisterAllPages();

                    if (entryPage != PageType.None)
                    {
                        TurnPageOn(entryPage);
                    }

                    //DontDestroyOnLoad(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }


            #endregion

            #region Public Functions
            public void TurnPageOn(PageType _type)
            {
                //Checks if the page is none type
                if (_type == PageType.None )
                {
                    return;
                }
                //Check if the page is any type
                if (!PageExists(_type))
                {
                    LogWarning("You are trying to turn a page on ["+ _type + "] that has not been registered");
                    return;
                }
                //gets the page
                Page _page = GetPage(_type);
                _page.gameObject.SetActive(true);
                _page.Animate(true); //if the page has ellected to use animation, it will run an animation, if not it will do nothing

            }

            //TurnPageOff has two optional parameters (page that you want off, an optional page to turn on after the first page is turned off, and a control parameter that determines
            //how these two pages turn on and off (either simultanously or one after the other, wait for animations, or dont, etc...)
            public void TurnPageOff(PageType _off, PageType _on = PageType.None, bool _waitForExit = false)
            {
                if (_off == PageType.None) return;
                if (!PageExists(_off))
                {
                    LogWarning("You are trying to turn a page off [" + _off + "] that has not been registered");
                    return;
                }

                Page _offPage = GetPage(_off);
                if(_offPage.gameObject.activeSelf)
                {
                    _offPage.Animate(false);
                }

                if (_on != PageType.None)
                { 
                    Page _onPage = GetPage(_on);
                    if (_waitForExit)
                    {                   
                        StopCoroutine("WaitForPageExit");
                        StartCoroutine(WaitForPageExit(_onPage, _offPage));
                    } else
                    {
                        TurnPageOn(_on);
                    }
                }
            }

            //Checks if a page exists and then returns a bool if the page is currently active
            public bool PageIsOn(PageType _type)
            {
                if (!PageExists(_type))
                {
                    LogWarning("You are trying to detect if a page is on [" + _type + "], but it has not been registered.");
                    return false;
                }

                return GetPage(_type).isOn;
            }

            #endregion

            #region Private Functions

            //Coroutine that waits for the off page to be finished turning off before turning the On page on
            private IEnumerator WaitForPageExit (Page _on, Page _off)
            {
                while (_off.targetState != Page.FLAG_NONE)
                {
                    yield return null;
                }

                TurnPageOn(_on.type);
                
            }

            private Page GetPage(PageType _type)
            {
                if (!PageExists(_type))
                {
                    LogWarning("You are trying to get a page [" + _type + "] that has not been registered");
                    return null;
                }
                return (Page)m_Pages[_type];
            }
            //loops through the pages in Page[] and registers them
            private void RegisterAllPages()
            {
                foreach (Page _page in pages)
                {
                    RegisterPage(_page);
                }
            }

            private void RegisterPage(Page _page)
            {
                if (PageExists(_page.type))
                {
                    LogWarning("You are trying to register a page ["+ _page.type + "] that has already been registered in: "+ _page.gameObject.name);
                    return;
                }

                m_Pages.Add(_page.type, _page);
                Log("Registered new page [" + _page.type + "]");
            }

            private bool PageExists(PageType _type)
            {

                return m_Pages.ContainsKey(_type);
            }


            //Functions to log messages and errors
            private void Log(string _msg)
            {
                if (!debug) return;
                Debug.Log("[PageController]" + _msg);
            }

            private void LogWarning (string _msg)
            {
                if (!debug) return;
                Debug.LogWarning("[PageController]" + _msg);
            }

            


            #endregion
        }
    }
}
