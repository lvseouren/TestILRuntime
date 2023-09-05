using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using System;
using UnityEngine;

namespace MainProject
{
    public class TestBaseClass
    { 
        public virtual void BaseMethod1()
        {
            Debug.Log("Run TestBaseClass BaseMethod1");
        }
    }

    public class TestInheritance
    {

    }

    public class TestBaseClassAdapter : CrossBindingAdaptor
    {
        static CrossBindingMethodInfo mBaseMethod1_0 = new CrossBindingMethodInfo("BaseMethod1");
        public override Type BaseCLRType
        {
            get
            {
                return typeof(TestBaseClass);
            }
        }


        public override Type AdaptorType
        {
            get
            {
                return typeof(Adapter);
            }
        }

        public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            return new Adapter(appdomain, instance);
        }


        public class Adapter : TestBaseClass, CrossBindingAdaptorType
        {
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            //必须要提供一个无参数的构造函数
            public Adapter()
            {

            }

            public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }

            public ILTypeInstance ILInstance { get { return instance; } }

            //下面将所有虚函数都重载一遍，并中转到热更内
            public override void BaseMethod1()
            {
                if (mBaseMethod1_0.CheckShouldInvokeBase(this.instance))
                    base.BaseMethod1();
                else
                    mBaseMethod1_0.Invoke(this.instance);
            }

            
            public override string ToString()
            {
                IMethod m = appdomain.ObjectType.GetMethod("ToString", 0);
                m = instance.Type.GetVirtualMethod(m);
                if (m == null || m is ILMethod)
                {
                    return instance.ToString();
                }
                else
                    return instance.Type.FullName;
            }
        }
    }
}
