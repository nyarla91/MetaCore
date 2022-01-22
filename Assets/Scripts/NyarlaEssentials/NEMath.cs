using UnityEngine;

namespace NyarlaEssentials
{
    public class NEMath : MonoBehaviour
    {
        public static int IntSign(float n)
        {
            if (n != 0)
                return n > 0 ? 1 : -1;
            return 0;
        }

        public static bool InBounds(float number, float max, float min) => (number >= min && number <= max);

        public static bool InBounds(float number, float bound) =>  InBounds(number, bound, -bound);

        public static float Evaluate(float value, float min, float max) => Mathf.Clamp((value - min) / (max - min), 0, 1);

        public static float Snap(float value, float step) => step == 0 ? value : Mathf.Round(value / step) * step;

        public static void Snap(ref float value, float step) => value = Snap(value, step);
        
        public static float TimeSin(float min, float max, float timeScale, float timeOffset)
        {
            float sin = Mathf.Sin(Time.time * timeScale + timeOffset);
            return Mathf.Lerp(min, max, (sin + 1) / 2);
        }
        public static float TimeSin(float min, float max, float timeScale) => TimeSin(min, max, timeScale, 0);
        public static float TimeSin(float min, float max) => TimeSin(min, max, 1);

        public static float Average(float[] numbers)
        {
            if (numbers.Length == 0)
                return 0;
            if (numbers.Length == 1)
                return numbers[0];
            float total = 0;
            foreach (var number in numbers)
            {
                total += number;
            }
            return total / numbers.Length;
        }

        public static void SetMax(ref float a, float b) => a = Mathf.Max(a, b);
        public static void SetMax(ref int a, int b) => a = Mathf.Max(a, b);
        public static void SetMin(ref float a, float b) => a = Mathf.Min(a, b);
        public static void SetMin(ref int a, int b) => a = Mathf.Min(a, b);

        public static bool IsEven(int value) => value % 2 == 0;
    }
}