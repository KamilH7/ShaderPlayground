using System;
using System.Collections.Generic;
using UnityEngine;
using Mathf = UnityEngine.Mathf;

namespace Playground._2.Wave.Scripts
{
    public static class FunctionLibrary
    {
        public delegate float Function(float xPos, float functionOffset, params FunctionParameter[] parameters);

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
        
        private static float NormalizedSine(float xPos, float functionOffset, params FunctionParameter[]  parameters)
        {
            return NormalizedSine(xPos, functionOffset, parameters.GetParameter(ParameterType.Frequency), parameters.GetParameter(ParameterType.Amplitude));
        }
        
        private static float NormalizedSine(float xPos, float functionOffset, float frequency, float amplitude)
        {
            return Mathf.Sin(frequency * Mathf.PI * (xPos + functionOffset)) * amplitude;
        }
        
        private static float MultiWave (float xPos, float functionOffset, params FunctionParameter[]  parameters) {
            float y = NormalizedSine(xPos, functionOffset);
            y += NormalizedSine(xPos, functionOffset, 2, 0.5f);
            return y * 2/3;
        }
        private static float MultiWaveSlide (float xPos, float functionOffset, params FunctionParameter[] parameters) {
            float y = NormalizedSine(xPos, functionOffset);
            //multiply the function offset to make it move slower than the wave above
            y += NormalizedSine(xPos, functionOffset * 0.5f, 2, 0.5f);
            return y * 2/3;
        }
        
        private static float Ripple(float xPos, float functionOffset, params FunctionParameter[] parameters)
        {
            float distance = Mathf.Abs(xPos);
            float amplitude = 1/(1 + parameters.GetParameter(ParameterType.DampeningForce) * distance);
            float y = NormalizedSine(distance, -functionOffset, parameters.GetParameter(ParameterType.Frequency), amplitude * parameters.GetParameter(ParameterType.Amplitude));
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
        Frequency
    }
        
    public enum FunctionType
    {
        Sine,
        DoubleSine,
        DoubleSineSlide,
        Ripple
    }
}
