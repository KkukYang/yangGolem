// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:0,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:True,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:0,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:379,x:33014,y:32828,varname:node_379,prsc:2|diff-318-OUT,emission-318-OUT,alpha-9067-A,refract-1179-OUT,olwid-8339-OUT,olcol-566-RGB;n:type:ShaderForge.SFN_Tex2d,id:9067,x:32185,y:32006,ptovrint:False,ptlb:BasicColor,ptin:_BasicColor,varname:node_9067,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Fresnel,id:1683,x:31719,y:33109,varname:node_1683,prsc:2|EXP-5265-OUT;n:type:ShaderForge.SFN_Color,id:829,x:31914,y:32677,ptovrint:False,ptlb:FresnelColor,ptin:_FresnelColor,varname:node_829,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:3177,x:32119,y:32724,varname:node_3177,prsc:2|A-829-RGB,B-1986-OUT,C-4809-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5265,x:31501,y:33086,ptovrint:False,ptlb:FresnelPower,ptin:_FresnelPower,varname:node_5265,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:4;n:type:ShaderForge.SFN_ValueProperty,id:4809,x:31914,y:32987,ptovrint:False,ptlb:FresnelIntensity,ptin:_FresnelIntensity,varname:node_4809,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Append,id:5378,x:32249,y:33060,varname:node_5378,prsc:2|A-1986-OUT,B-1986-OUT;n:type:ShaderForge.SFN_Multiply,id:1179,x:32475,y:33079,varname:node_1179,prsc:2|A-5378-OUT,B-257-OUT;n:type:ShaderForge.SFN_ValueProperty,id:257,x:32249,y:33205,ptovrint:False,ptlb:RefractionIntensity,ptin:_RefractionIntensity,varname:node_257,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Add,id:318,x:32844,y:32859,varname:node_318,prsc:2|A-6184-OUT,B-3177-OUT;n:type:ShaderForge.SFN_Slider,id:8339,x:32475,y:33224,ptovrint:False,ptlb:Outline,ptin:_Outline,varname:node_8339,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.04273505,max:1;n:type:ShaderForge.SFN_Color,id:566,x:32632,y:33313,ptovrint:False,ptlb:OutlineColor,ptin:_OutlineColor,varname:node_566,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Dot,id:4296,x:31542,y:32789,varname:node_4296,prsc:2,dt:0|A-4419-OUT,B-1925-OUT;n:type:ShaderForge.SFN_NormalVector,id:4419,x:31368,y:32708,prsc:2,pt:False;n:type:ShaderForge.SFN_Subtract,id:7978,x:31708,y:32789,varname:node_7978,prsc:2|A-5966-OUT,B-4296-OUT;n:type:ShaderForge.SFN_Vector1,id:5966,x:31542,y:32729,varname:node_5966,prsc:2,v1:1;n:type:ShaderForge.SFN_Power,id:1986,x:31884,y:32819,varname:node_1986,prsc:2|VAL-7978-OUT,EXP-5265-OUT;n:type:ShaderForge.SFN_Vector3,id:1925,x:31368,y:32865,varname:node_1925,prsc:2,v1:0,v2:0,v3:-1;n:type:ShaderForge.SFN_Color,id:5505,x:32185,y:32182,ptovrint:False,ptlb:AmbientColor,ptin:_AmbientColor,varname:node_5505,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_NormalVector,id:1635,x:31843,y:32404,prsc:2,pt:False;n:type:ShaderForge.SFN_Vector3,id:8921,x:31843,y:32313,varname:node_8921,prsc:2,v1:0,v2:-1,v3:0;n:type:ShaderForge.SFN_Dot,id:7742,x:32012,y:32351,varname:node_7742,prsc:2,dt:0|A-8921-OUT,B-1635-OUT;n:type:ShaderForge.SFN_Clamp01,id:6638,x:32197,y:32351,varname:node_6638,prsc:2|IN-7742-OUT;n:type:ShaderForge.SFN_Lerp,id:6184,x:32505,y:32543,varname:node_6184,prsc:2|A-9067-RGB,B-5505-RGB,T-6638-OUT;proporder:9067-829-5265-4809-257-8339-566-5505;pass:END;sub:END;*/

Shader "BurningFist/Slime" {
    Properties {
        _BasicColor ("BasicColor", 2D) = "white" {}
        _FresnelColor ("FresnelColor", Color) = (0.5,0.5,0.5,1)
        _FresnelPower ("FresnelPower", Float ) = 4
        _FresnelIntensity ("FresnelIntensity", Float ) = 2
        _RefractionIntensity ("RefractionIntensity", Float ) = 2
        _Outline ("Outline", Range(0, 1)) = 0.04273505
        _OutlineColor ("OutlineColor", Color) = (0.5,0.5,0.5,1)
        _AmbientColor ("AmbientColor", Color) = (0.5,0.5,0.5,1)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
        GrabPass{ }
        Pass {
            Name "Outline"
            Tags {
            }
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _Outline;
            uniform float4 _OutlineColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_FOG_COORDS(0)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( float4(v.vertex.xyz + v.normal*_Outline,1) );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                return fixed4(_OutlineColor.rgb,0);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform sampler2D _BasicColor; uniform float4 _BasicColor_ST;
            uniform float4 _FresnelColor;
            uniform float _FresnelPower;
            uniform float _FresnelIntensity;
            uniform float _RefractionIntensity;
            uniform float4 _AmbientColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float4 projPos : TEXCOORD2;
                UNITY_FOG_COORDS(3)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float3 normalDirection = i.normalDir;
                float node_1986 = pow((1.0-dot(i.normalDir,float3(0,0,-1))),_FresnelPower);
                float2 sceneUVs = (i.projPos.xy / i.projPos.w) + (float2(node_1986,node_1986)*_RefractionIntensity);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
////// Lighting:
////// Emissive:
                float4 _BasicColor_var = tex2D(_BasicColor,TRANSFORM_TEX(i.uv0, _BasicColor));
                float3 node_318 = (lerp(_BasicColor_var.rgb,_AmbientColor.rgb,saturate(dot(float3(0,-1,0),i.normalDir)))+(_FresnelColor.rgb*node_1986*_FresnelIntensity));
                float3 emissive = node_318;
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(lerp(sceneColor.rgb, finalColor,_BasicColor_var.a),1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
