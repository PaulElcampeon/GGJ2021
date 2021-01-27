using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCore
{
    namespace Menu
    {
        public class TestMenu : MonoBehaviour
        {
            public PageController pageController;


#if UNITY_EDITOR
            private void Update()
            {
                if (Input.GetKeyUp(KeyCode.U))
                {
                    pageController.TurnPageOn(PageType.Loading);
                }

                if (Input.GetKeyUp(KeyCode.I))
                {
                    pageController.TurnPageOff(PageType.Loading);
                }

                if (Input.GetKeyUp(KeyCode.O))
                {
                    pageController.TurnPageOff(PageType.Loading, PageType.Menu);
                }

                if (Input.GetKeyUp(KeyCode.P))
                {
                    pageController.TurnPageOff(PageType.Loading, PageType.Menu, true);
                }

            }

#endif
        }
    }
}
