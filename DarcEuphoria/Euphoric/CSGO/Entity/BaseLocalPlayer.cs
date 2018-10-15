using System.Collections.Generic;
using System.Threading;
using DarcEuphoria.Euphoric.Classes;
using DarcEuphoria.Euphoric.Structs;
using DarcEuphoria.Hacks.Injection;

namespace DarcEuphoria.Euphoric.CSGO.Entity
{
    public class BaseLocalPlayer : BasePlayer
    {
        private Devalue<int> _thirdPerson;
        public Devalue<Vector2> AimPunchAngle;
        private float CurrentMaxFlash;
        public Devalue<int> DefaultFOV;
        public Devalue<bool> DrawViewModel;
        public Devalue<int> ExecAttack;
        public Devalue<int> ExecAttack2;
        private Devalue<int> ExecJump;
        public Devalue<int> Flags;
        public Devalue<float> FlashAlpha;
        public Devalue<int> FlashDuration;
        public Devalue<int> FOV;
        public Devalue<int> ModelIndex;
        public Devalue<bool> Scoped;
        public Devalue<int> ShotsFired;

        public BaseLocalPlayer()
        {
            SetFields();

            if (GlobalVariables.ActiveSettings.MiscSettings.RankRevealer)
                RankRevealer.Start();
        }

        public Vector2 ViewAngles
        {
            get => Memory.Read<Vector2>(CSGOEngine.csClient.ClientState.Value + Offsets.dwClientState_ViewAngles);
            set => Memory.Write(CSGOEngine.csClient.ClientState.Value + Offsets.dwClientState_ViewAngles, value);
        }

        public int CrosshairID =>
            Memory.Read<int>(Pointer.Value + Netvars.m_iCrosshairId);

        public Vector2 ViewAndPunch => ViewAngles + AimPunchAngle.Value;

        public Vector3 EyeLevel
        {
            get
            {
                var vec = Position.Value;
                vec.Z += Memory.Read<float>(Pointer.Value + 0x10C);
                return vec;
            }
        }

        public bool ThirdPerson
        {
            get => _thirdPerson.Value == 1;
            set
            {
                if (_thirdPerson.Value != (value ? 1 : 0)) _thirdPerson.Value = value ? 1 : 0;
            }
        }

        public List<BaseWeapon> AllWeapon
        {
            get
            {
                var WeaponList = new List<BaseWeapon>();
                for (var i = 0; i < 16; i++)
                {
                    var weapon = new BaseWeapon().GetWeapon(Pointer.Value, i);
                    if (weapon.WeaponName == "NOWEAPONNAME")
                        continue;

                    WeaponList.Add(weapon);
                }

                return WeaponList;
            }
        }

        public bool IsFlashed()
        {
            if (CurrentMaxFlash <= 0)
                if (FlashDuration.Value > 0)
                    CurrentMaxFlash = FlashDuration.Value;

            if (CurrentMaxFlash > 0)
                CurrentMaxFlash -= CSGOEngine.csClient.GlobalVarsBase.Value.frameTime;

            if (CurrentMaxFlash < 0) CurrentMaxFlash = 0;

            if (FlashDuration.Value <= 1)
                CurrentMaxFlash = 0;

            if (CurrentMaxFlash <= 2)
                return false;
            return true;
        }

        public bool OnGround()
        {
            if (Flags.Value == 257) return true;
            if (Flags.Value == 263) return true;

            return false;
        }

        protected override void SetFields()
        {
            base.SetFields();
            _thirdPerson = new Devalue<int>(Pointer.Value + Netvars.m_iObserverMode);
            Flags = new Devalue<int>(Pointer.Value + Netvars.m_fFlags);
            ShotsFired = new Devalue<int>(Pointer.Value + Netvars.m_iShotsFired);
            FlashAlpha = new Devalue<float>(Pointer.Value + Netvars.m_flFlashMaxAlpha);
            FlashDuration = new Devalue<int>(Pointer.Value + Netvars.m_flFlashDuration);
            Scoped = new Devalue<bool>(Pointer.Value + Netvars.m_bIsScoped, true);
            DrawViewModel = new Devalue<bool>(Pointer.Value + Netvars.m_bDrawViewmodel, true);
            AimPunchAngle = new Devalue<Vector2>(Pointer.Value + Netvars.m_aimPunchAngle);
            ExecJump = new Devalue<int>(Memory.Client.Base + Offsets.dwForceJump);
            ExecAttack = new Devalue<int>(Memory.Client.Base + Offsets.dwForceAttack);
            ExecAttack2 = new Devalue<int>(Memory.Client.Base + Offsets.dwForceAttack2);
            FOV = new Devalue<int>(Pointer.Value + Netvars.m_iFOVStart - 0x4, true);
            DefaultFOV = new Devalue<int>(Pointer.Value + Netvars.m_iDefaultFOV, true);
            ModelIndex = new Devalue<int>(Pointer.Value + Offsets.m_nModelIndex, true);
        }

        public void Jump()
        {
            ExecJump.Value = 5;
            Thread.Sleep(15);
            ExecJump.Value = 4;
        }

        public void Attack()
        {
            if (!ActiveWeapon.AbleToFire) return;

            ExecAttack.Value = 5;
            Thread.Sleep(10);
            ExecAttack.Value = 4;
        }

        public void Attack2()
        {
            if (!ActiveWeapon.AbleToFire) return;

            ExecAttack2.Value = 5;
            Thread.Sleep(10);
            ExecAttack2.Value = 4;
        }
    }
}