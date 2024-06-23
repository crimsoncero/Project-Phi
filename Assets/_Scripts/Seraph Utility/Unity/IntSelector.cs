using System;
using UnityEngine;

namespace SeraphUtil
{
    public delegate void SelAction(int value, bool wasIncreased);

    /// <summary>
    /// Class that enables going through values in an interative fashion
    /// </summary>
    public class IntSelector : MonoBehaviour
    {
        [field: Header("Value Control")]
        [field: Tooltip("The value that is being changed, you can set the default here.")]
        [field: SerializeField] public int Value { get; set; }

        [field: Tooltip("The minimum value.")]
        [field: SerializeField] public int Min { get; set; }

        [field: Tooltip("The maximum value.")]
        [field: SerializeField] public int Max { get; set; }

        [field: Tooltip("The amount the value increases or decreases.")]
        [field: SerializeField] public int Iterator { get; set; }


        [field: Header("Options")]
        [field: Tooltip("Whether the values wrap or not")]
        [field: SerializeField] public bool Cycle { get; set; }


        // Events
        /// <summary>
        /// Raised whenever the value changes.
        /// </summary>
        public event SelAction ValueChanged;


        public void Increase()
        {
            int tempVal = Value;
            tempVal += Iterator;

            if (tempVal > Max)
            {
                if (Cycle)
                {
                    // Overflow value.
                    Value = Min;
                }
                else
                {
                    // Clamp value.
                    Value = Max;
                }
            }
            else
            {
                Value = tempVal;
            }

            ValueChanged?.Invoke(Value, true);
        }

        public void Decrease()
        {
            int tempVal = Value;
            tempVal -= Iterator;

            if (tempVal < Min)
            {
                if (Cycle)
                {
                    // Underflow value.
                    Value = Max;
                }
                else
                {
                    // Clamp value.
                    Value = Min;
                }
            }
            else
            {
                Value = tempVal;
            }

            ValueChanged?.Invoke(Value, false);
        }

    }
}