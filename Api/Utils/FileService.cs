namespace Api.Utils;

public static class FileService
{
    public static IEnumerable<string> GetFiles(params string[] paths)
    {
        var allPaths = new string[paths.Length + 1];
        allPaths[0] = AppDomain.CurrentDomain.BaseDirectory;
        paths.CopyTo(allPaths, 1);
        
        var directoryPath = Path.Combine(paths);

        if(Directory.Exists(directoryPath))
        {
            return Directory.GetFiles(directoryPath);
        }

        return Array.Empty<string>();
    }
    
    public static IEnumerable<string> GetFilesExtension(string extension, params string[] paths)
    {
        return GetFiles(paths).Where(x => Path.GetExtension(x) == $".{extension}");
    }
}