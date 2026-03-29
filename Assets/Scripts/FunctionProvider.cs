using UnityEngine;

public class FunctionProvider : MonoBehaviour
{
    [SerializeField] private float speed = 1;
        
    [Header("Function Settings")]
    [SerializeField] private FunctionType _functionType;
    [SerializeField] private FunctionParameter[] _parameters;

    public float GetYPosition(float x, float z)
    {
        return FunctionLibrary.GetFunction(_functionType)(x, z, Time.fixedTime * speed, _parameters);
    }
}
