using UnityEngine;

public class SizeChanger : MonoBehaviour
{
    private Vector3 initialScale;
    private void Awake()
    {
        initialScale = transform.localScale;
    }
    public void IncreaseScale(bool value)
    {
        Vector3 finalScale = initialScale;
        if (value)
        {
            finalScale = finalScale * 1.1f;
        }
        transform.localScale = finalScale;

    }
}
