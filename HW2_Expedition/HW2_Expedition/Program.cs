namespace HW2_Expedition
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            Game gameplay = new Game();
            //Loads the town order before allowing for any interaction
            gameplay.LoadTownOrder();
            gameplay.GameLoop();
        }
    }
}
