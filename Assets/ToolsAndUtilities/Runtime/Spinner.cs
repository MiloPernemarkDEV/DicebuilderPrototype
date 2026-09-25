using UnityEngine;
namespace DiceTools
{
    public class Spinner
    {

        private bool _isSpinning;

        public event System.Action SpinningStarted;
        public event System.Action SpinningFinished;
        public event System.Action SpinnerUpdated;


        public void Spin(float spinDuration, int steps)
        {
            float stepAmount = spinDuration / steps;
        }

    }
}

