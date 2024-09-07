using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

    public static class Extensions
    {
        public static Vector3 Decay(this Vector3 A,Vector3 B, float Decay, float dt) 
        {
            return B + (A - B) * Mathf.Exp(-Decay * dt);
        }

        public static float Decay(this float A, float B, float Decay, float dt)
        {
            return B + (A - B) * Mathf.Exp(-Decay * dt);
        }
}

