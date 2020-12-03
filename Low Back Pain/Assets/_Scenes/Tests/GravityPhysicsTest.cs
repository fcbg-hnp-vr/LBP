using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GravityPhysicsTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void GravityPhysicsTestSimplePasses()
        {
            // Use the Assert class to test conditions
           // Assert.Equals("Hello", "World");
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator GravityPhysicsTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
