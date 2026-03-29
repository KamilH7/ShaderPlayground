using System;
using System.Collections.Generic;
using UnityEngine;

public static class FunctionLibrary
{
    public delegate float Function(float xPos, float zPos, float functionOffset, params FunctionParameter[] parameters);

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
        
    private static float NormalizedSine(float xPos, float zPos, float timeOffset, params FunctionParameter[]  parameters)
    {
        return NormalizedSine(xPos, zPos, timeOffset, parameters.GetParameter(ParameterType.Frequency), parameters.GetParameter(ParameterType.Amplitude), parameters.GetParameter(ParameterType.Offset));
    }
        
    private static float NormalizedSine(float xPos, float zPos, float timeOffset, float frequency, float amplitude, float customOffset)
    {
        return Mathf.Sin(frequency * Mathf.PI * (xPos + zPos + timeOffset + customOffset)) * amplitude;
    }
        
    private static float MultiWave (float xPos, float zPos, float functionOffset, params FunctionParameter[]  parameters) {
        float y = NormalizedSine(xPos, 0, functionOffset, 1f, 1f, 0.5f);
        y += NormalizedSine(0, zPos, functionOffset, 2f, 0.5f, 0f);
        y += NormalizedSine(xPos, zPos, functionOffset, 1f, 1f, 0.25f);
        return y * 2/3;
    }
    private static float MultiWaveSlide (float xPos, float zPos, float functionOffset, params FunctionParameter[] parameters) {
        float y = NormalizedSine(zPos, 0, functionOffset);
        //multiply the function offset to make it move slower than the wave above
        y += NormalizedSine(xPos, 0, functionOffset * 0.5f, 2f, 0.5f,0f);
        return y * 2/3;
    }
        
    private static float Ripple(float xPos, float zPos, float functionOffset, params FunctionParameter[] parameters)
    {
        float distance = Mathf.Sqrt(xPos * xPos + zPos * zPos);
        float amplitude = 1/(1 + parameters.GetParameter(ParameterType.DampeningForce) * distance);
        float y = NormalizedSine(distance, 0, -functionOffset, parameters.GetParameter(ParameterType.Frequency), amplitude * parameters.GetParameter(ParameterType.Amplitude), 0f);
        return y;
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