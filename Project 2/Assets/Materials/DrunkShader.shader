// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Drunk"
{
	Properties
	{
		_TransitionAmount ("Transition Amount", float) = 0
		_MainTex ("Texture", 2D) = "black" {}
	}
	Subshader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vertex_shader
			#pragma fragment pixel_shader
			#pragma target 2.0

			sampler2D _MainTex;

			uniform float _TransitionAmount;

			float4 vertex_shader (float4 vertex:POSITION):SV_POSITION
			{
				return UnityObjectToClipPos(vertex);
			}

			float4 pixel_shader (float4 vertex:SV_POSITION):COLOR
			{
				vector <float,2> uv = vertex.xy/_ScreenParams.xy;

				//Screen shake
				uv.x += _TransitionAmount*cos(1.3*(uv.y*2.0+_Time.g))*0.025;
				uv.y += _TransitionAmount*sin(uv.x*2.0+_Time.g)*0.025;


				//Bluring
				float offset = _TransitionAmount*sin(_Time.g *0.5) * 0.01;
				
				float4 a = tex2D(_MainTex,uv);    
				float4 b = tex2D(_MainTex,uv-float2(offset,0.0));    
				float4 c = tex2D(_MainTex,uv+float2(offset,0.0));    
				float4 d = tex2D(_MainTex,uv-float2(0.0,offset));    
				float4 e = tex2D(_MainTex,uv+float2(0.0,offset));  
				float4 average = (a+b+c+d+e)/5.0;

				//colour waves
				float color_proportion = (1.5 + sin(1*(_Time.g + cos((0.5-uv.x)*(0.5-uv.x) + (0.5-uv.y)*(0.5-uv.y)))))/2.5;
				float drunk_color_effect = _TransitionAmount*0.2;
				average.rgb =  (1.0 - drunk_color_effect)*average.rgb + drunk_color_effect*(1.0 - color_proportion)*average.rgb + drunk_color_effect*color_proportion*float3(201, 110, 18)/256;
				
				return average;
			}
			ENDCG
		}
	}
}