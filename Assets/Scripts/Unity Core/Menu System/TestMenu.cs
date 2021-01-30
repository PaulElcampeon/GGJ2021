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
                if (Input.GetKeyUp(KeyCode.C))
                {
                    pageController.TurnPageOn(PageType.Loading);
                }

                if (Input.GetKeyUp(KeyCode.V))
                {
                    pageController.TurnPageOff(PageType.Loading);
                }

                if (Input.GetKeyUp(KeyCode.B))
                {
                    pageController.TurnPageOff(PageType.Loading, PageType.Menu);
                }

                if (Input.GetKeyUp(KeyCode.N))
                {
                    pageController.TurnPageOff(PageType.Loading, PageType.Menu, true);
                }

            }

#endif
        }
    }
}
