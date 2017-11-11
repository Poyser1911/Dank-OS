namespace Dank_OS
{
    public static class Applications
    {
        public static Application TicTacToeApp => new Application()
        {
            AppName = "Tic Tac Toe",
            AppWindowLogic = new TicTacToeApp(),
            AppIcon = "tictactoe.png",
            AppMemorySize = 100.00f,
            AppStorageSize = 50.00f,
            IsSystemApp = false,
        };
        public static Application MemoryManagerApp { get; set; } = new Application()
        {
            AppName = "Memory Manager",
            AppWindowLogic = new MemoryManagerApp(),
            AppIcon = "memory.png",
            AppMemorySize = 245.00f,
            AppStorageSize = 110.00f,
            IsSystemApp = true,
        };
        public static Application ProcessManagerApp { get; set; } = new Application()
        {
            AppName = "Process Manager",
            AppWindowLogic = new ProcessManangerApp(),
            AppIcon = "process.png",
            AppMemorySize = 150.00f,
            AppStorageSize = 200.00f,
            IsSystemApp = true,
        };
        public static Application FileManagerApp { get; set; } = new Application()
        {
            AppName = "File Manager",
            AppWindowLogic = new FileManagerApp(),
            AppIcon = "disk.png",
            AppMemorySize = 200.00f,
            AppStorageSize = 150.00f,
            IsSystemApp = true,
        }; 
    }
}
