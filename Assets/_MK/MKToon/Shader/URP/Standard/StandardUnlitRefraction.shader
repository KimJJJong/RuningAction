﻿//////////////////////////////////////////////////////
// MK Toon URP Standard Unlit + Refraction			//
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.            //
//////////////////////////////////////////////////////

Shader "MK/Toon/URP/Standard/Unlit + Refraction"
{
	Properties
	{
		/////////////////
		// Options     //
		/////////////////
		[Enum(MK.Toon.SurfaceRefraction)] _Surface ("", int) = 1
		_Blend ("", int) = 0
		[Toggle] _AlphaClipping ("", int) = 0
		[Enum(MK.Toon.RenderFace)] _RenderFace ("", int) = 2

		/////////////////
		// Input       //
		/////////////////
		[MainColor] _AlbedoColor ("", Color) = (1,1,1,1)
		_AlphaCutoff ("", Range(0, 1)) = 0.5
		[MainTexture] _AlbedoMap ("", 2D) = "white" {}
		_AlbedoMapIntensity ("", Range(0, 1)) = 1.0

		/////////////////
		// Stylize     //
		/////////////////
		_Contrast ("", Float) = 1.0
		_Hue ("", Range(0.0, 1.0)) = 0.0
		[MKToonSaturation] _Saturation ("", Float) = 1.0
		[MKToonBrightness] _Brightness ("",  Float) = 1
		[Enum(MK.Toon.ColorGrading)] _ColorGrading ("", int) = 0
		[Toggle] _VertexAnimationStutter ("", int) = 0
		[Enum(MK.Toon.VertexAnimation)] _VertexAnimation ("", int) = 0
        _VertexAnimationIntensity ("", Range(0, 1)) = 0.05
		_VertexAnimationMap ("", 2D) = "white" {}
		_NoiseMap ("", 2D) = "white" {}
        [MKToonVector3Drawer] _VertexAnimationFrequency ("", Vector) = (2.5, 2.5, 2.5, 1)
		[Enum(MK.Toon.Dissolve)] _Dissolve ("", int) = 0
		_DissolveMapScale ("", Float) = 1
		_DissolveMap ("", 2D) = "white" {}
		_DissolveAmount ("", Range(0.0, 1.0)) = 0.5
		_DissolveBorderSize ("", Range(0.0, 1.0)) = 0.25
		_DissolveBorderRamp ("", 2D) = "white" {}
		[HDR] _DissolveBorderColor ("", Color) = (1, 1, 1, 1)

		/////////////////
		// Advanced    //
		/////////////////
		[HideInInspector] [Enum(MK.Toon.BlendFactor)] _BlendSrc ("", int) = 1
		[HideInInspector] [Enum(MK.Toon.BlendFactor)] _BlendDst ("", int) = 0
		[HideInInspector] [Enum(MK.Toon.BlendFactor)] _BlendSrcAlpha ("", int) = 1
		[HideInInspector] [Enum(MK.Toon.BlendFactor)] _BlendDstAlpha ("", int) = 0
		[Enum(MK.Toon.ZWrite)] _ZWrite ("", int) = 0
		[Enum(MK.Toon.ZTest)] _ZTest ("", int) = 4.0
		[MKToonRenderPriority] _RenderPriority ("", Range(-50, 50)) = 0.0

		[Enum(MK.Toon.Stencil)] _Stencil ("", Int) = 1
		[MKToonStencilRef] _StencilRef ("", Range(0, 255)) = 0
		[MKToonStencilReadMask] _StencilReadMask ("", Range(0, 255)) = 255
		[MKToonStencilWriteMask] _StencilWriteMask ("", Range(0, 255)) = 255
		[Enum(MK.Toon.StencilComparison)] _StencilComp ("", Int) = 8
		[Enum(MK.Toon.StencilOperation)] _StencilPass ("", Int) = 0
		[Enum(MK.Toon.StencilOperation)] _StencilFail ("", Int) = 0
		[Enum(MK.Toon.StencilOperation)] _StencilZFail ("", Int) = 0
		[HideInInspector] _AddPrecomputedVelocity("_AddPrecomputedVelocity", Float) = 0.0

		/////////////////
		// Refraction  //
		/////////////////
		_RefractionDistortionMapScale ("", Float) = 1.0
		[Normal] _RefractionDistortionMap ("", 2D) = "bump" {}
		_RefractionDistortion ("", Float) = 0.1
		_IndexOfRefraction ("", Range(0, 0.5)) = 0.0
		_RefractionDistortionFade ("", Range(0.0, 1.0)) = 0.5

		/////////////////
		// Editor Only //
		/////////////////
		[HideInInspector] _Initialized ("", int) = 0
        [HideInInspector] _OptionsTab ("", int) = 1
		[HideInInspector] _InputTab ("", int) = 1
		[HideInInspector] _StylizeTab ("", int) = 0
		[HideInInspector] _AdvancedTab ("", int) = 0
		[HideInInspector] _RefractionTab ("", int) = 0

		/////////////////
		// System	   //
		/////////////////
		[HideInInspector] [Toggle] _AlembicMotionVectors("_AlembicMotionVectors", Float) = 0.0
		[HideInInspector] _Cutoff ("", Range(0, 1)) = 0.5
		[HideInInspector] _MainTex ("", 2D) = "white" {}
		[HideInInspector] _Color ("", Color) = (1,1,1,1)

		[HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
	}
	SubShader
	{
		Tags { "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

		/////////////////////////////////////////////////////////////////////////////////////////////
		// FORWARD BASE
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			PackageRequirements 
			{
				"com.unity.render-pipelines.universal":"[7.0,11.99]"
			}
			Stencil
			{
				Ref [_StencilRef]
				ReadMask [_StencilReadMask]
				WriteMask [_StencilWriteMask]
				Comp [_StencilComp]
				Pass [_StencilPass]
				Fail [_StencilFail]
				ZFail [_StencilZFail]
			}

			Tags { "LightMode" = "UniversalForward" }
			Name "ForwardBase" 
			Cull [_RenderFace]
			Blend [_BlendSrc] [_BlendDst], [_BlendSrcAlpha] [_BlendDstAlpha]
			ZWrite [_ZWrite]
			ZTest [_ZTest]

			HLSLPROGRAM
			#pragma target 4.5
			#pragma exclude_renderers gles gles3 glcore d3d11_9x wiiu n3ds switch
			#pragma shader_feature_local __ _MK_SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local __ _MK_ALPHA_CLIPPING
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_SINE _MK_VERTEX_ANIMATION_PULSE _MK_VERTEX_ANIMATION_NOISE
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_MAP
			#pragma shader_feature_local __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_BORDER_COLOR _MK_DISSOLVE_BORDER_RAMP
			#pragma shader_feature_local __ _MK_ALBEDO_MAP
            #pragma shader_feature_local __ _MK_BLEND_PREMULTIPLY _MK_BLEND_ADDITIVE _MK_BLEND_MULTIPLY
			#pragma shader_feature_local __ _MK_COLOR_GRADING_ALBEDO _MK_COLOR_GRADING_FINAL_OUTPUT
			#pragma shader_feature_local __ _MK_REFRACTION_DISTORTION_MAP
			#pragma shader_feature_local __ _MK_INDEX_OF_REFRACTION
			#if UNITY_VERSION >= 202120
				#pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
			#endif

			#if UNITY_VERSION >= 202320
				#include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
			#endif

			#if UNITY_VERSION >= 202220
				#pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
				#if UNITY_VERSION >= 202310
					#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
				#else
					#pragma multi_compile_fragment _ _WRITE_RENDERING_LAYERS
				#endif
			#endif

			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex ForwardVert
			#pragma fragment ForwardFrag

            #if UNITY_VERSION >= 61000000
				#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Fog.hlsl"
			#else
				#pragma multi_compile_fog
			#endif
			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer

			#define MK_URP
			#define MK_REFRACTION
			#define MK_UNLIT
			#define MK_STANDARD
			#include_with_pragmas "../../Lib/DotsInstancingSetup.hlsl"

			#include "../../Lib/Forward/BaseSetup.hlsl"
			
			ENDHLSL
		}

		Pass
		{
			PackageRequirements 
			{
				"com.unity.render-pipelines.universal":"[12.0,99.99]"
			}
			Stencil
			{
				Ref [_StencilRef]
				ReadMask [_StencilReadMask]
				WriteMask [_StencilWriteMask]
				Comp [_StencilComp]
				Pass [_StencilPass]
				Fail [_StencilFail]
				ZFail [_StencilZFail]
			}

			Tags { "LightMode" = "UniversalForwardOnly" }
			Name "ForwardBase" 
			Cull [_RenderFace]
			Blend [_BlendSrc] [_BlendDst], [_BlendSrcAlpha] [_BlendDstAlpha]
			ZWrite [_ZWrite]
			ZTest [_ZTest]

			HLSLPROGRAM
			#pragma target 4.5
			#pragma exclude_renderers gles gles3 glcore d3d11_9x wiiu n3ds switch
			#pragma shader_feature_local __ _MK_SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local __ _MK_ALPHA_CLIPPING
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_SINE _MK_VERTEX_ANIMATION_PULSE _MK_VERTEX_ANIMATION_NOISE
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_MAP
			#pragma shader_feature_local __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_BORDER_COLOR _MK_DISSOLVE_BORDER_RAMP
			#pragma shader_feature_local __ _MK_ALBEDO_MAP
            #pragma shader_feature_local __ _MK_BLEND_PREMULTIPLY _MK_BLEND_ADDITIVE _MK_BLEND_MULTIPLY
			#pragma shader_feature_local __ _MK_COLOR_GRADING_ALBEDO _MK_COLOR_GRADING_FINAL_OUTPUT
			#pragma shader_feature_local __ _MK_REFRACTION_DISTORTION_MAP
			#pragma shader_feature_local __ _MK_INDEX_OF_REFRACTION
			#if UNITY_VERSION >= 202120
				#pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
			#endif

			#if UNITY_VERSION >= 202320
				#include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
			#endif

			#if UNITY_VERSION >= 202220
				#pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
				#if UNITY_VERSION >= 202310
					#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
				#else
					#pragma multi_compile_fragment _ _WRITE_RENDERING_LAYERS
				#endif
			#endif

			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex ForwardVert
			#pragma fragment ForwardFrag

            #if UNITY_VERSION >= 61000000
				#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Fog.hlsl"
			#else
				#pragma multi_compile_fog
			#endif
			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer

			#define MK_URP
			#define MK_REFRACTION
			#define MK_UNLIT
			#define MK_STANDARD
			#include_with_pragmas "../../Lib/DotsInstancingSetup.hlsl"

			#include "../../Lib/Forward/BaseSetup.hlsl"
			
			ENDHLSL
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// FORWARD ADD
		/////////////////////////////////////////////////////////////////////////////////////////////

		/////////////////////////////////////////////////////////////////////////////////////////////
		// DEFERRED
		/////////////////////////////////////////////////////////////////////////////////////////////

		/////////////////////////////////////////////////////////////////////////////////////////////
		// SHADOWCASTER
		/////////////////////////////////////////////////////////////////////////////////////////////
		
		/////////////////////////////////////////////////////////////////////////////////////////////
		// META
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			Tags { "LightMode"="Meta" }
			Name "Meta" 

			Cull Off

			HLSLPROGRAM
			#pragma target 4.5
			#pragma exclude_renderers gles gles3 glcore d3d11_9x wiiu n3ds switch
			#pragma vertex MetaVert
			#pragma fragment MetaFrag
			#pragma fragmentoption ARB_precision_hint_fastest

			#pragma shader_feature_local __ _MK_ALBEDO_MAP
			#pragma shader_feature_local __ _MK_ALPHA_CLIPPING
			#pragma shader_feature_local __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_BORDER_COLOR _MK_DISSOLVE_BORDER_RAMP
			#pragma shader_feature_local __ _MK_COLOR_GRADING_ALBEDO _MK_COLOR_GRADING_FINAL_OUTPUT

			#define MK_URP
			#define MK_UNLIT
			#define MK_STANDARD
			#include_with_pragmas "../../Lib/DotsInstancingSetup.hlsl"

			#include "../../Lib/Meta/Setup.hlsl"
			ENDHLSL
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// Depth Only
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
        {
            Name "DepthOnly"
            Tags { "LightMode" = "DepthOnly" }

            ZWrite On
            ColorMask R
            Cull [_RenderFace]

            HLSLPROGRAM
            #pragma target 4.5
			#pragma exclude_renderers gles gles3 glcore d3d11_9x wiiu n3ds switch

            #pragma vertex DepthOnlyVert
            #pragma fragment DepthOnlyFrag

			#pragma shader_feature_local __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_BORDER_COLOR _MK_DISSOLVE_BORDER_RAMP
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_SINE _MK_VERTEX_ANIMATION_PULSE _MK_VERTEX_ANIMATION_NOISE
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_MAP
            #pragma shader_feature_local __ _MK_ALBEDO_MAP
            #pragma shader_feature_local __ _MK_ALPHA_CLIPPING

			#if UNITY_VERSION >= 202220
				#pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
			#endif

            #pragma multi_compile_instancing

			#define MK_URP
			#define MK_UNLIT
			#define MK_STANDARD
			#include_with_pragmas "../../Lib/DotsInstancingSetup.hlsl"

            #include "../../Lib/DepthOnly/Setup.hlsl"
            ENDHLSL
        }

		/////////////////////////////////////////////////////////////////////////////////////////////
		// Depth Normals
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
        {
            Name "DepthNormals"
            Tags { "LightMode" = "DepthNormals" }

            ZWrite On
            Cull [_RenderFace]

            HLSLPROGRAM
            #pragma target 4.5
			#pragma exclude_renderers gles gles3 glcore d3d11_9x wiiu n3ds switch

            #pragma vertex DepthNormalsVert
            #pragma fragment DepthNormalsFrag

			#pragma shader_feature_local __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_BORDER_COLOR _MK_DISSOLVE_BORDER_RAMP
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_SINE _MK_VERTEX_ANIMATION_PULSE _MK_VERTEX_ANIMATION_NOISE
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_MAP
            #pragma shader_feature_local __ _MK_ALBEDO_MAP
            #pragma shader_feature_local __ _MK_ALPHA_CLIPPING

			#if UNITY_VERSION >= 202220
				#pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
				#if UNITY_VERSION >= 202310
					#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
				#else
					#pragma multi_compile_fragment _ _WRITE_RENDERING_LAYERS
				#endif
			#endif

            #pragma multi_compile_instancing

			#define MK_URP
			#define MK_UNLIT
			#define MK_STANDARD
			#include_with_pragmas "../../Lib/DotsInstancingSetup.hlsl"

            #include "../../Lib/DepthNormals/Setup.hlsl"
            ENDHLSL
        }

		/////////////////////////////////////////////////////////////////////////////////////////////
		// Motion Vectors
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
        {
			PackageRequirements 
			{
				"com.unity.render-pipelines.universal":"[16.0,99.99]"
			}
            Name "MotionVectors"
            Tags { "LightMode" = "MotionVectors" }

            ZWrite On
            Cull [_RenderFace]
			ColorMask RG

            HLSLPROGRAM
            #pragma target 4.5
			#pragma exclude_renderers gles gles3 glcore d3d11_9x wiiu n3ds switch

            #pragma vertex MotionVectorsVert
            #pragma fragment MotionVectorsFrag
			#pragma shader_feature_local_vertex _ADD_PRECOMPUTED_VELOCITY

			#pragma shader_feature_local __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_BORDER_COLOR _MK_DISSOLVE_BORDER_RAMP
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_SINE _MK_VERTEX_ANIMATION_PULSE _MK_VERTEX_ANIMATION_NOISE
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_MAP
            #pragma shader_feature_local __ _MK_ALBEDO_MAP
            #pragma shader_feature_local __ _MK_ALPHA_CLIPPING

            #pragma multi_compile_instancing

			#if UNITY_VERSION >= 202220
				#pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
			#endif

			#define MK_URP
			#define MK_UNLIT
			#define MK_STANDARD

            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ObjectMotionVectors.hlsl"
            #include "../../Lib/MotionVectors/Setup.hlsl"
            ENDHLSL
        }

		/////////////////////////////////////////////////////////////////////////////////////////////
		// Universal2D
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
        {
            Name "Universal2D"
            Tags{ "LightMode" = "Universal2D" }

            Blend [_BlendSrc] [_BlendDst]
            ZWrite [_ZWrite]
            Cull [_RenderFace]

            HLSLPROGRAM
            
            
			#pragma target 4.5
			#pragma exclude_renderers gles gles3 glcore d3d11_9x wiiu n3ds switch

            #pragma vertex Universal2DVert
            #pragma fragment Universal2DFrag
			
			#pragma shader_feature_local __ _MK_SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local __ _MK_ALBEDO_MAP
			#pragma shader_feature_local __ _MK_ALPHA_CLIPPING
            #pragma shader_feature_local __ _MK_BLEND_PREMULTIPLY _MK_BLEND_ADDITIVE _MK_BLEND_MULTIPLY
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_SINE _MK_VERTEX_ANIMATION_PULSE _MK_VERTEX_ANIMATION_NOISE
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_MAP
			#pragma shader_feature_local __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_BORDER_COLOR _MK_DISSOLVE_BORDER_RAMP
			#pragma shader_feature_local __ _MK_COLOR_GRADING_ALBEDO _MK_COLOR_GRADING_FINAL_OUTPUT

            #define MK_URP
			#define MK_UNLIT
			#define MK_STANDARD
			#include_with_pragmas "../../Lib/DotsInstancingSetup.hlsl"

            #include "../../Lib/Universal2D/Setup.hlsl"

            ENDHLSL
        }
    }
	
	/////////////////////////////////////////////////////////////////////////////////////////////
	// SM 3.5
	/////////////////////////////////////////////////////////////////////////////////////////////
	SubShader
	{
		Tags { "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

		/////////////////////////////////////////////////////////////////////////////////////////////
		// FORWARD BASE
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			PackageRequirements 
			{
				"com.unity.render-pipelines.universal":"[7.0,11.99]"
			}
			Stencil
			{
				Ref [_StencilRef]
				ReadMask [_StencilReadMask]
				WriteMask [_StencilWriteMask]
				Comp [_StencilComp]
				Pass [_StencilPass]
				Fail [_StencilFail]
				ZFail [_StencilZFail]
			}

			Tags { "LightMode" = "UniversalForward" }
			Name "ForwardBase" 
			Cull [_RenderFace]
			Blend [_BlendSrc] [_BlendDst], [_BlendSrcAlpha] [_BlendDstAlpha]
			ZWrite [_ZWrite]
			ZTest [_ZTest]

			HLSLPROGRAM
			#pragma target 3.5
			#pragma exclude_renderers gles d3d11_9x ps4 ps5 xboxone
			#pragma shader_feature_local __ _MK_SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local __ _MK_ALPHA_CLIPPING
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_SINE _MK_VERTEX_ANIMATION_PULSE _MK_VERTEX_ANIMATION_NOISE
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_MAP
			#pragma shader_feature_local __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_BORDER_COLOR _MK_DISSOLVE_BORDER_RAMP
			#pragma shader_feature_local __ _MK_ALBEDO_MAP
            #pragma shader_feature_local __ _MK_BLEND_PREMULTIPLY _MK_BLEND_ADDITIVE _MK_BLEND_MULTIPLY
			#pragma shader_feature_local __ _MK_COLOR_GRADING_ALBEDO _MK_COLOR_GRADING_FINAL_OUTPUT
			#pragma shader_feature_local __ _MK_REFRACTION_DISTORTION_MAP
			#pragma shader_feature_local __ _MK_INDEX_OF_REFRACTION
			#if UNITY_VERSION >= 202120
				#pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
			#endif

			#if UNITY_VERSION >= 202320
				#include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
			#endif

			#if UNITY_VERSION >= 202220
				#if UNITY_VERSION >= 202310
					#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
				#else
					#pragma multi_compile_fragment _ _WRITE_RENDERING_LAYERS
				#endif
				#pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
			#endif

			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex ForwardVert
			#pragma fragment ForwardFrag

            #if UNITY_VERSION >= 61000000
				#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Fog.hlsl"
			#else
				#pragma multi_compile_fog
			#endif
			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer

			#define MK_URP
			#define MK_REFRACTION
			#define MK_UNLIT
			#define MK_STANDARD
			#include_with_pragmas "../../Lib/DotsInstancingSetup.hlsl"

			#include "../../Lib/Forward/BaseSetup.hlsl"
			
			ENDHLSL
		}

		Pass
		{
			PackageRequirements 
			{
				"com.unity.render-pipelines.universal":"[12.0,99.99]"
			}
			Stencil
			{
				Ref [_StencilRef]
				ReadMask [_StencilReadMask]
				WriteMask [_StencilWriteMask]
				Comp [_StencilComp]
				Pass [_StencilPass]
				Fail [_StencilFail]
				ZFail [_StencilZFail]
			}

			Tags { "LightMode" = "UniversalForwardOnly" }
			Name "ForwardBase" 
			Cull [_RenderFace]
			Blend [_BlendSrc] [_BlendDst], [_BlendSrcAlpha] [_BlendDstAlpha]
			ZWrite [_ZWrite]
			ZTest [_ZTest]

			HLSLPROGRAM
			#pragma target 3.5
			#pragma exclude_renderers gles d3d11_9x ps4 ps5 xboxone
			#pragma shader_feature_local __ _MK_SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local __ _MK_ALPHA_CLIPPING
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_SINE _MK_VERTEX_ANIMATION_PULSE _MK_VERTEX_ANIMATION_NOISE
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_MAP
			#pragma shader_feature_local __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_BORDER_COLOR _MK_DISSOLVE_BORDER_RAMP
			#pragma shader_feature_local __ _MK_ALBEDO_MAP
            #pragma shader_feature_local __ _MK_BLEND_PREMULTIPLY _MK_BLEND_ADDITIVE _MK_BLEND_MULTIPLY
			#pragma shader_feature_local __ _MK_COLOR_GRADING_ALBEDO _MK_COLOR_GRADING_FINAL_OUTPUT
			#pragma shader_feature_local __ _MK_REFRACTION_DISTORTION_MAP
			#pragma shader_feature_local __ _MK_INDEX_OF_REFRACTION
			#if UNITY_VERSION >= 202120
				#pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
			#endif

			#if UNITY_VERSION >= 202320
				#include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
			#endif

			#if UNITY_VERSION >= 202220
				#if UNITY_VERSION >= 202310
					#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
				#else
					#pragma multi_compile_fragment _ _WRITE_RENDERING_LAYERS
				#endif
				#pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
			#endif

			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex ForwardVert
			#pragma fragment ForwardFrag

            #if UNITY_VERSION >= 61000000
				#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Fog.hlsl"
			#else
				#pragma multi_compile_fog
			#endif
			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer

			#define MK_URP
			#define MK_REFRACTION
			#define MK_UNLIT
			#define MK_STANDARD
			#include_with_pragmas "../../Lib/DotsInstancingSetup.hlsl"

			#include "../../Lib/Forward/BaseSetup.hlsl"
			
			ENDHLSL
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// FORWARD ADD
		/////////////////////////////////////////////////////////////////////////////////////////////

		/////////////////////////////////////////////////////////////////////////////////////////////
		// DEFERRED
		/////////////////////////////////////////////////////////////////////////////////////////////

		/////////////////////////////////////////////////////////////////////////////////////////////
		// SHADOWCASTER
		/////////////////////////////////////////////////////////////////////////////////////////////
		
		/////////////////////////////////////////////////////////////////////////////////////////////
		// META
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			Tags { "LightMode"="Meta" }
			Name "Meta" 

			Cull Off

			HLSLPROGRAM
			#pragma target 3.5
			#pragma exclude_renderers gles d3d11_9x ps4 ps5 xboxone
			#pragma vertex MetaVert
			#pragma fragment MetaFrag
			#pragma fragmentoption ARB_precision_hint_fastest


			#pragma shader_feature_local __ _MK_ALBEDO_MAP
			#pragma shader_feature_local __ _MK_ALPHA_CLIPPING
			#pragma shader_feature_local __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_BORDER_COLOR _MK_DISSOLVE_BORDER_RAMP
			#pragma shader_feature_local __ _MK_COLOR_GRADING_ALBEDO _MK_COLOR_GRADING_FINAL_OUTPUT

			#define MK_URP
			#define MK_UNLIT
			#define MK_STANDARD
			#include_with_pragmas "../../Lib/DotsInstancingSetup.hlsl"

			#include "../../Lib/Meta/Setup.hlsl"
			ENDHLSL
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// Depth Only
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
        {
            Name "DepthOnly"
            Tags { "LightMode" = "DepthOnly" }

            ZWrite On
            ColorMask R
            Cull [_RenderFace]

            HLSLPROGRAM
            #pragma target 3.5
			#pragma exclude_renderers gles d3d11_9x ps4 ps5 xboxone

            #pragma vertex DepthOnlyVert
            #pragma fragment DepthOnlyFrag

			#pragma shader_feature_local __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_BORDER_COLOR _MK_DISSOLVE_BORDER_RAMP
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_SINE _MK_VERTEX_ANIMATION_PULSE _MK_VERTEX_ANIMATION_NOISE
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_MAP
            #pragma shader_feature_local __ _MK_ALBEDO_MAP
            #pragma shader_feature_local __ _MK_ALPHA_CLIPPING

			#if UNITY_VERSION >= 202220
				#pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
			#endif

            #pragma multi_compile_instancing

			#define MK_URP
			#define MK_UNLIT
			#define MK_STANDARD
			#include_with_pragmas "../../Lib/DotsInstancingSetup.hlsl"

            #include "../../Lib/DepthOnly/Setup.hlsl"
            ENDHLSL
        }

		/////////////////////////////////////////////////////////////////////////////////////////////
		// Depth Normals
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
        {
            Name "DepthNormals"
            Tags { "LightMode" = "DepthNormals" }

            ZWrite On
            Cull [_RenderFace]

            HLSLPROGRAM
            #pragma target 3.5
			#pragma exclude_renderers gles d3d11_9x ps4 ps5 xboxone

            #pragma vertex DepthNormalsVert
            #pragma fragment DepthNormalsFrag

			#pragma shader_feature_local __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_BORDER_COLOR _MK_DISSOLVE_BORDER_RAMP
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_SINE _MK_VERTEX_ANIMATION_PULSE _MK_VERTEX_ANIMATION_NOISE
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_MAP
            #pragma shader_feature_local __ _MK_ALBEDO_MAP
            #pragma shader_feature_local __ _MK_ALPHA_CLIPPING

			#if UNITY_VERSION >= 202220
				#if UNITY_VERSION >= 202310
					#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
				#else
					#pragma multi_compile_fragment _ _WRITE_RENDERING_LAYERS
				#endif
				#pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
			#endif

            #pragma multi_compile_instancing

			#define MK_URP
			#define MK_UNLIT
			#define MK_STANDARD
			#include_with_pragmas "../../Lib/DotsInstancingSetup.hlsl"

            #include "../../Lib/DepthNormals/Setup.hlsl"
            ENDHLSL
        }

		/////////////////////////////////////////////////////////////////////////////////////////////
		// Motion Vectors
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
        {
			PackageRequirements 
			{
				"com.unity.render-pipelines.universal":"[16.0,99.99]"
			}
            Name "MotionVectors"
            Tags { "LightMode" = "MotionVectors" }

            ZWrite On
            Cull [_RenderFace]
			ColorMask RG

            HLSLPROGRAM
            #pragma target 3.5
			#pragma exclude_renderers gles d3d11_9x ps4 ps5 xboxone

            #pragma vertex MotionVectorsVert
            #pragma fragment MotionVectorsFrag
			#pragma shader_feature_local_vertex _ADD_PRECOMPUTED_VELOCITY

			#pragma shader_feature_local __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_BORDER_COLOR _MK_DISSOLVE_BORDER_RAMP
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_SINE _MK_VERTEX_ANIMATION_PULSE _MK_VERTEX_ANIMATION_NOISE
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_MAP
            #pragma shader_feature_local __ _MK_ALBEDO_MAP
            #pragma shader_feature_local __ _MK_ALPHA_CLIPPING

            #pragma multi_compile_instancing

			#if UNITY_VERSION >= 202220
				#pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
			#endif

			#define MK_URP
			#define MK_UNLIT
			#define MK_STANDARD

            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ObjectMotionVectors.hlsl"
            #include "../../Lib/MotionVectors/Setup.hlsl"
            ENDHLSL
        }

		/////////////////////////////////////////////////////////////////////////////////////////////
		// Universal2D
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
        {
            Name "Universal2D"
            Tags{ "LightMode" = "Universal2D" }

            Blend [_BlendSrc] [_BlendDst]
            ZWrite [_ZWrite]
            Cull [_RenderFace]

            HLSLPROGRAM
			#pragma target 3.5
			#pragma exclude_renderers gles d3d11_9x ps4 ps5 xboxone

            #pragma vertex Universal2DVert
            #pragma fragment Universal2DFrag
			
			#pragma shader_feature_local __ _MK_SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local __ _MK_ALBEDO_MAP
			#pragma shader_feature_local __ _MK_ALPHA_CLIPPING
            #pragma shader_feature_local __ _MK_BLEND_PREMULTIPLY _MK_BLEND_ADDITIVE _MK_BLEND_MULTIPLY
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_SINE _MK_VERTEX_ANIMATION_PULSE _MK_VERTEX_ANIMATION_NOISE
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_MAP
			#pragma shader_feature_local __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_BORDER_COLOR _MK_DISSOLVE_BORDER_RAMP
			#pragma shader_feature_local __ _MK_COLOR_GRADING_ALBEDO _MK_COLOR_GRADING_FINAL_OUTPUT

            #define MK_URP
			#define MK_UNLIT
			#define MK_STANDARD
			#include_with_pragmas "../../Lib/DotsInstancingSetup.hlsl"

            #include "../../Lib/Universal2D/Setup.hlsl"

            ENDHLSL
        }
    }

	/////////////////////////////////////////////////////////////////////////////////////////////
	// SM 2.5
	/////////////////////////////////////////////////////////////////////////////////////////////
	SubShader
	{
		Tags { "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

		/////////////////////////////////////////////////////////////////////////////////////////////
		// FORWARD BASE
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			PackageRequirements 
			{
				"com.unity.render-pipelines.universal":"[7.0,11.99]"
			}
			Stencil
			{
				Ref [_StencilRef]
				ReadMask [_StencilReadMask]
				WriteMask [_StencilWriteMask]
				Comp [_StencilComp]
				Pass [_StencilPass]
				Fail [_StencilFail]
				ZFail [_StencilZFail]
			}

			Tags { "LightMode" = "UniversalForward" }
			Name "ForwardBase" 
			Cull [_RenderFace]
			Blend [_BlendSrc] [_BlendDst], [_BlendSrcAlpha] [_BlendDstAlpha]
			ZWrite [_ZWrite]
			ZTest [_ZTest]

			HLSLPROGRAM
			#pragma target 2.5
			#pragma exclude_renderers gles3 d3d11 ps4 ps5 xboxone wiiu n3ds switch
			#pragma shader_feature_local __ _MK_SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local __ _MK_ALPHA_CLIPPING
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_SINE _MK_VERTEX_ANIMATION_PULSE _MK_VERTEX_ANIMATION_NOISE
			#pragma shader_feature_local __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_BORDER_COLOR _MK_DISSOLVE_BORDER_RAMP
			#pragma shader_feature_local __ _MK_ALBEDO_MAP
            #pragma shader_feature_local __ _MK_BLEND_PREMULTIPLY _MK_BLEND_ADDITIVE _MK_BLEND_MULTIPLY
			#pragma shader_feature_local __ _MK_COLOR_GRADING_ALBEDO _MK_COLOR_GRADING_FINAL_OUTPUT
			#pragma shader_feature_local __ _MK_REFRACTION_DISTORTION_MAP
			#pragma shader_feature_local __ _MK_INDEX_OF_REFRACTION

			#if UNITY_VERSION >= 202320
				#include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
			#endif

			#if UNITY_VERSION >= 202220
				#pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
			#endif

			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex ForwardVert
			#pragma fragment ForwardFrag

            #if UNITY_VERSION >= 61000000
				#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Fog.hlsl"
			#else
				#pragma multi_compile_fog
			#endif
			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer

			#define MK_URP
			#define MK_REFRACTION
			#define MK_UNLIT
			#define MK_STANDARD
			#include_with_pragmas "../../Lib/DotsInstancingSetup.hlsl"

			#include "../../Lib/Forward/BaseSetup.hlsl"
			
			ENDHLSL
		}

		Pass
		{
			PackageRequirements 
			{
				"com.unity.render-pipelines.universal":"[12.0,99.99]"
			}
			Stencil
			{
				Ref [_StencilRef]
				ReadMask [_StencilReadMask]
				WriteMask [_StencilWriteMask]
				Comp [_StencilComp]
				Pass [_StencilPass]
				Fail [_StencilFail]
				ZFail [_StencilZFail]
			}

			Tags { "LightMode" = "UniversalForwardOnly" }
			Name "ForwardBase" 
			Cull [_RenderFace]
			Blend [_BlendSrc] [_BlendDst], [_BlendSrcAlpha] [_BlendDstAlpha]
			ZWrite [_ZWrite]
			ZTest [_ZTest]

			HLSLPROGRAM
			#pragma target 2.5
			#pragma exclude_renderers gles3 d3d11 ps4 ps5 xboxone wiiu n3ds switch
			#pragma shader_feature_local __ _MK_SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local __ _MK_ALPHA_CLIPPING
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_SINE _MK_VERTEX_ANIMATION_PULSE _MK_VERTEX_ANIMATION_NOISE
			#pragma shader_feature_local __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_BORDER_COLOR _MK_DISSOLVE_BORDER_RAMP
			#pragma shader_feature_local __ _MK_ALBEDO_MAP
            #pragma shader_feature_local __ _MK_BLEND_PREMULTIPLY _MK_BLEND_ADDITIVE _MK_BLEND_MULTIPLY
			#pragma shader_feature_local __ _MK_COLOR_GRADING_ALBEDO _MK_COLOR_GRADING_FINAL_OUTPUT
			#pragma shader_feature_local __ _MK_REFRACTION_DISTORTION_MAP
			#pragma shader_feature_local __ _MK_INDEX_OF_REFRACTION

			#if UNITY_VERSION >= 202320
				#include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
			#endif
			
			#if UNITY_VERSION >= 202220
				#pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
			#endif

			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex ForwardVert
			#pragma fragment ForwardFrag

            #if UNITY_VERSION >= 61000000
				#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Fog.hlsl"
			#else
				#pragma multi_compile_fog
			#endif
			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer

			#define MK_URP
			#define MK_REFRACTION
			#define MK_UNLIT
			#define MK_STANDARD
			#include_with_pragmas "../../Lib/DotsInstancingSetup.hlsl"

			#include "../../Lib/Forward/BaseSetup.hlsl"
			
			ENDHLSL
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// FORWARD ADD
		/////////////////////////////////////////////////////////////////////////////////////////////

		/////////////////////////////////////////////////////////////////////////////////////////////
		// DEFERRED
		/////////////////////////////////////////////////////////////////////////////////////////////

		/////////////////////////////////////////////////////////////////////////////////////////////
		// SHADOWCASTER
		/////////////////////////////////////////////////////////////////////////////////////////////
		
		/////////////////////////////////////////////////////////////////////////////////////////////
		// META
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			Tags { "LightMode"="Meta" }
			Name "Meta" 

			Cull Off

			HLSLPROGRAM
			#pragma target 2.5
			#pragma exclude_renderers gles3 d3d11 ps4 ps5 xboxone wiiu n3ds switch
			#pragma vertex MetaVert
			#pragma fragment MetaFrag
			#pragma fragmentoption ARB_precision_hint_fastest


			#pragma shader_feature_local __ _MK_ALBEDO_MAP
			#pragma shader_feature_local __ _MK_ALPHA_CLIPPING
			#pragma shader_feature_local __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_BORDER_COLOR _MK_DISSOLVE_BORDER_RAMP
			#pragma shader_feature_local __ _MK_COLOR_GRADING_ALBEDO _MK_COLOR_GRADING_FINAL_OUTPUT

			#define MK_URP
			#define MK_UNLIT
			#define MK_STANDARD
			#include_with_pragmas "../../Lib/DotsInstancingSetup.hlsl"

			#include "../../Lib/Meta/Setup.hlsl"
			ENDHLSL
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// Depth Only
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
        {
            Name "DepthOnly"
            Tags { "LightMode" = "DepthOnly" }

            ZWrite On
            ColorMask R
            Cull [_RenderFace]

            HLSLPROGRAM
            #pragma target 2.5
			#pragma exclude_renderers gles3 d3d11 ps4 ps5 xboxone wiiu n3ds switch

            #pragma vertex DepthOnlyVert
            #pragma fragment DepthOnlyFrag

			#pragma shader_feature_local __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_BORDER_COLOR _MK_DISSOLVE_BORDER_RAMP
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_SINE _MK_VERTEX_ANIMATION_PULSE _MK_VERTEX_ANIMATION_NOISE
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_MAP
            #pragma shader_feature_local __ _MK_ALBEDO_MAP
            #pragma shader_feature_local __ _MK_ALPHA_CLIPPING

			#if UNITY_VERSION >= 202220
				#pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
			#endif

            #pragma multi_compile_instancing

			#define MK_URP
			#define MK_UNLIT
			#define MK_STANDARD
			#include_with_pragmas "../../Lib/DotsInstancingSetup.hlsl"

            #include "../../Lib/DepthOnly/Setup.hlsl"
            ENDHLSL
        }

		/////////////////////////////////////////////////////////////////////////////////////////////
		// Depth Normals
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
        {
            Name "DepthNormals"
            Tags { "LightMode" = "DepthNormals" }

            ZWrite On
            Cull [_RenderFace]

            HLSLPROGRAM
            #pragma target 2.5
			#pragma exclude_renderers gles3 d3d11 ps4 ps5 xboxone wiiu n3ds switch

            #pragma vertex DepthNormalsVert
            #pragma fragment DepthNormalsFrag

			#pragma shader_feature_local __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_BORDER_COLOR _MK_DISSOLVE_BORDER_RAMP
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_SINE _MK_VERTEX_ANIMATION_PULSE _MK_VERTEX_ANIMATION_NOISE
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_MAP
            #pragma shader_feature_local __ _MK_ALBEDO_MAP
            #pragma shader_feature_local __ _MK_ALPHA_CLIPPING

			#if UNITY_VERSION >= 202220
				#pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
			#endif

            #pragma multi_compile_instancing

			#define MK_URP
			#define MK_UNLIT
			#define MK_STANDARD
			#include_with_pragmas "../../Lib/DotsInstancingSetup.hlsl"

            #include "../../Lib/DepthNormals/Setup.hlsl"
            ENDHLSL
        }

		/////////////////////////////////////////////////////////////////////////////////////////////
		// Motion Vectors
		/////////////////////////////////////////////////////////////////////////////////////////////
		//Disabled for Shader Model 2.5

		/////////////////////////////////////////////////////////////////////////////////////////////
		// Universal2D
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
        {
            Name "Universal2D"
            Tags{ "LightMode" = "Universal2D" }

            Blend [_BlendSrc] [_BlendDst]
            ZWrite [_ZWrite]
            Cull [_RenderFace]

            HLSLPROGRAM
            
            
			#pragma target 2.5
			#pragma exclude_renderers gles3 d3d11 ps4 ps5 xboxone wiiu n3ds switch

            #pragma vertex Universal2DVert
            #pragma fragment Universal2DFrag
			
			#pragma shader_feature_local __ _MK_SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local __ _MK_ALBEDO_MAP
			#pragma shader_feature_local __ _MK_ALPHA_CLIPPING
            #pragma shader_feature_local __ _MK_BLEND_PREMULTIPLY _MK_BLEND_ADDITIVE _MK_BLEND_MULTIPLY
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_SINE _MK_VERTEX_ANIMATION_PULSE _MK_VERTEX_ANIMATION_NOISE
			#pragma shader_feature_local __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_BORDER_COLOR _MK_DISSOLVE_BORDER_RAMP
			#pragma shader_feature_local __ _MK_COLOR_GRADING_ALBEDO _MK_COLOR_GRADING_FINAL_OUTPUT

            #define MK_URP
			#define MK_UNLIT
			#define MK_STANDARD
			#include_with_pragmas "../../Lib/DotsInstancingSetup.hlsl"

            #include "../../Lib/Universal2D/Setup.hlsl"

            ENDHLSL
        }
    }
	
	FallBack Off
	CustomEditor "MK.Toon.Editor.URP.StandardUnlitEditor"
}
