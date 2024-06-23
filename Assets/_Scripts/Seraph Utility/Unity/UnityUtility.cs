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
    }
}