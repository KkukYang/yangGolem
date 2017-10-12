﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Custom/OutLine"
{
    Properties {
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _Outline ("Outline width", Range (0, 0.1)) = .005
        //_MainTex ("Base (RGB)", 2D) = "white" { }
    }
 
    SubShader {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            // Pass drawing outline
            Cull Front
       
            Blend SrcAlpha OneMinusSrcAlpha
           
            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag
           
            uniform float _Outline;
            uniform float4 _OutlineColor;
            //uniform float4 _MainTex_ST;
           // uniform sampler2D _MainTex;
 
            struct v2f
            {
                float4 pos : POSITION;
                float4 color : COLOR;
            };
           
            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos (v.vertex);
                float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
                float2 offset = TransformViewToProjection(norm.xy);
                o.pos.xy += offset  * _Outline;
                o.color = _OutlineColor;
                return o;
            }
           
            half4 frag(v2f i) :COLOR
            {
                return i.color;
            }
                   
            ENDCG
        }
       
    }
   
    Fallback Off
}