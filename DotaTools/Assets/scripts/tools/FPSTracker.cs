using Tools;
using UnityEngine;

public class FPSTracker : MonoBehaviour
{
    public TXT text;

    private AverageFloat aveFPS = new AverageFloat(10);
    private int counter = 0;

#if UNITY_EDITOR
    [Range(30, 60)] public int fakeFpsSetter;
#endif

    public static float FPS = 60;

    private void Update()
    {
        aveFPS.AddNext(Time.deltaTime);
        counter++;
        if (counter > 10)
        {
            counter = 0;
            text.text = aveFPS.Average.Inv().Round().ToString();
#if UNITY_EDITOR
            FPS = fakeFpsSetter;
#else
            FPS = aveFPS.Average.Inv();
#endif
        }
    }
}