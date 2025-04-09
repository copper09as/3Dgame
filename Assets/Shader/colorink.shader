Shader "Unlit/colorink"
{
    Properties
    {
        _MainTex("Main", 2D) = "white" {}
        _Thred("Edge Thred", Range(0.01, 1)) = 0.25
        _Range("Edge Range", Range(1, 10)) = 1		
        _Pow("Edge Intensity", Range(0, 10)) = 1
        _BrushTex("Brush Texture", 2D) = "white" {}

        [Enum(Opacity,1,Darken,2,Lighten,3,Multiply,4,Screen,5,Overlay,6,SoftLight,7)]
        _BlendType("Blend Type", Int) = 1

        _SaturationBoost("Saturation Boost", Range(0, 2)) = 1.2
        _ShadowThreshold("Shadow Protect Threshold", Range(0, 0.3)) = 0.1
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float viewNormalDot : TEXCOORD1;
                float4 vertex : SV_POSITION;
                UNITY_FOG_COORDS(2)
            };

            sampler2D _MainTex;
            sampler2D _BrushTex;
            float4 _MainTex_ST;
            float _Thred;
            float _Range;
            float _Pow;
            int _BlendType;
            float _SaturationBoost;
            float _ShadowThreshold;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float3 viewDir = normalize(_WorldSpaceCameraPos - worldPos);
                o.viewNormalDot = dot(normalize(worldNormal), viewDir);

                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 BlendColor(fixed4 baseCol, fixed4 brushCol, int blendType)
            {
                fixed4 result;
                if (blendType == 1)         result = lerp(baseCol, brushCol, brushCol.a);
                else if (blendType == 2)    result = min(baseCol, brushCol);
                else if (blendType == 3)    result = max(baseCol, brushCol);
                else if (blendType == 4)    result = baseCol * brushCol;
                else if (blendType == 5)    result = 1 - (1 - baseCol) * (1 - brushCol);
                else if (blendType == 6)    result = lerp(2 * baseCol * brushCol, 1 - 2 * (1 - baseCol)*(1 - brushCol), step(0.5, baseCol));
                else if (blendType == 7)    result = lerp(sqrt(baseCol) * brushCol * 2, 1 - (1 - baseCol) * (1 - brushCol) * 2, step(0.5, brushCol));
                else                        result = baseCol;

                result.a = 1;
                return result;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 baseCol = tex2D(_MainTex, i.uv);
                fixed4 brushCol = tex2D(_BrushTex, i.uv);
                fixed4 blended = BlendColor(baseCol, brushCol, _BlendType);

                // ---------- 保留黑色 ----------
                float brightness = dot(blended.rgb, float3(0.299, 0.587, 0.114));

                if (brightness > _ShadowThreshold)
                {
                    // ---- 卡通色阶 ----
                    float toonStep;
                    if (brightness > 0.8)
                        toonStep = 1.0;
                    else if (brightness > 0.5)
                        toonStep = 0.7;
                    else if (brightness > 0.25)
                        toonStep = 0.4;
                    else
                        toonStep = 0.2;

                    blended.rgb = normalize(blended.rgb + 1e-5) * toonStep;

                    // ---- 提鲜 ----
                    float avg = dot(blended.rgb, float3(0.333, 0.333, 0.333));
                    blended.rgb = lerp(avg.xxx, blended.rgb, _SaturationBoost);
                }
                // 否则是暗部，保留原始颜色不动

                // ---------- 边缘处理 ----------
                float edge = pow(i.viewNormalDot, 1) / _Range;
                edge = edge > _Thred ? 1 : edge;
                edge = pow(edge, _Pow);
                fixed4 edgeColor = fixed4(edge, edge, edge, edge);

                // ---------- 合成 ----------
                fixed4 finalCol = edgeColor * (1 - edgeColor.a) + blended * edgeColor.a;
                UNITY_APPLY_FOG(i.fogCoord, finalCol);
                return finalCol;
            }
            ENDCG
        }
    }
}
