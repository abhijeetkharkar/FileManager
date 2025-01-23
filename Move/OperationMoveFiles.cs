namespace FileManager.Move
{
    public class OperationMoveFiles
    {
        public static void MoveAllFiles(string sourceFolderPath, string destinationFolderPath)
        {
            var directory = new DirectoryInfo(sourceFolderPath);
            if (!directory.Exists)
            {
                throw new ArgumentException("Folder does not exist.", nameof(sourceFolderPath));
            }

            var files = directory.GetFiles();
            foreach (var file in files)
            {
                file.MoveTo(destinationFolderPath + "//" + file.Name);
            }
            Console.WriteLine($"All files in {directory.Name} moved to {destinationFolderPath}!");
        }
    }
}
