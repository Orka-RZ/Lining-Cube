Shader "LowPolyWater/Advanced" {
	Properties {
		_Shadow ("Shadow Bias", Range(0, 1)) = 0.7
		_Color ("Color", Vector) = (0,0.5,0.7,1)
		_Opacity ("Opacity", Range(0, 1)) = 0.7
		_Specular ("Specular", Range(1, 300)) = 70
		_SpecColor ("Sun Color", Vector) = (0.703,0.676,0.438,1)
		_Diffuse ("Diffuse", Range(0, 1)) = 0.5
		[Toggle] _PointLights ("Enable Point Lights", Float) = 0
		[KeywordEnum(Flat, VertexLit, PixelLit)] _Shading ("Shading", Float) = 0
		[NoScaleOffset] _FresnelTex ("Fresnel (A) ", 2D) = "" {}
		_FresPower ("Fresnel Exponent", Range(0, 2)) = 1.5
		_FresColor ("Fresnel Color", Vector) = (0.305,0.371,0.395,1)
		_Reflection ("Reflection", Range(0, 2)) = 1.2
		_Refraction ("Refractive Distortion", Float) = 2
		_NormalOffset ("Normal Offset", Range(0, 5)) = 1
		[Toggle] _Distort ("Enable Distortion", Float) = 0
		_Distortion ("Reflective Distortion", Float) = 1
		[NoScaleOffset] _BumpTex ("Distortion Map", 2D) = "" {}
		_BumpScale ("Distortion Scale", Float) = 35
		_BumpSpeed ("Distortion Speed", Float) = 0.2
		[KeywordEnum(Off, LowQuality, HighQuality)] _Waves ("Enable Waves", Float) = 2
		_Length ("Wave Length", Float) = 4
		_Stretch ("Wave Stretch", Float) = 10
		_Speed ("Wave Speed", Float) = 0.5
		_Height ("Wave Height", Float) = 0.5
		_Steepness ("Wave Steepness", Range(0, 1)) = 0.2
		_Direction ("Wave Direction", Range(0, 360)) = 180
		_RSpeed ("Ripple Speed", Float) = 1
		_RHeight ("Ripple Height", Float) = 0.25
		[Toggle] _EdgeBlend ("Enable Foam", Float) = 0
		_ShoreColor ("Foam Color", Vector) = (1,1,1,1)
		_ShoreIntensity ("Foam Intensity", Range(-1, 1)) = 1
		_ShoreDistance ("Foam Distance", Float) = 0.5
		[Toggle] _HQFoam ("Enable HQ Foam", Float) = 0
		_FoamScale ("Foam Scale", Float) = 20
		_FoamSpeed ("Foam Speed", Float) = 0.3
		_FoamSpread ("Foam Spread", Float) = 1
		[Toggle] _LightAbs ("Enable Light Absorption", Float) = 0
		_Absorption ("Depth Transparency", Float) = 5
		_DeepColor ("Deep Water Color", Vector) = (0,0.1,0.2,1)
		_Scale ("Global Scale", Float) = 1
		[NoScaleOffset] _NoiseTex ("Noise Texture (A)", 2D) = "" {}
		[Toggle] _Cull ("Show Surface Underwater", Float) = 0
		[Toggle] _ZWrite ("Write to Depth Buffer", Float) = 0
		[HideInInspector] _TransformScale_ ("_TransformScale_", Float) = 1
		[HideInInspector] _Scale_ ("_Scale_", Float) = 1
		[HideInInspector] _BumpScale_ ("_BumpScale_", Float) = 1
		[HideInInspector] _Cull_ ("_Cull_", Float) = 2
		[HideInInspector] _Direction_ ("_Direction_", Vector) = (0,0,0,0)
		[HideInInspector] _RHeight_ ("_RHeight_", Float) = 0.2
		[HideInInspector] _RSpeed_ ("_RSpeed_", Float) = 0.2
		[HideInInspector] _TexSize_ ("_TexSize_", Float) = 64
		[HideInInspector] _Speed_ ("_Speed_", Float) = 0
		[HideInInspector] _Height_ ("_Height_", Float) = 0
		[HideInInspector] _ReflectionTex ("_ReflectionTex", 2D) = "" {}
		[HideInInspector] _RefractionTex ("_RefractionTex", 2D) = "" {}
		[HideInInspector] _Time_ ("_Time_", Float) = 0
		[HideInInspector] _EnableShadows ("_EnableShadows", Float) = 0
		[HideInInspector] _Sun ("_Sun", Vector) = (0,0,0,0)
		[HideInInspector] _SunColor ("_SunColor", Vector) = (1,1,1,1)
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
	Fallback "Mobile/VertexLit"
	//CustomEditor "LPWAsset.LPWShaderGUI"
}