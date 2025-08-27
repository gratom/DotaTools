using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Tools
{
    public static class Pause
    {
        public static bool IsPaused
        {
            get => isPaused;
            set
            {
                if (isPaused != value)
                {
                    isPaused = value;
                    if (timeScaleFollowPause)
                    {
                        TimeScalePause = isPaused;
                    }
                    OnPauseChange?.Invoke(isPaused);
                    if (isPaused)
                    {
                        OnPaused?.Invoke();
                        unpausedAwaiter = new TaskCompletionSource<bool>();
                        pausedAwaiter.TrySetResult(true);
                    }
                    else
                    {
                        OnUnpaused?.Invoke();
                        pausedAwaiter = new TaskCompletionSource<bool>();
                        unpausedAwaiter.TrySetResult(true);
                    }

                }
            }
        }
        private static bool isPaused = false;

        public static bool TimeScaleFollowPause
        {
            get => timeScaleFollowPause;
            set
            {
                if (value != timeScaleFollowPause)
                {
                    timeScaleFollowPause = value;
                    if (TimeScalePause != isPaused)
                    {
                        TimeScalePause = isPaused;
                    }
                }
            }
        }
        private static bool timeScaleFollowPause;

        private static bool TimeScalePause
        {
            get => Time.timeScale == 0;
            set => Time.timeScale = value ? 0 : 1;
        }

        public static event Action OnPaused;
        public static event Action OnUnpaused;
        public static event Action<bool> OnPauseChange;
        public static Task Paused => pausedAwaiter.Task;

        private static TaskCompletionSource<bool> pausedAwaiter = new TaskCompletionSource<bool>();
        public static Task UnPaused => unpausedAwaiter.Task;

        private static TaskCompletionSource<bool> unpausedAwaiter;

        static Pause()
        {
            unpausedAwaiter = new TaskCompletionSource<bool>();
            unpausedAwaiter.TrySetResult(true);
        }

        public static void PauseOn()
        {
            IsPaused = true;
        }

        public static void PauseOff()
        {
            IsPaused = false;
        }

        public static void Toggle()
        {
            Debug.Log($"toggle pause [{IsPaused} -> {!IsPaused}]");
            IsPaused = !IsPaused;
        }
    }
}