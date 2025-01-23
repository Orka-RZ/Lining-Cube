Shader "Custom/Snow Glitter Optimized" {
	Properties {
		_Color ("Color", Vector) = (0.7058823,0.7058823,0.7058823,1)
		_Shininess ("Shininess", Range(0, 1)) = 0.2
		_BumpMap ("Normalmap", 2D) = "bump" {}
		_SpecularGlitterTex ("Specular glitter", 2D) = "white" {}
		_SpecularPower ("Specular power (0 - 5)", Range(0, 5)) = 1.5
		_SpecularContrast ("Specular contrast (1 - 3)", Range(1, 3)) = 1
		_SpecularMap ("Glitter map", 2D) = "white" {}
		_SpecularColor ("Glitter color", Vector) = (1,1,1,1)
		_GlitterPower ("Glitter power (0 - 10)", Range(0, 10)) = 2
		_GlitterContrast ("Glitter contrast (1 - 3)", Range(1, 3)) = 1.5
		_GlitterSpeed ("Glittery speed (0 - 1)", Range(0, 1)) = 0.5
		_GlitterMaskScale ("Glittery & mask dots scale", Range(0.1, 8)) = 2.5
		_MaskAdjust ("Mask adjust (0.5 - 1.5)", Range(0.5, 1.5)) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = _Color.rgb;
			o.Alpha = _Color.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}