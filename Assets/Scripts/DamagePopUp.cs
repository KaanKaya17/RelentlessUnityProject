using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 1f;
    public float disappearTime = 1f;
    [Header("Components")]
    private TextMeshPro textMesh;
    private Color textColor;

    public void Setup(int damageAmount)
    {
        textMesh = GetComponent<TextMeshPro>();
        textMesh.SetText("-" + damageAmount.ToString());
        textColor = textMesh.color;
    }

    void Update()
    {
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);

        disappearTime -= Time.deltaTime;
        if (disappearTime < 0)
        {
            float fadeSpeed = 3f;
            textColor.a -= fadeSpeed * Time.deltaTime;
            textMesh.color = textColor;

            if (textColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}