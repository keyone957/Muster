using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
//아이돌 페이셜 트래킹 블랜드 쉐이프 값 네트워크 연동
//최초 작성자: 홍원기
//수정자: 
//최종 수정일: 2024-02-12

public class FacialTrackingNetworkController : NetworkBehaviour
{

    public SkinnedMeshRenderer meshRenderer;
    [Networked]
    public float eye_close_L { get; set; }
    [Networked]
    public float eye_close_R { get; set; }
    [Networked]
    public float eye_under_up { get; set; }
    [Networked]
    public float eye_under_up_L { get; set; }
    [Networked]
    public float eye_under_up_R { get; set; }
    [Networked]
    public float eye_under_down_L { get; set; }
    [Networked]
    public float eye_under_down_R { get; set; }
    [Networked]
    public float mouth_grin_L { get; set; }
    [Networked]
    public float mouth_grin_R { get; set; }
    [Networked]
    public float mouth_grin_notooth_L { get; set; }
    [Networked]
    public float mouth_grin_notooth_R { get; set; }
    [Networked]
    public float mouth_grin2_L { get; set; }
    [Networked]
    public float mouth_grin2_R { get; set; }
    [Networked]
    public float mouth_awawa { get; set; }
    [Networked]
    public float mouth_smile_L { get; set; }
    [Networked]
    public float mouth_smile_R { get; set; }
    [Networked]
    public float mouth_smile2 { get; set; }
    [Networked]
    public float mouth_sad_L { get; set; }
    [Networked]
    public float mouth_sad_R { get; set; }
    [Networked]
    public float mouth_wide_L { get; set; }
    [Networked]
    public float mouth_wide_R { get; set; }
    [Networked]
    public float mouth_narrow_L { get; set; }
    [Networked]
    public float mouth_narrow_R { get; set; }

    [Networked]
    public float mouth_tang_up { get; set; }
    [Networked]
    public float mouth_tang_down { get; set; }
    [Networked]
    public float mouth_H_L { get; set; }
    [Networked]
    public float mouth_H_R { get; set; }
    [Networked]
    public float brow_joy { get; set; }
    [Networked]
    public float brow_joy_L { get; set; }
    [Networked]
    public float brow_joy_R { get; set; }
    [Networked]
    public float brow_up_L { get; set; }
    [Networked]
    public float brow_up_R { get; set; }
    [Networked]
    public float brow_down_L { get; set; }
    [Networked]
    public float brow_down_R { get; set; }
    [Networked]
    public float eye_morph_wide { get; set; }
    [Networked]
    public float nose_morph_up { get; set; }
    [Networked]
    public float jaw_morph_sharp{ get; set; }


    //*FacialTracking
    [Networked]
    public float EyeLookOutRight { get; set; }
    [Networked]
    public float EyeLookInRight { get; set; }
    [Networked]
    public float EyeLookOutLeft{ get; set; }
    [Networked]
    public float EyeLookInLeft { get; set; }
    [Networked]
    public float EyeLookUpRight { get; set; }
    [Networked]
    public float EyeLookUpLeft { get; set; }
    [Networked]
    public float EyeClosedRight { get; set; }
    [Networked]
    public float EyeClosedLeft { get; set; }
    [Networked]
    public float EyeSquint { get; set; }
    [Networked]
    public float EyeSquintRight { get; set; }
    [Networked]
    public float EyeSquintLeft { get; set; }
    [Networked]
    public float EyeWideRight { get; set; }
    [Networked]
    public float EyeWideLeft { get; set; }
    [Networked]
    public float BrowInnerUpRight { get; set; }
    [Networked]
    public float BrowInnerUpLeft { get; set; }
    [Networked]
    public float BrowOuterUpRight { get; set; }
    [Networked]
    public float BrowOuterUpLeft { get; set; }
    [Networked]
    public float NoseSneerRight { get; set; }
    [Networked]
    public float CheekPuffRight { get; set; }
    [Networked]
    public float CheekPuffLeft { get; set; }
    [Networked]
    public float JawOpen { get; set; }
    [Networked]
    public float JawRight { get; set; }
    [Networked]
    public float JawLeft { get; set; }
    [Networked]
    public float LipSuckUpper { get; set; }
    [Networked]
    public float LipSuckLower { get; set; }
    [Networked]
    public float LipPuckerUpper { get; set; }
    [Networked]
    public float MouthUpperUpLeft { get; set; }
    [Networked]
    public float MouthUpperUpRight { get; set; }
    [Networked]
    public float MouthUpperUp { get; set; }
    [Networked]
    public float MouthLowerDownRight { get; set; }
    [Networked]
    public float MouthLowerDownLeft { get; set; }
    [Networked]
    public float MouthRight { get; set; }
    [Networked]
    public float MouthLeft { get; set; }
    [Networked]
    public float MouthSmileRight { get; set; }
    [Networked]
    public float MouthSmileLeft { get; set; }
    [Networked]
    public float MouthSmile { get; set; }
    [Networked]
    public float MouthSadRight { get; set; }
    [Networked]
    public float MouthSadLeft { get; set; }
    [Networked]
    public float MouthSad { get; set; }
    [Networked]
    public float MouthRaiser { get; set; }
    [Networked]
    public float TongueJawOpenCorrectionLeft { get; set; }


