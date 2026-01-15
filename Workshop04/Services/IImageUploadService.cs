namespace Workshop04.Services;

/// <summary>
/// Service interface for handling image upload operations
/// </summary>
public interface IImageUploadService
{
    /// <summary>
    /// Saves a profile picture for a user
    /// </summary>
    /// <param name="file">The image file to upload</param>
    /// <param name="userId">The user ID to associate the image with</param>
    /// <returns>The relative path to the saved image, or null if save failed</returns>
    Task<string?> SaveProfilePictureAsync(IFormFile file, string userId);

    /// <summary>
    /// Deletes a profile picture from the file system
    /// </summary>
    /// <param name="filePath">The relative path to the file to delete</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    Task<bool> DeleteProfilePictureAsync(string? filePath);

    /// <summary>
    /// Validates if a file is a valid image
    /// </summary>
    /// <param name="file">The file to validate</param>
    /// <returns>True if the file is a valid image, false otherwise</returns>
    bool IsValidImage(IFormFile file);
}
