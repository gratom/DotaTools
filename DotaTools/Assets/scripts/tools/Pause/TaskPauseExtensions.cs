using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Tools
{
    public static class TaskPauseExtensions
    {
        public static async Task PauseAwareDelay(int delayTime)
        {
            int elapsedTime = 0;

            start:
            int timeLeft = delayTime - elapsedTime;
            await Pause.UnPaused;

            float startAwaitingTime = Time.realtimeSinceStartup;
            Task delay = Task.Delay(timeLeft);
            Task pause = Pause.Paused;
            await Task.WhenAny(delay, pause);

            int passedTime = (int)((Time.realtimeSinceStartup - startAwaitingTime) * 1000);
            elapsedTime += passedTime;

            if (elapsedTime < delayTime)
            {
                goto start;
            }
        }
    }
}