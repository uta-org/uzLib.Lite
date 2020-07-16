#if UNITY_EDITOR

//using Mono.Cecil;
//using Mono.Cecil.Cil;
//using System;

using UnityEditor;

//using Weaver;
//using Weaver.Extensions;

namespace UnityEngine.UI.Menus
{
    public class RadioMenuComponent
        // : WeaverComponent
    {
        //public override string addinName => "Radio Menu";

        //private TypeReference m_InvokerTypeReference;
        //private MethodReference m_PerfomActionMethodRef;

        ////private MethodReference m_GetEnabledMethodRef;

        //public override void VisitModule(ModuleDefinition moduleDefinition)
        //{
        //    // Get 'Radio Menu Invoker' type
        //    Type invokerType = typeof(RadioMenuInvoker);
        //    // Import the 'Radio Menu Invoker' type
        //    m_InvokerTypeReference = moduleDefinition.Import(invokerType);
        //    // Get the type def by resolving
        //    TypeDefinition invokerTypeDef = m_InvokerTypeReference.Resolve();
        //    // Get our start sample
        //    m_PerfomActionMethodRef = invokerTypeDef.GetMethod("PerformAction", 1);
        //    //// Get the type GameObject
        //    //Type componentType = typeof(RadioMenuAttribute);
        //    //// Get Game Object Type R
        //    //TypeReference componentTypeRef = moduleDefinition.Import(componentType);
        //    //// Get the type def
        //    //TypeDefinition componentTypeDef = componentTypeRef.Resolve();
        //    //// Get our get property
        //    //PropertyDefinition gameObjectPropertyDef = componentTypeDef.GetProperty("Enabled");
        //    //m_GetEnabledMethodRef = gameObjectPropertyDef.GetMethod;

        //    // Import everything
        //    moduleDefinition.Import(typeof(RadioMenuAttribute));
        //    moduleDefinition.Import(m_PerfomActionMethodRef);
        //    //moduleDefinition.Import(m_GetEnabledMethodRef);
        //}

        //public override void VisitMethod(MethodDefinition methodDefinition)
        //{
        //    Debug.Log(methodDefinition.FullName);

        //    // Check if we have our attribute
        //    CustomAttribute customAttribute = methodDefinition.GetCustomAttribute<RadioMenuAttribute>();
        //    if (customAttribute == null)
        //        return;

        //    // Remove the attribute
        //    methodDefinition.CustomAttributes.Remove(customAttribute);

        //    MethodBody body = methodDefinition.Body;
        //    ILProcessor bodyProcessor = body.GetILProcessor();

        //    // Get the method definition for the injection definition
        //    // MethodDefinition performAction = typeof(RadioMenuInvoker).Assembly.GetMethod("PerformAction");

        //    // Start of method
        //    {
        //        // Instruction _00 = Instruction.Create(OpCodes.Ldstr, methodDefinition.DeclaringType.Name + ":" + methodDefinition.Name);
        //        //Instruction _00 = Instruction.Create(OpCodes.ld, );
        //        //Instruction _01 = Instruction.Create(OpCodes.Ldarg_0);
        //        Instruction _02 = Instruction.Create(OpCodes.Call, methodDefinition.Module.Import(m_PerfomActionMethodRef));
        //        // Instruction _03 = Instruction.Create(OpCodes.Call, methodDefinition.Module.Import(m_BeginSampleWithGameObjectMethodRef));

        //        bodyProcessor.InsertBefore(body.Instructions[0], _02);
        //        //bodyProcessor.InsertAfter(_00, _01);
        //        //bodyProcessor.InsertAfter(_01, _02);
        //        // bodyProcessor.InsertAfter(_02, _03);
        //    }

        //    // Loop over all types and insert end sample before return
        //    //for (int i = 0; i < body.Instructions.Count; i++)
        //    //{
        //    //    if (body.Instructions[i].OpCode == OpCodes.Ret)
        //    //    {
        //    //        Instruction _00 = Instruction.Create(OpCodes.Call, methodDefinition.Module.Import(m_EndSampleMethodRef));
        //    //        bodyProcessor.InsertBefore(body.Instructions[i], _00);
        //    //        i++;
        //    //    }
        //    //}

        //    // methodDefinition.CustomAttributes.Remove(profileSample);
        //}

        public class RadioMenuInvoker
        {
            public static void PerformAction(string path)
            {
                var attr = RadioMenuAttribute.GetAttributeFromPath(path);
                PerformAction(attr);
            }

            public static void PerformAction(RadioMenuAttribute radioMenu, bool changeOpt = true)
            {
                var enabled = EditorPrefs.GetBool(radioMenu.Path);
                if (changeOpt) enabled = !enabled;

                // Reset values
                var hashset = RadioMenuAttribute.GetAttributes(radioMenu.Path);
                foreach (var attr in hashset) SetValue(attr, false);

                SetValue(radioMenu, enabled);
            }

            private static void SetValue(RadioMenuAttribute radioMenu, bool enabled)
            {
                // Set checkmark on menu item
                Menu.SetChecked(radioMenu.Path, enabled);

                // Saving editor state
                EditorPrefs.SetBool(radioMenu.Path, enabled);

                radioMenu.Enabled = enabled;
            }
        }
    }
}

#endif