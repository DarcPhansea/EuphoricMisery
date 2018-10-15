using static DarcEuphoria.Hacks.Injection.ClientCmd;

namespace DarcEuphoria.Hacks.Injection
{
    public static class ChatSpam
    {
        private static int Index;

        public static void Start()
        {
            switch (Index)
            {
                case 0:
                    Exec("say Mad Cause Bad.");
                    break;
                case 200:
                    Exec("say Git Gud. Git EuphoricMisery");
                    break;
                case 400:
                    Exec("say EuphoricMisery - 2 Steps Behind The Game.");
                    break;
                case 800:
                    Exec("say 1 Step Forward, 2 Steps Back");
                    break;
                case 1000:
                    Exec("say I would KMS but you already killed me.");
                    break;
                case 1200:
                    Exec("say Not saying I'm good, just saying you're just that bad.");
                    break;
                case 1400:
                    Index = 0;
                    break;
            }

            Index++;
        }
    }
}