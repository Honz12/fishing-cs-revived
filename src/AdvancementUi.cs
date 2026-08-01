static class AdvancementUi
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
        Console.WriteLine($"Advancementy: {Program.data.Advancements.Count}/{AdvancementProcessor.AdvancementCount}\n");

        for (int i = 0; i < Program.data.Advancements.Count; i++)
        {
            Advancement advancement = Program.data.Advancements[i];

            if (Selected == i)
                Program.DisplayImage(
                    new Image("advancements", advancement.IconName),
                    $"{advancement.Name} ({advancement.Description})"
                );
            else
                Console.WriteLine($"- {advancement.Name} ({advancement.Description})");
        }
    }
}
