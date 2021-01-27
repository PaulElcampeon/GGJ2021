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

#region Unity Functions
#if UNITY_EDITOR
            private void Update()
            {
                if (Input.GetKeyUp(KeyCode.V))
                {
                    audioController.PlayAudio(AudioType.ST_01, true, 1.0f);
                }
                if (Input.GetKeyUp(KeyCode.B))
                {
                    audioController.StopAudio(AudioType.ST_01, true);
                }
                if (Input.GetKeyUp(KeyCode.N))
                {
                    audioController.RestartAudio(AudioType.ST_01, true, 1.0f);
                }               
                if (Input.GetKeyUp(KeyCode.M))
                {
                    audioController.PlayAudio(AudioType.SFX_01);
                }
                if (Input.GetKeyUp(KeyCode.F))
                {
                    audioController.StopAudio(AudioType.SFX_01);
                }
                if (Input.GetKeyUp(KeyCode.G))
                {
                    audioController.RestartAudio(AudioType.SFX_01);
                }
                if (Input.GetKeyUp(KeyCode.H))
                {

                }
            }

#endif
#endregion



        }
    }
}


