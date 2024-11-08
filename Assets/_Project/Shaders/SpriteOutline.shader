Shader "Custom/SpriteOutline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineWidth ("Outline Width", Range(0, 16)) = 2
        _AlphaCutoff ("Alpha Cutoff", Range(0, 1)) = 0.1
        [Toggle] _PulsingEffect ("Enable Pulsing", Float) = 0
        _PulseSpeed ("Pulse Speed", Range(0, 10)) = 2
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            fixed4 _RendererColor;
            fixed4 _OutlineColor;
            float _OutlineWidth;
            float _AlphaCutoff;
            float _PulsingEffect;
            float _PulseSpeed;
            float4 _Flip;
            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                IN.vertex.xy *= _Flip.xy;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _RendererColor;
                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap(OUT.vertex);
                #endif

                return OUT;
            }

            fixed4 SampleSpriteTexture(float2 uv)
            {
                fixed4 color = tex2D(_MainTex, uv);
                return color;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
                
                // Calculate outline
                fixed outline = 0;
                
                // Check near pixels for the outline
                float2 texelSize = _MainTex_TexelSize.xy * _OutlineWidth;
                fixed4 pixelUp = tex2D(_MainTex, IN.texcoord + float2(0, texelSize.y));
                fixed4 pixelDown = tex2D(_MainTex, IN.texcoord - float2(0, texelSize.y));
                fixed4 pixelRight = tex2D(_MainTex, IN.texcoord + float2(texelSize.x, 0));
                fixed4 pixelLeft = tex2D(_MainTex, IN.texcoord - float2(texelSize.x, 0));
                
                outline = max(outline, pixelUp.a);
                outline = max(outline, pixelDown.a);
                outline = max(outline, pixelRight.a);
                outline = max(outline, pixelLeft.a);
                
                // Remove outline from the sprite's area
                outline = (outline - c.a) > _AlphaCutoff ? 1 : 0;
                
                // Pulse effect
                fixed4 outlineColor = _OutlineColor;
                if (_PulsingEffect)
                {
                    float pulse = (sin(_Time.y * _PulseSpeed) + 1.0) * 0.5;
                    outlineColor.a *= lerp(0.5, 1.0, pulse);
                }
                
                // Mix sprite and outline
                fixed4 result = lerp(c, outlineColor, outline * outlineColor.a);
                result.rgb *= result.a;
                
                return result;
            }
            ENDCG
        }
    }
}
