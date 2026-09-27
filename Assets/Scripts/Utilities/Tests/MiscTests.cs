using UnityEngine;

namespace Utilities.Tests
{
    public class MiscTests : MonoBehaviour
    {
        [DefaultInitStaticField] static int testCounter = 0;
        private bool hasDisplayed = false;
        public void Update()
        {
        }

        private void TestHasBeenPressedFor()
        {
            if (InputUtils.HasBeenPressedFor(2.0f))
            {
                Debug.Log("Pressed");
            }
        }
        
        private void TestStaticRuntimeInit()
        {
            if (hasDisplayed)
            {
                return;
            }
            Debug.Log("Test Counter: " + testCounter);
            testCounter = 100;
            hasDisplayed = true;
        }
    }
}