using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCore
{

    namespace Audio
    {
        public class AudioController : MonoBehaviour
        {
            //members
            public static AudioController instance;

            public bool debug;
            public AudioTrack[] tracks;

            private Hashtable m_AudioTable;//This is a relatioship between the audio types (keys) and the audio tracks (values)
            private Hashtable m_JobTable;//This is a relationship between the audio types (keys) and jobs (values) (Coroutine,IEnumerator)

            [System.Serializable] //These classes are public because we want to access them from the inspector.
            public class AudioObject
            {
                public AudioType type;
                public AudioClip clip;
            }

            [System.Serializable]
            public class AudioTrack 
            {
                public AudioSource source;
                public AudioObject[] audio;
            }

            private class AudioJob
            {
                public AudioAction action;
                public AudioType type;
                public bool fade;
                public float delay;

                public AudioJob(AudioAction _action, AudioType _type, bool _fade, float _delay)
                {
                    action = _action;
                    type = _type;
                    fade = _fade;
                    delay = _delay;
                }
            }

            private enum AudioAction
            {
                START,
                STOP,
                RESTART
            }

            #region Unity Functions
            //singleton
            private void Awake()
            {
                //instance
                if (!instance)
                {
                    Configure(); 
                }

                
               
            }
            //We also want to make sure that the coroutines are ended if the object is destroyed because coroutines in background can cause a memory leak.
            private void OnDisable()
            {
                Dispose();
            }



            #endregion

            #region Public Functions
            public void PlayAudio(AudioType _type, bool _fade = false, float _delay = 0)
            {                
                AddJob(new AudioJob(AudioAction.START, _type, _fade, _delay));
            }

            public void StopAudio(AudioType _type, bool _fade = false, float _delay = 0)
            {
                AddJob(new AudioJob(AudioAction.STOP, _type, _fade, _delay));
            }

            public void RestartAudio(AudioType _type, bool _fade = false, float _delay = 0)
            {
                AddJob(new AudioJob(AudioAction.RESTART, _type, _fade, _delay));
            }

            public AudioClip GetAudioClipFromAudioTrack(AudioType _type, AudioTrack _track)
            {
                foreach (AudioObject _obj in _track.audio)
                {
                    if (_obj.type == _type)
                    {
                        return _obj.clip;
                    }
                }

                return null;
            }

            #endregion

            #region Private Functions
            //In configure we do everything we need to do to initialise the object
            private void Configure()
            {
                instance = this;
                m_AudioTable = new Hashtable();
                m_JobTable = new Hashtable();
                GenerateAudioTable();
                            
            }

            //In Dispose we do everything we need to do to avoid potential memory leaks just in case
            private void Dispose()
            {
                foreach (DictionaryEntry _entry in m_JobTable)
                {
                    //We want to get the job and stop the coroutine
                    IEnumerator _job = (IEnumerator)_entry.Value;
                    StopCoroutine(_job);
                }
            }

            //Since we are defining AudioObject and AudioTrack from the inspector we want to convert those into the array tracks that eventually will be used to populate the hashtable    
            private void GenerateAudioTable()
            {
                foreach(AudioTrack _track in tracks)
                {
                    foreach(AudioObject _obj in _track.audio)
                    {
                        //We don't want to duplicate keys
                        if (m_AudioTable.ContainsKey(_obj.type))
                        {
                            LogWarning("You are trying to register Audio [" + _obj.type + "] that has already been registered.");
                        }
                        else
                        {
                            m_AudioTable.Add(_obj.type, _track);
                            Log("Registering audio [" + _obj.type + "].");
                        }
                    }
                }
            }


            private IEnumerator RunAudioJob (AudioJob _job)
            {

                yield return new WaitForSeconds(_job.delay);

                AudioTrack _track = (AudioTrack)m_AudioTable[_job.type];
                _track.source.clip = GetAudioClipFromAudioTrack(_job.type, _track);

                switch (_job.action)
                {
                    case AudioAction.START:
                        _track.source.Play();

                        break;

                    case AudioAction.STOP:
                        if(!_job.fade)
                        {
                            _track.source.Stop();
                        }                       
                        break;

                    case AudioAction.RESTART:
                        _track.source.Stop();
                        _track.source.Play();
                        break;
                }

                if(_job.fade)
                {
                    float _initial = _job.action == AudioAction.START || _job.action == AudioAction.RESTART ? 0.0f : 1.0f ;
                    float _target = _job.action == AudioAction.START || _job.action == AudioAction.RESTART ? 1.0f : 0.0f;

                    float _duration = 1.0f; //TODO make this a variable not a constant so we can modify the amount of time that we want to fade. 
                    float _timer = 0.0f;

                    while (_timer <= _duration)
                    {
                        _track.source.volume = Mathf.Lerp(_initial, _target, _timer/_duration); //this makes a number go from an initial value to a target value on  a normalised scale
                        _timer += Time.deltaTime;
                        yield return null;
                    }

                    if (_job.action == AudioAction.STOP)
                    {
                        _track.source.Stop();
                    }
                }

                m_JobTable.Remove(_job.type);
                Log("Job Count: " + m_JobTable.Count);

                yield return null;
            }

            private void AddJob (AudioJob _job)
            {
                //Remove conflicting jobs (lets say that one job on Soundtrack is running, and we need to play another, we need to turn the other off). Its an important edge case to take care of.
                RemoveConflictingJobs(_job.type);

                //Start the job
                IEnumerator _jobRunner = RunAudioJob(_job);
                m_JobTable.Add(_job.type, _jobRunner);
                StartCoroutine(_jobRunner);
                Log("Starting Job on [" + _job.type +"] with the operation [" + _job.action);

            
            }

            private void RemoveJob(AudioType _type)
            {
                if(!m_JobTable.ContainsKey(_type))
                {
                    LogWarning("You are trying to stop a job [" + _type + "] that is not running.");
                }

                IEnumerator _runningJob = (IEnumerator)m_JobTable[_type];
                StopCoroutine(_runningJob);
                m_JobTable.Remove(_type);
            }

            private void RemoveConflictingJobs (AudioType _type)
            {
                //Checks if the jobtable for existing jobs and if there is a conflicting one it removes it.
                if(m_JobTable.ContainsKey (_type))
                {
                    RemoveConflictingJobs(_type);
                }

                AudioType _conflictAudio = AudioType.None;
                foreach(DictionaryEntry _entry in m_JobTable)
                {
                    AudioType _audioType = (AudioType)_entry.Key;
                    AudioTrack _audioTrackInUse = (AudioTrack)m_AudioTable[_audioType];
                    AudioTrack _audioTrackNeeded = (AudioTrack)m_AudioTable[_type];//this is our incoming job type

                    if (_audioTrackNeeded.source == _audioTrackInUse.source)
                    {
                        //There is a conflict
                        _conflictAudio = _audioType;
                        //We dont remove the job because we are iterating across the job table, and you should not remove entries as you are iterating through a table , you will get exceptions thrown.
                    }
                }
                if (_conflictAudio != AudioType.None) //if the audiotype changes, that means there was a conflict
                {
                    RemoveJob(_conflictAudio);
                }
            
            }



            private void Log(string _msg)
            {
                if (!debug) return;
                Debug.Log("[AudioController]" + _msg);
            }

            private void LogWarning(string _msg)
            {
                if (!debug) return;
                Debug.LogWarning("[AudioController]" + _msg);
            }
            #endregion  
        }
    }
}
