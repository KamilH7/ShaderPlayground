using System;
using System.Collections.Generic;
using UnityEngine;

public static class FunctionLibrary
{
    public delegate Vector3 Function(float u, float v, float functionOffset, params FunctionParameter[] parameters);

    private static readonly Dictionary<FunctionType, Function> _functionsMap = new ()
    {
        {
            FunctionType.Sine,
            NormalizedSine
        },
        {
            FunctionType.DoubleSine,
            MultiWave
        },
        {
            FunctionType.DoubleSineSlide,
            MultiWaveSlide
        },
        {
            FunctionType.Ripple,
            Ripple
        }
    };
        
    public static Function GetFunction(FunctionType type)
    {
        return _functionsMap[type];
    }
        
    private static Vector3 NormalizedSine(float xPos, float zPos, float timeOffset, params FunctionParameter[]  parameters)
    {
        return NormalizedSine(xPos, zPos, timeOffset, parameters.GetParameter(ParameterType.Frequency), parameters.GetParameter(ParameterType.Amplitude), parameters.GetParameter(ParameterType.Offset));
    }
        
    private static Vector3 NormalizedSine(float xPos, float zPos, float timeOffset, float frequency, float amplitude, float customOffset)
    {
        Vector3 result = new()
        {
            x = xPos,
            y = Mathf.Sin(frequency * Mathf.PI * (xPos + zPos + timeOffset + customOffset)) * amplitude,
            z = zPos,
        };

        return result;
    }
        
    private static Vector3 MultiWave (float xPos, float zPos, float functionOffset, params FunctionParameter[]  parameters) 
    {
        Vector3 result = NormalizedSine(xPos, 0, functionOffset, 1f, 1f, 0.5f);
        result += NormalizedSine(0, zPos, functionOffset, 2f, 0.5f, 0f);
        result += NormalizedSine(xPos, zPos, functionOffset, 1f, 1f, 0.25f);
        result.y *= 2f / 3;

        result.x = xPos;
        result.z = zPos;
        
        return result;
    }
    
    private static Vector3 MultiWaveSlide (float xPos, float zPos, float functionOffset, params FunctionParameter[] parameters) 
    {
        Vector3 result = NormalizedSine(zPos, 0, functionOffset);
        
        //multiply the function offset to make it move slower than the wave above
        result += NormalizedSine(xPos, 0, functionOffset * 0.5f, 2f, 0.5f,0f);
        
        result.x = xPos;
        result.z = zPos;
        result.y *= 2f / 3;
        return result;
    }
        
    private static Vector3 Ripple(float xPos, float zPos, float functionOffset, params FunctionParameter[] parameters)
    {
        float distance = Mathf.Sqrt(xPos * xPos + zPos * zPos);
        float amplitude = 1/(1 + parameters.GetParameter(ParameterType.DampeningForce) * distance); 
        Vector3 result = NormalizedSine(distance, 0, -functionOffset, parameters.GetParameter(ParameterType.Frequency), amplitude * parameters.GetParameter(ParameterType.Amplitude), 0f);
        result.x = xPos;
        result.z = zPos;
        return result;
    }

    private static float GetParameter(this FunctionParameter[] parameters, ParameterType type)
    {
        for (int i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].Type == type)
            {
                return parameters[i].Value;
            }
        }

        return 1;
    }
}

    
[Serializable]
public class FunctionParameter
{
    [field: SerializeField] public ParameterType Type { get; private set; }
    [field: SerializeField] public float Value { get; private set; }
        
    public FunctionParameter(ParameterType type, float value)
    {
        Type = type;
        Value = value;
    }
}
        
public enum ParameterType
{
    DampeningForce,
    Amplitude,
    Frequency,
    Offset
}
        
public enum FunctionType
{
    Sine,
    DoubleSine,
    DoubleSineSlide,
    Ripple
}