namespace fishing_cs_revived.src
{
    public class Image
    {
        static byte ConvertColorHexToByte(char c) => char.ToLower(c) switch
        {
            '0' => 0,
            '1' => 1,
            '2' => 2,
            '3' => 3,
            '4' => 4,
            '5' => 5,
            '6' => 6,
            '7' => 7,
            '8' => 8,
            '9' => 9,
            'a' => 10,
            'b' => 11,
            'c' => 12,
            'd' => 13,
            'e' => 14,
            'f' => 15,
            _   => throw new ArgumentException($"Invalid hex character: '{c}'", nameof(c))
        };

        public byte[,] colors;

        public Image(string type, string name) // The constructor
        {
            string contents;
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "src", "assets", "images", type, name);
                contents = File.ReadAllText(path);
            }
            catch
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "src", "assets", "images", "fish", "uhorRicniEletricky.img");
                contents = File.ReadAllText(path);
            }

            colors = new byte[16, 16];

                int x = 0;
                int y = 0;

                foreach (char c in contents)
                {
                    if (c == '\n')
                    {
                        y++;
                        x = 0;
                        continue;
                    }
                    if (c == '\r')
                    {
                        continue;
                    }
                    colors[x, y] = ConvertColorHexToByte(c);
                    x++;
                }
        }
    }
}
