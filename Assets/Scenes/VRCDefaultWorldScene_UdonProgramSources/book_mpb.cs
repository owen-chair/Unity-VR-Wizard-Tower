using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

#if !COMPILER_UDONSHARP && UNITY_EDITOR
using UnityEditor;
using UdonSharpEditor;
#endif

[ExecuteInEditMode]  // Allows execution in Unity Editor
public class book_mpb : UdonSharpBehaviour
{
    [SerializeField] private Vector2 uvOffset = Vector2.zero;  // UV Offset exposed in Inspector
    private Renderer rend;
    private MaterialPropertyBlock mpb;

    private void Start()
    {
        ApplyUVOffset();
    }

    private void OnValidate()  // Runs when values change in the Editor
    {
        ApplyUVOffset();
    }

    public void ApplyUVOffset()
    {
        if (!rend) rend = GetComponent<Renderer>();
        if (mpb == null) mpb = new MaterialPropertyBlock();

        rend.GetPropertyBlock(mpb);
        mpb.SetVector("_MainTex_ST", new Vector4(1, 1, uvOffset.x, uvOffset.y));
        rend.SetPropertyBlock(mpb);
    }

#if !COMPILER_UDONSHARP && UNITY_EDITOR
    [CustomEditor(typeof(book_mpb))]
    public class BookUVOffsetChangerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(target)) return;

            book_mpb script = (book_mpb)target;

            EditorGUI.BeginChangeCheck();
            Vector2 newOffset = EditorGUILayout.Vector2Field("UV Offset", script.uvOffset);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(script, "Modify UV Offset");
                script.uvOffset = newOffset;
                script.ApplyUVOffset();
            }
        }
    }
#endif
}
