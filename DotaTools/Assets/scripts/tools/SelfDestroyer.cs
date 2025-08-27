using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Tools
{
    public class SelfDestroyer : MonoBehaviour
    {
        [SerializeField] private float timeToDestroy = 1;

        private async void Awake()
        {
            await Task.Delay((int)(timeToDestroy * 1000));
            Destroy(gameObject);
        }
    }
}