using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.IMGUI.Controls;
#endif
public class SquareHitBox : MonoBehaviour
{
    /*****public field*****/
    public Collider2D[] results { get { return m_results; } }
    public int hitCount { get { return m_hitCount; } }

    public Vector2 offset = new Vector2(0f, 0f);
    public Vector2 size = new Vector2(1f, 1f);
    [Tooltip("スプライトをX軸反転させたときhitBoxも反転させるかどうか")]
    public bool offsetBasedOnSpriteFacing = true;
    [Tooltip("反転してるかどうかを判定するためのスプライト")]
    public SpriteRenderer spriteRenderer;
    [Tooltip("トリガーにもヒットするかどうか")]
    public bool canHitTriggers;
    [Tooltip("ヒット後すぐにヒットできるようにするかどうか")]
    public bool disableHitAfterHit = false;
    public LayerMask hittableLayers;
    /*****protected method*****/
    protected bool m_spriteOriginallyFlipped;
    protected bool m_canHit = true;
    protected ContactFilter2D m_contactFilter;
    protected Collider2D[] m_results = new Collider2D[10];
    protected Transform m_transform;
    protected Collider2D m_lastHit;
    protected int m_hitCount;

    /*****monoBehaviour method*****/
    void Awake()
    {
        m_contactFilter.layerMask = hittableLayers;
        m_contactFilter.useLayerMask = true;
        m_contactFilter.useTriggers = canHitTriggers;
        if (offsetBasedOnSpriteFacing && spriteRenderer != null)
            m_spriteOriginallyFlipped = spriteRenderer.flipX;
        m_transform = transform;
    }
    /*****public method*****/
    public void EnableHit()
    {
        m_canHit = true;
    }
    public void DisableHit()
    {
        m_canHit = false;
    }
    public void CheckHit()
    {
        if (!m_canHit)
            return;

        Vector2 scale = m_transform.lossyScale;
        Vector2 facingOffset = Vector2.Scale(offset, scale);
        if (offsetBasedOnSpriteFacing && spriteRenderer != null && spriteRenderer.flipX != m_spriteOriginallyFlipped)
            facingOffset = new Vector2(-offset.x * scale.x, offset.y * scale.y);

        Vector2 scaledSize = Vector2.Scale(size, scale);

        Vector2 pointA = (Vector2)m_transform.position + facingOffset - scaledSize * 0.5f;
        Vector2 pointB = pointA + scaledSize;
        m_hitCount = Physics2D.OverlapArea(pointA, pointB, m_contactFilter, m_results);
        if(disableHitAfterHit)
            m_canHit = false;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SquareHitBox))]
public class DamagerEditor : Editor
{
    static BoxBoundsHandle s_boxBoundsHandle = new BoxBoundsHandle();
    static Color s_enabledColor = Color.green + Color.grey;

    SerializedProperty m_offsetProp;
    SerializedProperty m_sizeProp;
    SerializedProperty m_offsetBasedOnSpriteFacingProp;
    SerializedProperty m_spriteRendererProp;
    SerializedProperty m_canHitTriggersProp;
    SerializedProperty m_hittableLayersProp;
    SerializedProperty m_onDamageableHitProp;
    SerializedProperty m_onNonDamageableHitProp;

    void OnEnable()
    {
        m_offsetProp = serializedObject.FindProperty("offset");
        m_sizeProp = serializedObject.FindProperty("size");
        m_offsetBasedOnSpriteFacingProp = serializedObject.FindProperty("offsetBasedOnSpriteFacing");
        m_spriteRendererProp = serializedObject.FindProperty("spriteRenderer");
        m_canHitTriggersProp = serializedObject.FindProperty("canHitTriggers");
        m_hittableLayersProp = serializedObject.FindProperty("hittableLayers");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_offsetProp);
        EditorGUILayout.PropertyField(m_sizeProp);
        EditorGUILayout.PropertyField(m_offsetBasedOnSpriteFacingProp);
        if (m_offsetBasedOnSpriteFacingProp.boolValue)
            EditorGUILayout.PropertyField(m_spriteRendererProp);
        EditorGUILayout.PropertyField(m_canHitTriggersProp);
        EditorGUILayout.PropertyField(m_hittableLayersProp);

        serializedObject.ApplyModifiedProperties();
    }

    void OnSceneGUI()
    {
        SquareHitBox hitBox = (SquareHitBox)target;

        if (!hitBox.enabled)
            return;

        Matrix4x4 handleMatrix = hitBox.transform.localToWorldMatrix;
        handleMatrix.SetRow(0, Vector4.Scale(handleMatrix.GetRow(0), new Vector4(1f, 1f, 0f, 1f)));
        handleMatrix.SetRow(1, Vector4.Scale(handleMatrix.GetRow(1), new Vector4(1f, 1f, 0f, 1f)));
        handleMatrix.SetRow(2, new Vector4(0f, 0f, 1f, hitBox.transform.position.z));
        using (new Handles.DrawingScope(handleMatrix))
        {
            s_boxBoundsHandle.center = hitBox.offset;
            s_boxBoundsHandle.size = hitBox.size;

            s_boxBoundsHandle.SetColor(s_enabledColor);
            EditorGUI.BeginChangeCheck();
            s_boxBoundsHandle.DrawHandle();
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(hitBox, "Modify HitBox");

                hitBox.size = s_boxBoundsHandle.size;
                hitBox.offset = s_boxBoundsHandle.center;
            }
        }
    }
}
#endif