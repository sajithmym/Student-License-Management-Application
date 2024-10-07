using Microsoft.AspNetCore.Mvc;

public class FileResponse
{
    public byte[]? FileBytes { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileExtension { get; set; } = string.Empty;
}


public class CustomFileResponse
{
    public FileContentResult? File { get; set; }
    public string FileExtension { get; set; } = string.Empty;
}