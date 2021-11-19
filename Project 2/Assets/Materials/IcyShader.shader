// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Icy"
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

			// Julia set constants
			// c = -0.43 + 0.58i is a magic number
			static const float c_x = -0.43;
            static const float c_y = 0.58;
			static const float iterations = 200;

			float4 vertex_shader (float4 vertex:POSITION):SV_POSITION
			{
				return UnityObjectToClipPos(vertex);
			}

			float4 pixel_shader (float4 vertex:SV_POSITION):COLOR
			{
				// ------- Find distance from centre ----------

				// get centre coordinates
				float2 centre = _ScreenParams.xy * 0.5;
				// float radius = 0.25 * _ScreenParams.y;

				// find distance between pixel and centre, divide by screen height
				float dist = distance(vertex.xy, centre)/_ScreenParams.y;
				
				// icy proportional to distance
				float icyProportion = 0.35 * dist*dist*dist*dist;

				// fade in ice
				icyProportion *= _TransitionAmount;

                // -------- Julia Set----------

				// Scale coords to be roughly between -2 and 2 in local area
				float2 sCoords = 0.2 * (2 * vertex.xy - _ScreenParams.xy) / _ScreenParams.y;
			
                
                // find initial z (real/imaginary components)
                float z_x = sCoords.x;
                float z_y = sCoords.y;

                float temp_z_x;
                float temp_z_y;

				// save whether still in julia set after iteration
				float juliaProportion = 0.8;

                // for each iteration, find z = z*z + c
                for (uint i = 0; i < iterations; i++) {
					// if z is outside julia set, reduction juliaProportion
                    if ( length(float2(z_x, z_y)) > 2) {
                        juliaProportion = clamp(0.8 * i / iterations, 0.2, juliaProportion);
                        break;
                    }

                    // find z*z
                    temp_z_x = z_x * z_x - z_y * z_y; // real term
                    temp_z_y = z_x * z_y + z_x * z_y; // imaginary term

                    // find z*z + c
                    z_x = temp_z_x + c_x;  // real term
                    z_y = temp_z_y + c_y;  // imaginary term

                }

				// layer julia component on icyProportion
                icyProportion *= juliaProportion;

				// -------------------------------

				// get pixel uv coordinate
				vector <float,2> uv = vertex.xy/_ScreenParams.xy;

				// get original colour of pixel
				float4 output = tex2D(_MainTex, uv);

				output.rgb = (1 - icyProportion) * output.rgb  +  (icyProportion) * float3(174, 219, 240)/256;

				// Uncomment to test fractal image:
				// output = float4(1.0, 1.0, 1.0, 1.0);
				// output.rgb *= juliaProportion;

				return output;

			}
			ENDCG
		}
	}
}