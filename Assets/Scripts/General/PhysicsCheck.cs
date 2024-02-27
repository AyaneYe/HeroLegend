using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    [Header("��������")]
    public Vector2 bottomOffset;
    public float checkRadius;
    public LayerMask groundLayer;

    [Header("״̬")]
    public bool isGrounded;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Check();
    }

    void Check()
    {
        isGrounded = Physics2D.OverlapCircle((Vector2)transform.position+bottomOffset, checkRadius, groundLayer);
    }
    //���ƽŵ���ײ����
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRadius);
    }
}
