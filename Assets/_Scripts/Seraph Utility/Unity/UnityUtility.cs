using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SeraphUtil
{
    public static class UnityUtility 
    {
        public static IEnumerator RumbleController(float lowFrequency, float highFrequency, float duration)
        {
            Gamepad.current.SetMotorSpeeds(lowFrequency, highFrequency);

            yield return new WaitForSeconds(duration);

            Gamepad.current.ResetHaptics();
        }

        /// <summary>
        /// Returns a random element from the given array. Using Unity Random
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T Random<T> (this T[] t)
        {
            int index = UnityEngine.Random.Range(0, t.Length);

            return t[index];
        }

    }
}