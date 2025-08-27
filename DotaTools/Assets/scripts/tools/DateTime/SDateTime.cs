using System;
using UnityEngine;

namespace Tools
{
    [Serializable]
    public struct SDateTime
    {
        public DateTime Value
        {
            get
            {
                if (val == default)
                {
                    val = new DateTime(sValue);
                }
                return val;
            }
            set
            {
                val = value;
                sValue = val.Ticks;
            }
        }
        [NonSerialized] private DateTime val;

        [SerializeField] private long sValue;
    }

}