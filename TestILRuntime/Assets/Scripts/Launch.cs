using System;
using System.Collections;
using System.IO;
using TMPro;
using Tool;
using UnityEngine;
using UnityEngine.UI;

namespace MainProject
{
    public class Launch : MonoBehaviour
    {
        public Button StartButton;
        System.IO.MemoryStream fs;
        System.IO.MemoryStream p;

        public static Action OnUpdate { get; set; }
        public static Action OnLateUpdate { get; set; }
        public static Action OnFixedUpdate { get; set; }
        public static Action OnApplicationQuitAction { get; set; }


        ILRuntime.Runtime.Enviorment.AppDomain appdomain;
        // Start is called before the first frame update
        void Start()
        {
            StartButton.onClick.AddListener(StartHotfix);
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
            fs = new MemoryStream(dll);
            p = new MemoryStream(pdb);
            appdomain.LoadAssembly(fs, p, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
            OnILRuntimeInitialized();

            //用于ILRuntime Debug
            if (Application.isEditor)
                appdomain.DebugService.StartDebugService(56000);
            OnHotFixLoaded();
        }

        void OnILRuntimeInitialized()
        {
            //registe delegate
            appdomain.DelegateManager.RegisterMethodDelegate<System.Boolean>();

            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction>((act) =>
            {
                return new UnityEngine.Events.UnityAction(() =>
                {
                    ((Action)act)();
                });
            });

            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.Boolean>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<System.Boolean>((arg0) =>
                {
                    ((Action<System.Boolean>)act)(arg0);
                });
            });
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.Int32, ILRuntime.Runtime.Intepreter.ILTypeInstance>, ILRuntime.Runtime.Intepreter.ILTypeInstance>();
            appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Boolean>();
            appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>();
            appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>>((act) =>
            {
                return new System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>((x, y) =>
                {
                    return ((Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>)act)(x, y);
                });
            });

            //registe crossbindadapter
            appdomain.RegisterCrossBindingAdaptor(new CoroutineAdapter());
        }

        void OnHotFixLoaded()
        {
            //HelloWorld，第一次方法调用
            //appdomain.Invoke("Hotfix.HotfixLuanch", "Start", null, null);
            StartHotfix();
        }

        void StartHotfix()
        {
            StartButton.gameObject.SetActive(false);
            appdomain.Invoke("Hotfix.HotfixLuanch", "Start", null, null);
        }

        void Update()
        {
            OnUpdate?.Invoke();
        }

        void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }

        void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }

        void OnApplicationQuit()
        {
            if (fs != null)
                fs.Close();
            if (p != null)
                p.Close();
            fs = null;
            p = null;
            StartButton.onClick.RemoveAllListeners();
            OnApplicationQuitAction?.Invoke();
            GC.Collect();
        }
    }
}

