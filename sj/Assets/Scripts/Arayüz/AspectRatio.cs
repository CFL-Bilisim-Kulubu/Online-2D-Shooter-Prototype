using UnityEngine;

public enum ForceType
{
    Width, Height
}
public class AspectRatio : MonoBehaviour
{
    [SerializeField] private ForceType forceType;
    public float widthRatio, heightRatio;

    void Start()
    {

    }

    void Update()
    {

    }
}