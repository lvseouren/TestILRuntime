using System.Collections;
using System.IO;
using UnityEngine;

namespace MainProject
{
    public class TestHotfix : MonoBehaviour
    {
        ILRuntime.Runtime.Enviorment.AppDomain appdomain;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(LoadILRuntime());
        }

        IEnumerator LoadILRuntime()
        {
            appdomain = new ILRuntime.Runtime.Enviorment.AppDomain();
#if UNITY_ANDROID
        WWW www = new WWW(Application.streamingAssetsPath + "/Hotfix.dll");
#else
            WWW www = new WWW("file:///" + Application.streamingAssetsPath + "/Hotfix.dll");
#endif
            while (!www.isDone)
                yield return null;
            if (!string.IsNullOrEmpty(www.error))
                Debug.LogError(www.error);
            byte[] dll = www.bytes;
            www.Dispose();
#if UNITY_ANDROID
        www = new WWW(Application.streamingAssetsPath + "/Hotfix.pdb");
#else
            www = new WWW("file:///" + Application.streamingAssetsPath + "/Hotfix.pdb");
#endif
            while (!www.isDone)
                yield return null;
            if (!string.IsNullOrEmpty(www.error))
                Debug.LogError(www.error);
            byte[] pdb = www.bytes;
            System.IO.MemoryStream fs = new MemoryStream(dll);
            System.IO.MemoryStream p = new MemoryStream(pdb);
            appdomain.LoadAssembly(fs, p, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());

            OnILRuntimeInitialized();
        }

        void OnILRuntimeInitialized()
        {
            appdomain.RegisterCrossBindingAdaptor(new TestBaseClassAdapter());
            appdomain.Invoke("Hotfix.DerivedClass", "TestInheritance", null, null);
        }
    }
}

