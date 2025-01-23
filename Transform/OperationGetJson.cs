using FileManager.Transform.Models;
using System.Text.Json;
using System.Drawing;
using System.Globalization;

namespace FileManager.Transform
{
    public class OperationGetJson
    {
        public static string GetPathByPhotoCategory(PhotoCategory photoCategory)
        {
            switch (photoCategory)
            {
                case PhotoCategory.HOMES_AND_SPACES:
                    return "homesAndSpaces";
                case PhotoCategory.MEGA_PROJECTS:
                    return "megaProjects";
                case PhotoCategory.FEATURED:
                    return "featured";
                case PhotoCategory.HOME_DECOR:
                    return "homeDecor";
                default:
                    return "homesAndSpaces";
            }
        }

        public static void GeneratePhotoMetadataFromFiles(string directoryPath, PhotoCategory category, string s3MainFolder = "images", bool isPrimaryKeyGuid = false, int startIndex = 0)
        {
            var subFolder = GetPathByPhotoCategory(category);
            var directory = new DirectoryInfo(directoryPath);
            if (!directory.Exists)
            {
                throw new ArgumentException("Folder does not exist.", nameof(directoryPath));
            }

            var files = directory.GetFiles();
            var photos = new List<Photo>();
            int i = startIndex;
            foreach (var file in files)
            {
                var id = Guid.NewGuid();
                var img = Image.FromFile(file.FullName);
                photos.Add(new Photo
                {
                    AltText = file.Name,
                    Description = "",
                    Title = "",
                    Url = $"https://s3.ap-south-1.amazonaws.com/siddheshsavantphotography.com/{s3MainFolder}/{subFolder}/" + file.Name,
                    Width = img.Width,
                    Height = img.Height,
                    Id = isPrimaryKeyGuid ? id.ToString() : i + "",
                });
                i++;
            }

            File.WriteAllText(directoryPath + "\\photos.json", JsonSerializer.Serialize(photos, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
            Console.WriteLine($"Generated JSON successfully!");
        }

        public static void GeneratePhotoCollectionMetadataFromFiles(string rootDirectoryPath, PhotoCategory category, string s3MainFolder = "images")
        {
            var subFolder = category == PhotoCategory.HOMES_AND_SPACES ? "homesAndSpaces" : (category == PhotoCategory.MEGA_PROJECTS ? "megaProjects" : "featured");
            var rootDirectory = new DirectoryInfo(rootDirectoryPath);
            if (!rootDirectory.Exists)
            {
                throw new ArgumentException("Folder does not exist.", nameof(rootDirectoryPath));
            }

            var directories = rootDirectory.GetDirectories();
            var photoCollections = new List<PhotoCollection>();
            int i = 0;
            foreach (var directory in directories)
            {
                if (!directory.Exists)
                {
                    continue;
                }
                var files = directory.GetFiles();
                var photos = new List<Photo>();
                int j = 0;
                foreach (var file in files)
                {
                    var id = Guid.NewGuid();
                    var img = Image.FromFile(file.FullName);
                    photos.Add(new Photo
                    {
                        AltText = file.Name,
                        Description = "",
                        Title = "",
                        Url = $"https://s3.ap-south-1.amazonaws.com/siddheshsavantphotography.com/{s3MainFolder}/{subFolder}/{directory.Name}/" + file.Name,
                        Width = img.Width,
                        Height = img.Height,
                        Id = j + "",
                    });
                    j++;
                }

                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                var titleDescription = directory.Name.Split("__");
                Console.WriteLine($"DirectoryName: {textInfo.ToTitleCase(directory.Name.ToLower())}");
                var thumbnail = files.First(file => file.Name.Contains("Cover", StringComparison.OrdinalIgnoreCase));
                var thumbnailImg = Image.FromFile(thumbnail.FullName);

                photoCollections.Add(new PhotoCollection
                {
                    CollectionId = i + "",
                    Title = textInfo.ToTitleCase(titleDescription[0].ToLower()),
                    Description = textInfo.ToTitleCase(titleDescription.Length == 2 ? titleDescription[1].ToLower() : ""),
                    ThumbnailUrl = $"https://s3.ap-south-1.amazonaws.com/siddheshsavantphotography.com/{s3MainFolder}/{subFolder}/{directory.Name}/" + thumbnail?.Name,
                    ThumbnailAltText = thumbnail?.Name,
                    Width = thumbnailImg.Width,
                    Height = thumbnailImg.Height,
                    Eager = false,
                    Photos = photos,
                    Next = i < directories.Length - 1 
                            ? new Collection { CollectionId = (i + 1) + "", Title = textInfo.ToTitleCase(directories[i + 1].Name.Split("__")[0].ToLower()) } 
                            : new Collection { CollectionId = "0", Title = textInfo.ToTitleCase(directories[0].Name.Split("__")[0].ToLower()) },
                    Previous = i > 0 
                            ? new Collection { CollectionId = (i - 1) + "", Title = textInfo.ToTitleCase(directories[i - 1].Name.Split("__")[0].ToLower()) } 
                            : new Collection { CollectionId = (directories.Length - 1) + "", Title = textInfo.ToTitleCase(directories[directories.Length - 1].Name.Split("__")[0].ToLower()) }
                });
                i++;
            }

            File.WriteAllText(rootDirectoryPath + "\\photoCollections.json", JsonSerializer.Serialize(photoCollections, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
            Console.WriteLine($"Generated JSON successfully!");
        }
    }
}
