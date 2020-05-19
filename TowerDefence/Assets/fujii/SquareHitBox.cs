using UnityEngine;
public class SquareHitBox : MonoBehaviour
{
    /*****public field*****/
    public Collider2D LastHit { get { return m_lastHit; } }
    public Collider2D[] overlapResults { get { return m_overlapResults; } }
    public int hitCount { get { return m_hitCount; } }

    public Vector2 offset = new Vector2(1.5f, 1f);
    public Vector2 size = new Vector2(2.5f, 1f);
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
    protected Collider2D[] m_overlapResults = new Collider2D[10];
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
    public void DisableDamage()
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

        m_hitCount = Physics2D.OverlapArea(pointA, pointB, m_contactFilter, m_overlapResults);
        if(disableHitAfterHit)
            m_canHit = false;
    }
}