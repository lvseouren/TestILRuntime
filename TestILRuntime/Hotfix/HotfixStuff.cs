using MainProject;
using UnityEngine;

namespace Hotfix
{
    public class HotfixStuff
    {
        public static void TestFunc1()
        {
            Debug.Log("Run Hotfix HotfixStuff TestFun1");
        }
    }

    public class DerivedClass: TestBaseClass
    {
        static void TestInheritance()
        {
            Debug.Log("Run Hotfix DerivedClass TestInheritance");
        }

        public override void BaseMethod1()
        {
            Debug.Log("Run DerivedClass BaseMethod1");
        }

    }
}
