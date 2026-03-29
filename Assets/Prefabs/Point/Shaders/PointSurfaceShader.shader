Shader "Custom/PointSurfaceShader"
{
    Properties 
    {
		_Smoothness ("Smoothness", Range(0,1)) = 0.5
	}
    
    SubShader
    {
        CGPROGRAM
        #pragma enable_d3d11_debug_symbols
        #pragma surface ConfigureSurface Standard fullforwardshadows
        #pragma target 3.0

        struct Input
        {
            float3 worldPos;
        };

        float _Smoothness;
        
        void ConfigureSurface (Input input, inout SurfaceOutputStandard surface)
        {
            surface.Albedo = surface.Albedo = saturate(input.worldPos * 0.5 + 0.5);
        }
        
        ENDCG
    }
    
    Fallback "Diffuse"
}
