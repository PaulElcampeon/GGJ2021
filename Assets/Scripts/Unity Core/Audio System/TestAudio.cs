using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCore
{

    namespace Audio
    {
        public class TestAudio : MonoBehaviour
        {
            public AudioController audioController;

            private void Awake()
            {
                audioController.PlayAudio(AudioType.ST_01, false, 0f);

            }

            public void PlaySoundtrack()
            {
                audioController.PlayAudio(AudioType.ST_02, false, 0f);

            }

            #region Unity Functions
#if UNITY_EDITOR
            //private void Update()
            //{
            //    if (Input.GetKeyUp(KeyCode.R))
            //    {
            //        audioController.PlayAudio(AudioType.ST_01, true, 1.0f);
            //    }
            //    if (Input.GetKeyUp(KeyCode.T))
            //    {
            //        audioController.StopAudio(AudioType.ST_01, true);
            //    }
            //    if (Input.GetKeyUp(KeyCode.Y))
            //    {
            //        audioController.RestartAudio(AudioType.ST_01, true, 1.0f);
            //    }               
            //    if (Input.GetKeyUp(KeyCode.U))
            //    {
            //        audioController.PlayAudio(AudioType.SFX_01);
            //    }
            //    if (Input.GetKeyUp(KeyCode.I))
            //    {
            //        audioController.StopAudio(AudioType.SFX_01);
            //    }
            //    if (Input.GetKeyUp(KeyCode.O))
            //    {
            //        audioController.RestartAudio(AudioType.SFX_01);
            //    }
            //    if (Input.GetKeyUp(KeyCode.P))
            //    {

            //    }
            //}

#endif
            #endregion



        }
    }
}


