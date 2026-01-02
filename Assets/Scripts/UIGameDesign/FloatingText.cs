using UnityEngine;
using TMPro;

public class FloatingHealth : MonoBehaviour
{
    [Header("Floating Text Settings")]
    public float moveSpeed = 2f;
    public float destroyTime = 1f;

    [Header("Components")]
    private TextMeshPro textMesh;
    private Color color;

    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        color = textMesh.color;
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);

        color.a -= Time.deltaTime / destroyTime;
        textMesh.color = color;
    }
}