    void Update()
    {
        if (meshRenderer != null && !HasStateAuthority)
        {
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("eye_close_L"), eye_close_L);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("eye_close_R"), eye_close_R);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("eye_under_up"), eye_under_up);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("eye_under_up_L"), eye_under_up_L);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("eye_under_up_R"), eye_under_up_R);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("eye_under_down_L"), eye_under_down_L);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("eye_under_down_R"), eye_under_down_R);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_grin_L"), mouth_grin_L);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_grin_R"), mouth_grin_R);
            // meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_grin_notooth_L"), mouth_grin_notooth_L);
            //meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_grin_notooth_R"), mouth_grin_notooth_R);
            //meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_grin2_L"), mouth_grin2_L);
            //meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_grin2_R"), mouth_grin2_R);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("EyeLookOutRight"), EyeLookOutRight);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("EyeLookInRight"), EyeLookInRight);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("EyeLookOutLeft"), EyeLookOutLeft);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("EyeLookInLeft"), EyeLookInLeft);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("EyeLookUpRight"), EyeLookUpRight);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("EyeLookUpLeft"), EyeLookUpLeft);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("EyeClosedRight"), EyeClosedRight);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("EyeClosedLeft"), EyeClosedLeft);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("EyeSquint"), EyeSquint);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("EyeSquintRight"), EyeSquintRight);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("EyeSquintLeft"), EyeSquintLeft);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("EyeWideRight"), EyeWideRight);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("EyeWideLeft"), EyeWideLeft);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("BrowInnerUpRight"), BrowInnerUpRight);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("BrowInnerUpLeft"), BrowInnerUpLeft);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("BrowOuterUpRight"), BrowOuterUpRight);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("BrowOuterUpLeft"), BrowOuterUpLeft);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("NoseSneerRight"), NoseSneerRight);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("CheekPuffRight"), CheekPuffRight);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("CheekPuffLeft"), CheekPuffLeft);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("JawOpen"), JawOpen);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("JawRight"), JawRight);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("JawLeft"), JawLeft);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("LipSuckUpper"), LipSuckUpper);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("LipPuckerUpper"), LipPuckerUpper);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("MouthUpperUpLeft"), MouthUpperUpLeft);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("MouthUpperUpRight"), MouthUpperUpRight);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("MouthUpperUp"), MouthUpperUp);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("MouthLowerDownRight"), MouthLowerDownRight);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("MouthLowerDownLeft"), MouthLowerDownLeft);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("MouthRight"), MouthRight);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("MouthLeft"), MouthLeft);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("MouthSmileRight"), MouthSmileRight);
            //meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("MouthSmileLeft "), MouthSmileLeft);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("MouthSmile"), MouthSmile);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("MouthSadRight"), MouthSadRight);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("MouthSadLeft"), MouthSadLeft);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("MouthSad"), MouthSad);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("MouthRaiser"), MouthRaiser);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("TongueJawOpenCorrectionLeft"), TongueJawOpenCorrectionLeft);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_awawa"), mouth_awawa);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_smile_L"), mouth_smile_L);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_smile_R"), mouth_smile_R);
            //meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_smile2"), mouth_smile2);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_sad_L"), mouth_sad_L);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_sad_R"), mouth_sad_R);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_wide_L"), mouth_wide_L);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_wide_R"), mouth_wide_R);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_narrow_L"), mouth_narrow_L);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_narrow_R"), mouth_narrow_R);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_tang_up"), mouth_tang_up);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_tang_down"), mouth_tang_down);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_H_L"), mouth_H_L);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_H_R"), mouth_H_R);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("brow_joy"), brow_joy);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("brow_joy_L"), brow_joy_L);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("brow_joy_R"), brow_joy_R);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("brow_up_L"), brow_up_L);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("brow_up_R"), brow_up_R);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("brow_down_L"), brow_down_L);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("brow_down_R"), brow_down_R);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("eye_morph_wide"), eye_morph_wide);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("nose_morph_up"), nose_morph_up);
            meshRenderer.SetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("jaw_morph_sharp"), jaw_morph_sharp);

        }
    }
    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority)
        {
            eye_close_L = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("eye_close_L"));
            eye_close_R = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("eye_close_R"));
            eye_under_up = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("eye_under_up"));
            eye_close_L = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("eye_close_L"));
            eye_under_up_R = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("eye_under_up_R"));
            eye_under_up_R = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("eye_under_up_R"));
            eye_under_down_L = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("eye_under_down_L"));
            eye_under_down_R = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("eye_under_down_R"));
            mouth_grin_L = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_grin_L"));
            mouth_grin_R = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_grin_R"));
            //mouth_grin_notooth_L = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_grin_notooth_L"));
            //mouth_grin_notooth_R = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_grin_notooth_R"));
            //mouth_grin2_L = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_grin2_L"));
            //mouth_grin2_R = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_grin2_R"));
            EyeLookOutRight = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("EyeLookOutRight"));
            EyeLookInRight = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("EyeLookInRight"));
            EyeLookOutLeft = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("EyeLookOutLeft"));
            EyeLookInLeft = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("EyeLookInLeft"));
            EyeLookUpRight = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("EyeLookUpRight"));
            EyeLookUpLeft = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("EyeLookUpLeft"));
            EyeClosedRight = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("EyeClosedRight"));
            EyeClosedLeft = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("EyeClosedLeft"));
            EyeSquint = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("EyeSquint"));
            EyeSquintRight = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("EyeSquintRight"));
            EyeSquintLeft = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("EyeSquintLeft"));
            EyeWideRight = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("EyeWideRight"));
            EyeWideLeft = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("EyeWideLeft"));
            BrowInnerUpRight = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("BrowInnerUpRight"));
            BrowInnerUpLeft = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("BrowInnerUpLeft"));
            BrowOuterUpRight = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("BrowOuterUpRight"));
            BrowOuterUpLeft = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("BrowOuterUpLeft"));
            NoseSneerRight = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("NoseSneerRight"));
            CheekPuffRight = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("CheekPuffRight"));
            CheekPuffLeft = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("CheekPuffLeft"));
            JawOpen = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("JawOpen"));
            JawRight = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("JawRight"));
            JawLeft = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("JawLeft"));
            LipSuckUpper = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("LipSuckUpper"));
            LipPuckerUpper = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("LipPuckerUpper"));
            MouthUpperUpLeft = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("MouthUpperUpLeft"));
            MouthUpperUpRight = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("MouthUpperUpRight"));
            MouthUpperUp = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("MouthUpperUp"));
            MouthLowerDownRight = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("MouthLowerDownRight"));
            MouthLowerDownLeft = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("MouthLowerDownLeft"));
            MouthRight = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("MouthRight"));
            MouthLeft = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("MouthLeft"));
            MouthSmileRight = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("MouthSmileRight"));
            //MouthSmileLeft = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("MouthSmileLeft"));
            MouthSmile = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("MouthSmile"));
            MouthSadRight = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("MouthSadRight"));
            MouthSadLeft = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("MouthSadLeft"));
            MouthSad = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("MouthSad"));
            MouthRaiser = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("MouthRaiser"));
            TongueJawOpenCorrectionLeft = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("TongueJawOpenCorrectionLeft"));
            mouth_awawa = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_awawa"));
            mouth_smile_L = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_smile_L"));
            mouth_smile_R = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_smile_R"));
            //mouth_smile2 = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_smile2"));
            mouth_sad_L = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_sad_L"));
            mouth_sad_R = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_sad_R"));
            mouth_wide_L = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_wide_L"));
            mouth_wide_R = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_wide_R"));
            mouth_narrow_L = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_narrow_L"));
            mouth_narrow_R = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_narrow_R"));
            mouth_tang_up = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_tang_up"));
            mouth_tang_down = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_tang_down"));
            mouth_H_L = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_H_L"));
            mouth_H_R = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("mouth_H_R"));
            brow_joy = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("brow_joy"));
            brow_joy_L = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("brow_joy_L"));
            brow_joy_R = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("brow_joy_R"));
            brow_up_L = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("brow_up_L"));
            brow_up_R = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("brow_up_R"));
            brow_down_L = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("brow_down_L"));
            brow_down_R = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("brow_down_R"));
            eye_morph_wide = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("eye_morph_wide"));
            nose_morph_up = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("nose_morph_up"));
            jaw_morph_sharp = meshRenderer.GetBlendShapeWeight(meshRenderer.sharedMesh.GetBlendShapeIndex("jaw_morph_sharp"));
        }
    }
}
