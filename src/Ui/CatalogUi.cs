namespace fishing_cs_revived.src.Ui
{
    public class CatalogUi
    {
        public static void ShowCatalog()
        {
            Console.Clear();
            Console.WriteLine("Katalog, jakákoliv klávesa pro pokračování:\n");
            Console.WriteLine("1. Rybářské potřeby");
            Console.WriteLine("2. Oblečení");
            Console.WriteLine("3. Elektronika");
            Console.WriteLine("4. Nábytek");
            Console.WriteLine("5. Knihy");
            Console.WriteLine("6. Hračky");
            Console.WriteLine("7. Sportovní vybavení");
            Console.WriteLine("8. Domácí potřeby");
            Console.WriteLine("9. Zahradní nářadí");
            Console.WriteLine("10. Auto-moto příslušenství");
            Console.WriteLine("\nStiskněte jakoukoliv klávesu pro návrat do hlavního menu...");
            Console.ReadKey(true);
        }
    }
}