#if USE_MESSAGEPACK
using MessagePack;
using MessagePack.Resolvers;
using MessagePack.Unity;
using MessagePack.Unity.Extension;
using UnityEngine;

public class MessagePackStartup
{
    public static bool serializerRegistered = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        if (!serializerRegistered)
        {
            Debug.Log("MessagePack : Initialize ... ");
            StaticCompositeResolver.Instance.Register(
                GeneratedResolver.Instance,
                StandardResolver.Instance,
                StandardResolverAllowPrivate.Instance,
                BuiltinResolver.Instance,
                PrimitiveObjectResolver.Instance,
                AttributeFormatterResolver.Instance,
                DynamicGenericResolver.Instance,
                UnityResolver.Instance,
                UnityBlitWithPrimitiveArrayResolver.Instance
            );

            var option = MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);

            MessagePackSerializer.DefaultOptions = option;
            serializerRegistered = true;
        }
    }

#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
    static void EditorInitialize()
    {
        Initialize();
    }

#endif
}

#endif