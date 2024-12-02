using UnityEngine;

namespace ProjectSims.Scripts
{
    [System.Serializable]
    public class RangeValue
    {
        [field: SerializeField]public float _Min { get; private set; }
        [field:SerializeField]public float _Max { get; private set; }

        public RangeValue(int min, int max)
        {
            _Min = min;
            _Max = max;
        }

        public void SetMin(int min)
        {
            _Min = min;
        }

        public void SetMax(int max)
        {
            _Max = max;
        }

        public float GetValue()
        {
            return Random.Range(_Min, _Max);
        }
    }
}