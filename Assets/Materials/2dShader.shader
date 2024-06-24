Shader "Custom/2dShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Angle("Angle", Float) = 45
    }

        SubShader
        {
            Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
            LOD 100

            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    UNITY_FOG_COORDS(1)
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float _Angle;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    UNITY_TRANSFER_FOG(o,o.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // Calculate angle correction
                    float angleCorrection = 1.0 / tan(radians(_Angle));

                // Apply correction to UV coordinates
                float2 correctedUV = i.uv * float2(angleCorrection, 1.0);

                // Sample the texture
                fixed4 c = tex2D(_MainTex, correctedUV);

                // Output the color
                return c;
            }
            ENDCG
        }
        }
}
