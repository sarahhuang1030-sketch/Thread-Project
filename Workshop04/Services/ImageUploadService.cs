namespace Workshop04.Services;

/// <summary>
/// Service implementation for handling image upload operations
/// </summary>
public class ImageUploadService : IImageUploadService
{
    private readonly IWebHostEnvironment _environment;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ImageUploadService> _logger;

    // Configuration values
    private readonly long _maxFileSizeInBytes;
    private readonly string[] _allowedExtensions;
    private readonly string _storagePath;

    public ImageUploadService(
        IWebHostEnvironment environment,
        IConfiguration configuration,
        ILogger<ImageUploadService> logger)
    {
        _environment = environment;
        _configuration = configuration;
        _logger = logger;

        // Load configuration with defaults
        _maxFileSizeInBytes = _configuration.GetValue<int>("ImageUpload:ProfilePictures:MaxFileSizeInMB", 5) * 1024 * 1024;
        _allowedExtensions = _configuration.GetSection("ImageUpload:ProfilePictures:AllowedExtensions").Get<string[]>()
            ?? new[] { ".jpg", ".jpeg", ".png", ".gif" };
        _storagePath = _configuration.GetValue<string>("ImageUpload:ProfilePictures:StoragePath", "uploads/profile-pictures");
    }

    public bool IsValidImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            _logger.LogWarning("File is null or empty");
            return false;
        }

        // Check file size
        if (file.Length > _maxFileSizeInBytes)
        {
            _logger.LogWarning("File size {Size} exceeds maximum allowed size {MaxSize}", file.Length, _maxFileSizeInBytes);
            return false;
        }

        // Check file extension
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(extension) || !_allowedExtensions.Contains(extension))
        {
            _logger.LogWarning("File extension {Extension} is not allowed", extension);
            return false;
        }

        // Check MIME type
        var allowedMimeTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
        if (!allowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
        {
            _logger.LogWarning("File MIME type {MimeType} is not allowed", file.ContentType);
            return false;
        }

        return true;
    }

    public async Task<string?> SaveProfilePictureAsync(IFormFile file, string userId)
    {
        try
        {
            // Validate the image
            if (!IsValidImage(file))
            {
                _logger.LogWarning("Invalid image file for user {UserId}", userId);
                return null;
            }

            // Ensure the upload directory exists
            var uploadPath = Path.Combine(_environment.WebRootPath, _storagePath);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
                _logger.LogInformation("Created upload directory: {UploadPath}", uploadPath);
            }

            // Generate a unique filename
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var fileName = $"{userId}_{timestamp}{extension}";
            var filePath = Path.Combine(uploadPath, fileName);

            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return the relative path for storing in database
            var relativePath = $"/{_storagePath}/{fileName}".Replace("\\", "/");
            _logger.LogInformation("Successfully saved profile picture for user {UserId} at {Path}", userId, relativePath);

            return relativePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving profile picture for user {UserId}", userId);
            return null;
        }
    }

    public async Task<bool> DeleteProfilePictureAsync(string? filePath)
    {
        try
        {
            if (string.IsNullOrEmpty(filePath))
            {
                _logger.LogWarning("File path is null or empty");
                return false;
            }

            // Don't delete the default avatar
            if (filePath.Contains("default-avatar", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation("Skipping deletion of default avatar");
                return true;
            }

            // Convert relative path to physical path
            var physicalPath = Path.Combine(_environment.WebRootPath, filePath.TrimStart('/').Replace("/", "\\"));

            if (File.Exists(physicalPath))
            {
                await Task.Run(() => File.Delete(physicalPath));
                _logger.LogInformation("Successfully deleted profile picture at {Path}", physicalPath);
                return true;
            }
            else
            {
                _logger.LogWarning("File not found at {Path}", physicalPath);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting profile picture at {Path}", filePath);
            return false;
        }
    }
}
