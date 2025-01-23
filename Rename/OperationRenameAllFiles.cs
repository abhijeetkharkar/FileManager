namespace FileManager.Rename
{
    public class OperationRenameAllFiles
    {
        public static void RenameAllFilesWithPrefix(string directoryPath, string? prefix = null)
        {
            var directory = new DirectoryInfo(directoryPath);
            if (!directory.Exists)
            {
                throw new ArgumentException("Folder does not exist.", nameof(directoryPath));
            }
            var directoryName = prefix ?? directory.Name.Split("__")[0].Replace(" ", "_");

            var files = directory.GetFiles();
            foreach (var file in files)
            {
                file.MoveTo(directoryPath + "\\" + directoryName + "_" + file.Name);
            }
            Console.WriteLine($"All files in {directoryName} renamed!");
        }
    }
}
