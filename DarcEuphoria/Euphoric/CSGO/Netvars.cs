using DarcEuphoria.Euphoric.ProcessScanner;

namespace DarcEuphoria.Euphoric.CSGO
{
    internal static class Netvars
    {
        public static int m_ArmorValue;
        public static int m_OriginalOwnerXuidHigh;
        public static int m_OriginalOwnerXuidLow;
        public static int m_aimPunchAngle;
        public static int m_aimPunchAngleVel;
        public static int m_bHasDefuser;
        public static int m_bHasHelmet;
        public static int m_bIsDefusing;
        public static int m_bIsScoped;
        public static int m_bSpotted;
        public static int m_bSpottedByMask;
        public static int m_dwBoneMatrix;
        public static int m_fFlags;
        public static int m_flFallbackWear;
        public static int m_flFlashMaxAlpha;
        public static int m_flFlashDuration;
        public static int m_flNextPrimaryAttack;
        public static int m_hActiveWeapon;
        public static int m_hMyWeapons;
        public static int m_hObserverTarget;
        public static int m_hOwner;
        public static int m_hOwnerEntity;
        public static int m_iAccountID;
        public static int m_iClip1;
        public static int m_iCompetitiveRanking;
        public static int m_iCompetitiveWins;
        public static int m_iCrosshairId;
        public static int m_iEntityQuality;
        public static int m_iGlowIndex;
        public static int m_iHealth;
        public static int m_iItemDefinitionIndex;
        public static int m_iItemIDHigh;
        public static int m_iItemIDLow;
        public static int m_iObserverMode;
        public static int m_iShotsFired;
        public static int m_iState;
        public static int m_iTeamNum;
        public static int m_lifeState;
        public static int m_nFallbackPaintKit;
        public static int m_nFallbackSeed;
        public static int m_nFallbackStatTrak;
        public static int m_nForceBone;
        public static int m_nTickBase;
        public static int m_szCustomName;
        public static int m_szLastPlaceName;
        public static int m_vecOrigin;
        public static int m_vecVelocity;
        public static int m_vecViewOffset;
        public static int m_viewPunchAngle;
        public static int m_thirdPersonViewAngles;
        public static int m_clrRender;
        public static int m_zoomLevel;
        public static int m_bDrawViewmodel;
        public static int m_iFOVStart;
        public static int m_iDefaultFOV;
        public static int m_flC4Blow;
        public static int m_fAccuracyPenalty;

        public static void Init()
        {
            m_aimPunchAngle = NetvarManager.Table["DT_BasePlayer"]["m_aimPunchAngle"];
            m_bIsScoped = NetvarManager.Table["DT_CSPlayer"]["m_bIsScoped"];
            m_bSpotted = NetvarManager.Table["DT_BaseEntity"]["m_bSpotted"];
            m_bSpottedByMask = NetvarManager.Table["DT_BaseEntity"]["m_bSpottedByMask"];
            m_dwBoneMatrix = NetvarManager.Table["DT_BaseAnimating"]["m_nForceBone"] + 28;
            m_fFlags = NetvarManager.Table["DT_CSPlayer"]["m_fFlags"];
            m_flFlashMaxAlpha = NetvarManager.Table["DT_CSPlayer"]["m_flFlashMaxAlpha"];
            m_flFlashDuration = NetvarManager.Table["DT_CSPlayer"]["m_flFlashDuration"];
            m_hObserverTarget = NetvarManager.Table["DT_BasePlayer"]["m_hObserverTarget"];
            m_iCrosshairId = NetvarManager.Table["DT_CSPlayer"]["m_bHasDefuser"] + 92;
            m_iGlowIndex = NetvarManager.Table["DT_CSPlayer"]["m_flFlashDuration"] + 24;
            m_iHealth = NetvarManager.Table["DT_BasePlayer"]["m_iHealth"];
            m_iObserverMode = NetvarManager.Table["DT_BasePlayer"]["m_iObserverMode"];
            m_iShotsFired = NetvarManager.Table["DT_CSPlayer"]["m_iShotsFired"];
            m_iTeamNum = NetvarManager.Table["DT_BasePlayer"]["m_iTeamNum"];
            m_lifeState = NetvarManager.Table["DT_CSPlayer"]["m_lifeState"];
            m_vecOrigin = NetvarManager.Table["DT_BasePlayer"]["m_vecOrigin"];
            m_vecVelocity = NetvarManager.Table["DT_CSPlayer"]["m_vecVelocity[0]"];
            m_clrRender = NetvarManager.Table["DT_BaseEntity"]["m_clrRender"];
            m_bDrawViewmodel = NetvarManager.Table["DT_BasePlayer"]["m_bDrawViewmodel"];
            m_iItemDefinitionIndex = NetvarManager.Table["DT_BaseCombatWeapon"]["m_iItemDefinitionIndex"];
            m_iCompetitiveRanking = NetvarManager.Table["DT_CSPlayerResource"]["m_iCompetitiveRanking"];
            m_iFOVStart = NetvarManager.Table["DT_CSPlayer"]["m_iFOVStart"];
            m_iDefaultFOV = NetvarManager.Table["DT_CSPlayer"]["m_iDefaultFOV"];
            m_zoomLevel = NetvarManager.Table["DT_WeaponCSBaseGun"]["m_zoomLevel"];
            m_flC4Blow = NetvarManager.Table["DT_PlantedC4"]["m_flC4Blow"];
            m_hActiveWeapon = NetvarManager.Table["DT_BasePlayer"]["m_hActiveWeapon"];
            m_hMyWeapons = NetvarManager.Table["DT_BasePlayer"]["m_hActiveWeapon"] - 256;

            m_nFallbackPaintKit = NetvarManager.Table["DT_BaseAttributableItem"]["m_nFallbackPaintKit"];
            m_nFallbackSeed = NetvarManager.Table["DT_BaseAttributableItem"]["m_nFallbackSeed"];
            m_flFallbackWear = NetvarManager.Table["DT_BaseAttributableItem"]["m_flFallbackWear"];
            m_nFallbackStatTrak = NetvarManager.Table["DT_BaseAttributableItem"]["m_nFallbackStatTrak"];
            m_szCustomName = NetvarManager.Table["DT_BaseAttributableItem"]["m_szCustomName"];
            m_iAccountID = NetvarManager.Table["DT_BaseAttributableItem"]["m_iAccountID"];
            m_iItemIDLow = NetvarManager.Table["DT_BaseAttributableItem"]["m_iItemIDLow"];
            m_OriginalOwnerXuidLow = NetvarManager.Table["DT_BaseAttributableItem"]["m_OriginalOwnerXuidLow"];
        }
    }
}