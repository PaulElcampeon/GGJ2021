﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCore.Menu;//Yay! we get to use our menu class

namespace UnityCore
{

    namespace Scene
    {
        public class TestScene : MonoBehaviour
        {

            public SceneController sceneController;

            #region Unity Functions
#if UNITY_EDITOR
            private void Update()
            {
                if (Input.GetKeyUp(KeyCode.M))
                {
                    sceneController.Load(SceneType.Menu, (_scene) => {
                        Debug.Log("Scene [" + _scene + "] loaded from test script!");
                    }, false, PageType.Loading);
                }
                if (Input.GetKeyUp(KeyCode.G))
                {
                    sceneController.Load(SceneType.Level1);
                }
            }
#endif
            #endregion
        }
    }
}
