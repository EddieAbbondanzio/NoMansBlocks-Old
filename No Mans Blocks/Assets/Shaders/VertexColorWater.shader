// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Eddie/VertexColorWater" {
	Properties {
		//Vertex Moving Stuff
		_WaveLength ("Wave Length", Float) = 0.15
		_WaveHeight ("Wave Height", Float) = 0.15
		_WaveSpeed ("Wave speed", Float) = 1.76
		_RandomHeight("Random Height", Float) = 0.3
		_RandomSpeed("Random Speed", Float) = 0.36
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Specifies that there is a vertex function
		#pragma surface surf Lambert vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		float rand(float3 co)
		{	
			return frac(sin(dot(co.xyz ,float3(12.9898,78.233,45.5432))) * 43758.5453);
		}
 
		float rand2(float3 co)
		{
			return frac(sin(dot(co.xyz ,float3(19.9128,75.2,34.5122))) * 12765.5213);
		}

		struct Input {
			float4 color: Color;
		};

		float _WaveLength;
		float _WaveHeight;
		float _WaveSpeed;
		float _RandomHeight;
		float _RandomSpeed;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		//Vertex Modifications
		void vert (inout appdata_full v) {
			float3 v0 = mul(unity_ObjectToWorld, v.vertex).xyz;

			float phase0 = (_WaveHeight)* sin((_Time[1] * _WaveSpeed) + (v0.x * _WaveLength) + (v0.z * _WaveLength) + rand2(v0.xzz));
			float phase0_1 = (_RandomHeight)*sin(cos(rand(v0.xzz) * _RandomHeight * cos(_Time[1] * _RandomSpeed * sin(rand(v0.xxz)))));
 
			v.vertex.y += phase0 + phase0_1;
     	 }

     	//Add color
		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = IN.color;	
		}
		ENDCG
	}
	FallBack "Diffuse"
}
