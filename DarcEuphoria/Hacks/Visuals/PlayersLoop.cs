using DarcEuphoria.Euphoric;
using DarcEuphoria.Euphoric.Configs.Enums;
using DarcEuphoria.Euphoric.Configs.Structs;
using DarcEuphoria.Euphoric.Enums;
using DarcEuphoria.Euphoric.Structs;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace DarcEuphoria.Hacks.Visuals
{
    public class PlayersLoop
    {
        private static VisualsPlayer PlayerSettings;

        public static void Start(RenderTarget Device, VisualSettings visSettings, Matrix4x4 matrix4X4)
        {
            foreach (var player in GlobalVariables.PlayerList)
            {
                var VISIBLE = player.IsVisible;
                if (player.IsDormant.Value) continue;

                if (player.IsLocalPlayer())
                    PlayerSettings = visSettings.Yourself;
                else if (player.IsSameTeam())
                    PlayerSettings = visSettings.Team;
                else
                    PlayerSettings = visSettings.Enemy;

                if (!PlayerSettings.Enabled) continue;

                if (player.LifeState != LifeState.Alive ||
                    player.Health.Value <= 0) continue;

                if (PlayerSettings.VisableOnly && !VISIBLE)
                    continue;

                if (player.IsLocalPlayer() && !GlobalVariables.ActiveSettings.MiscSettings.ThirdPerson)
                    continue;

                var screenPosition = player.Position.Value.ToScreen(matrix4X4);
                if (screenPosition.X == -1f && screenPosition.Y == -1f) continue;

                if (!player.IsLocalPlayer())
                    if (PlayerSettings.Snaplines)
                        SnapLines.Start(Device, screenPosition,
                            VISIBLE ? PlayerSettings.vSnapLinesColor : PlayerSettings.hSnapLinesColor);

                var headPosition = player.BonePosition(8).ToScreen(matrix4X4);
                if (headPosition.X == -1f && headPosition.Y == -1f) continue;

                if (PlayerSettings.HeadSpot)
                {
                    var ellipse = new Ellipse(headPosition.ToRawVector2(), 4, 4);

                    using (var brush = new SolidColorBrush(Device,
                        VISIBLE ? PlayerSettings.vHeadSpotColor : PlayerSettings.hHeadSpotColor))
                    {
                        Device.FillEllipse(ellipse, brush);
                    }
                }

                DrawArea drawArea;
                drawArea.Height = screenPosition.Y - headPosition.Y;
                drawArea.Width = drawArea.Height / 2f;
                drawArea.Left = (screenPosition.X + headPosition.X) / 2f - drawArea.Width / 2f;
                drawArea.Top = headPosition.Y - drawArea.Height / 10f;
                drawArea.Height *= 1.1f;

                if (!drawArea.IsVisible()) continue;

                if (PlayerSettings.BoxMode == BoxMode.TwoD)
                {
                    if (PlayerSettings.BoxOutline)
                        Box2D.Start(Device, drawArea, new RawColor4(1 / 255f, 1 / 255f, 1 / 255f, 1), 3);

                    Box2D.Start(Device, drawArea,
                        VISIBLE ? PlayerSettings.vBoxColor : PlayerSettings.hBoxColor);
                }
                else if (PlayerSettings.BoxMode == BoxMode.Edges)
                {
                    if (PlayerSettings.BoxOutline)
                        BoxCorners.Start(Device, drawArea, new RawColor4(1 / 255f, 1 / 255f, 1 / 255f, 1), 3);

                    BoxCorners.Start(Device, drawArea,
                        VISIBLE ? PlayerSettings.vBoxColor : PlayerSettings.hBoxColor);
                }
                else if (PlayerSettings.BoxMode == BoxMode.ThreeD)
                {
                    if (PlayerSettings.BoxOutline)
                        Box3D.Start(Device, player, matrix4X4, new RawColor4(1 / 255f, 1 / 255f, 1 / 255f, 1), 3);

                    Box3D.Start(Device, player, matrix4X4,
                        VISIBLE ? PlayerSettings.vBoxColor : PlayerSettings.hBoxColor);
                }

                if (PlayerSettings.HealthMode == HealthMode.Bar ||
                    PlayerSettings.HealthMode == HealthMode.Both)
                    Health.Start(Device, drawArea, player.Health.Value);

                if (PlayerSettings.HealthMode == HealthMode.Number ||
                    PlayerSettings.HealthMode == HealthMode.Both)
                    Health.NumberStart(Device, drawArea, player.Health.Value);

                if (PlayerSettings.Name)
                    Name.Start(Device, drawArea, player.Name,
                        VISIBLE ? PlayerSettings.vTextColor : PlayerSettings.hTextColor,
                        PlayerSettings.Rank);

                if (PlayerSettings.Weapon)
                    ActiveWeapon.Start(Device, drawArea, player.ActiveWeapon.WeaponName,
                        VISIBLE ? PlayerSettings.vTextColor : PlayerSettings.hTextColor);

                if (PlayerSettings.Rank)
                    Rank.Start(Device, drawArea, player.Rank,
                        VISIBLE ? PlayerSettings.vTextColor : PlayerSettings.hTextColor);

                if (PlayerSettings.GlowMode != GlowMode.Off)
                    Glow.Start(player, PlayerSettings.GlowMode,
                        VISIBLE ? PlayerSettings.vGlowColor : PlayerSettings.hGlowColor);
            }
        }
    }
}