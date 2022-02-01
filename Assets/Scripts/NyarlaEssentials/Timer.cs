using System;
using System.Collections;
using UnityEngine;

namespace NyarlaEssentials
{
    public class Timer
    {

        private TimerContainer _container;
        private float _timeElapsed;
        private bool _active;
        
        public float Length { get; set; }
        public bool Loop { get; set; }
        public bool FixedTime { get; set; }

        public float TimeElapsed => _timeElapsed;
        public float TimeLeft => Length - TimeElapsed;

        public Action OnExpired;

        public Timer(float length, bool loop, bool fixedTime)
        {
            Length = length;
            Loop = loop;
            FixedTime = fixedTime;
            Init();
        }

        public Timer(float length, bool loop)
        {
            Length = length;
            Loop = loop;
            Init();
        }
        
        public Timer(float length)
        {
            Length = length;
            Init();
        }

        public void Start()
        {
            _container.StartCoroutine(Ticking());
        }

        public void Restart()
        {
            Stop();
            Start();
        }

        public void Reset()
        {
            Stop();
            _timeElapsed = 0;
        }

        public void Stop()
        {
            _container.StopAllCoroutines();
        }

        private void Init()
        {
            _container = new GameObject().AddComponent<TimerContainer>();
        }

        private IEnumerator Ticking()
        {
            for (_timeElapsed = 0;
                _timeElapsed < Length;
                _timeElapsed += FixedTime ? Time.fixedDeltaTime : Time.deltaTime)
            {
                if (FixedTime)
                    yield return new WaitForFixedUpdate();
                else
                    yield return null;
            }
            
            OnExpired?.Invoke();
            
            if (Loop)
                Start();
            else
                Reset();
        }
    }
}