// See https://aka.ms/new-console-template for more information
using FileManager.Move;
using FileManager.Rename;
using FileManager.Transform;
using FileManager.Transform.Models;

var photoCategory = PhotoCategory.FEATURED;
var rootDirectoryPath = $@"C:\Users\abhij\Downloads\SiddhuImages\{OperationGetJson.GetPathByPhotoCategory(photoCategory)}";
Console.WriteLine(rootDirectoryPath);
var directories = Directory.GetDirectories(rootDirectoryPath);

// Photos
/*foreach (var directory in directories)
{
    OperationRenameAllFiles.RenameAllFilesWithPrefix(directory);
    OperationMoveFiles.MoveAllFiles(directory, rootDirectoryPath);
}
OperationGetJson.GeneratePhotoMetadataFromFiles(rootDirectoryPath, PhotoCategory.HOMES_AND_SPACES, "images1", false);*/

// PhotoCollections
/*foreach (var directory in directories)
{
    OperationRenameAllFiles.RenameAllFilesWithPrefix(directory);
}*/
OperationGetJson.GeneratePhotoCollectionMetadataFromFiles(rootDirectoryPath, photoCategory, "images");
