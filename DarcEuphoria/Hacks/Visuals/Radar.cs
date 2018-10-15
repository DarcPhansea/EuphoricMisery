using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using DarcEuphoria.Euphoric;
using DarcEuphoria.Euphoric.CSGO;

namespace DarcEuphoria.Hacks.Visuals
{
    public static class Radar
    {
        private static Task ThreadTask = null;
        public static Point Center;

        public static void Start(PaintEventArgs e)
        {
            foreach (var player in GlobalVariables.PlayerList)
            {
                var dist = Maths.LocationToPlayer(
                    CSGOEngine.LocalPlayer.Position.Value,
                    player.Position.Value);

                dist.X += Center.X;
                dist.Y += Center.Y;

                if (player.IsSameTeam())
                {
                }
            }
        }
    }
}