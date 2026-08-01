namespace fishing_cs_revived.src.Ui
{
    public static class AdvancementUi
    {
        public static int Selected = 0;

        /// <summary>
        /// Called when the user presses <code>ConsoleKey.DownArrow</code>
        /// </summary>
        public static void UiButtonMenuDown()
        {
            Selected++;
            Selected += Program.data.Advancements.Count;
            Selected %= Program.data.Advancements.Count;
        }

        /// <summary>
        /// Called when the user presses <code>ConsoleKey.UpArrow</code>
        /// </summary>
        public static void UiButtonMenuUp()
        {
            Selected--;
            Selected += Program.data.Advancements.Count;
            Selected %= Program.data.Advancements.Count;
        }

        public static void Display()
        {
            string title = (Program.TITLE_COLOR +
                @"   ___     _              _        
    / _ \ __| |____ _  __ _| |___  _ 
    | (_) / _` |_ / ' \/ _` | / / || |
    \___/\__,_/__|_||_\__,_|_\_\\_, |
                                |__/ " + "\x1b[0m"
            );

            Console.WriteLine(title);

            Console.WriteLine($"{Program.data.Advancements.Count}/{AdvancementProcessor.AdvancementCount}\n");

            for (int i = 0; i < Program.data.Advancements.Count; i++)
            {
                Advancement advancement = Program.data.Advancements[i];

                if (Selected == i)
                {
                    Console.WriteLine();
                    Program.DisplayImage(
                        new Image("advancements", advancement.IconName),
                        $"{advancement.Name} ({advancement.Description})"
                    );
                    Console.WriteLine();
                }
                else
                    Console.WriteLine($"- {advancement.Name} ({advancement.Description})");
            }
        }
    }
}
