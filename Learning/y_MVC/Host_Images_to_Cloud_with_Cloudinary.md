# Host Images to Cloud with Cloudinary

## 1. Set up a Cloudinary Account

1. Go to [Cloudinary](https://cloudinary.com/) and create an account.
2. After signing in, go to the **Dashboard**.
3. In the **Product Environment** section, click on **Go to API Keys** to find:
   - **Cloud Name**
   - **API Key**
   - **API Secret**

These credentials will be necessary for configuring Cloudinary in your application.

## 2. Install Cloudinary .NET SDK

To integrate Cloudinary with your ASP.NET Core project, install the `CloudinaryDotNet` package from NuGet:

1. In Visual Studio, go to **Tools** > **NuGet Package Manager** > **Manage NuGet Packages for Solution**.
2. In the search bar, type **CloudinaryDotNet** and install the package.

## 3. Configure Cloudinary Settings in appsettings.json

To allow access to Cloudinary’s API, add configuration values for it in your `appsettings.json` file.

### Example `appsettings.json`

```json
{
  "CloudinarySettings": {
    "Cloud": "YourCloudName",
    "ApiKey": "YourApiKey",
    "ApiSecret": "YourApiSecret"
  }
}
```

Replace `"YourCloudName"`, `"YourApiKey"`, and `"YourApiSecret"` with the values from your Cloudinary dashboard.

### Explanation

- **Purpose:** This section allows us to keep Cloudinary credentials secure and centralized within configuration files.
- **Format:** Each property in `"CloudinarySettings"` matches the settings we need to configure Cloudinary (`Cloud`, `ApiKey`, and `ApiSecret`).
  
## 4. Create Cloudinary Settings Class

To map and access these settings in your application, create a class that represents the Cloudinary settings.

```csharp
public class CloudinarySettings
{
    public string Cloud { get; set; } = null!;
    public string ApiKey { get; set; } = null!;
    public string ApiSecret { get; set; } = null!;
}
```
> **Note:** Property names should match those in `appsettings.json`.

### Explanation

- **Purpose:** The `CloudinarySettings` class maps the values in `appsettings.json` to properties that can be used within the application.
- **Property Names:** The property names must match those in `appsettings.json` so that they can be properly bound.

## 5. Register Cloudinary Settings in Program.cs

To make the `CloudinarySettings` available throughout your application, register them in `Program.cs`.

```csharp
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection(nameof(CloudinarySettings)));
```

### Explanation

- **Service Registration:** This line tells ASP.NET Core to read the `CloudinarySettings` section from `appsettings.json` and make it available via dependency injection.
- **`nameof(CloudinarySettings)`:** Ensures that the configuration section name is consistent with the class name.

## 6. Configure Cloudinary in the Controller

In your controller, set up a `Cloudinary` instance. Start by adding a private field for the `Cloudinary` instance and then initialize it in the constructor using `IOptions<CloudinarySettings>`.

### Example in `BooksController`

```csharp
public class BooksController : Controller
{
    private readonly Cloudinary _cloudinary;

    public BooksController(IOptions<CloudinarySettings> cloudinaryOptions)
    {
        var account = new Account(
            cloudinaryOptions.Value.Cloud,
            cloudinaryOptions.Value.ApiKey,
            cloudinaryOptions.Value.ApiSecret
        );
        _cloudinary = new Cloudinary(account);
    }
}
```

### Explanation

- **Private Field `_cloudinary`:** A private field that holds the `Cloudinary` instance.
- **Constructor Injection:** The constructor takes `IOptions<CloudinarySettings>`, which provides access to Cloudinary settings.
- **Account Setup:** Using the values from `IOptions<CloudinarySettings>`, an `Account` object is created and passed to the `Cloudinary` instance.

## 7. Upload an Image to Cloudinary with Transformations

The following code example shows how to upload an image to Cloudinary with basic validation, and it applies transformations like width, height, crop, and gravity.

```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(BookFormViewModel model)
{
    if (!ModelState.IsValid)
    {
        model = PopulateViewModel(model);
        return View("Form", model);
    }

    if (model.Image != null)
    {
        // Validate file type and size
        var imageExtension = Path.GetExtension(model.Image.FileName);
        if (!AllowedImageExtensions.Contains(imageExtension) || model.Image.Length > MaxImageSize)
        {
            ModelState.AddModelError("Image", "Invalid image format or size.");
            model = PopulateViewModel(model);
            return View("Form", model);
        }

        // Upload image to Cloudinary with transformations
        using var stream = model.Image.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(model.Image.FileName, stream),
            PublicId = Guid.NewGuid().ToString(),
            Transformation = new Transformation().Width(300).Height(300).Crop("fill").Gravity("face") // optinal
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.Error != null)
        {
            ModelState.AddModelError("Image", uploadResult.Error.Message);
            model = PopulateViewModel(model);
            return View("Form", model);
        }

        model.ImageUrl = uploadResult.SecureUrl.ToString();
    }

    var book = _mapper.Map<Book>(model);
    _context.Books.Add(book);
    await _context.SaveChangesAsync();

    return RedirectToAction(nameof(Index));
}
```
- **Notes:**
  - This example also includes file validation (checking file extension and size).
  - On a successful upload, `model.ImageUrl` is set to the URL of the uploaded image.

### Explanation

1. **Image Validation:**
   - Validates image file type and size before uploading to ensure it meets allowed formats and size constraints.
   
2. **Transformation Settings in `ImageUploadParams`:**
   - **Width(300):** Sets the image width to 300 pixels.
   - **Height(300):** Sets the image height to 300 pixels.
   - **Crop("fill"):** Crops the image to fill the specified dimensions, possibly trimming parts of the image to fit.
   - **Gravity("face"):** Centers the crop around detected faces, if any, in the image.

3. **Image Upload:**
   - The image is uploaded to Cloudinary, and the secure URL is saved in `model.ImageUrl`.

4. **Error Handling:** If there is an upload error, an error message is displayed in the view.

## 8. Generate a Thumbnail URL from Cloudinary

Cloudinary makes it easy to create different versions of an image, including thumbnails, by appending transformations directly in the image URL. Here’s how to modify an existing image URL to generate a thumbnail version.

### Original and Thumbnail URLs

- **Original Image URL:**  
  `https://res.cloudinary.com/trojan74/image/upload/v1730144402/5402d336-ace3-437e-926b-611ece11554f_iqvb1n.png`

- **Thumbnail Image URL:**  
  `https://res.cloudinary.com/trojan74/image/upload/c_thumb,w_200,g_face/v1730144402/5402d336-ace3-437e-926b-611ece11554f_iqvb1n.png`

**Difference Between URLs:**  
Note that The only difference between the original and the thumbnail URLs is the added transformation part (`c_thumb,w_200,g_face`). This portion defines the thumbnail parameters.

### Example Method for Generating Thumbnail URL

This method takes the original URL, inserts the transformation string, and returns the new thumbnail URL:

```csharp
private string GetThumbnailImageUrl(string imageUrl)
{
    var transformation = "c_thumb,w_200,g_face";
    var separator = "image/upload/";
    var urlParts = imageUrl.Split(separator);

    return $"{urlParts[0]}{separator}{transformation}/{urlParts[1]}";
}
```

### Explanation

- **Transformation Settings in URL:**
  - **c_thumb:** Specifies a thumbnail transformation, scaling the image for thumbnail purposes.
  - **w_200:** Sets the thumbnail width to 200 pixels.
  - **g_face:** Centers the thumbnail crop around any detected face within the image.

- **Logic of the Method:**
  - **separator** is `"image/upload/"`: Used to split the URL into two parts.
  - **Transformation Concatenation:** By inserting the transformation segment after the separator, this code creates a URL that Cloudinary interprets as a thumbnail request with the specified parameters.


  Certainly! Here’s how to incorporate deleting an image from Cloudinary into your application.

## 9. Delete an Image from Cloudinary

Deleting an image from Cloudinary can be essential for managing resources and optimizing storage. Cloudinary provides an API to delete images by specifying the `PublicId` of the resource.

### Example Code for Deleting an Image

The following code demonstrates how to delete an image in Cloudinary by calling the `DeleteResourcesAsync` method. In this example, the image’s `PublicId` is passed to specify which image to delete.

```csharp
// Delete the image from Cloudinary
await _cloudinary.DeleteResourcesAsync(new DelResParams
{
    PublicIds = new List<string> { book.ImagePublicId }
});

// Update the database record to remove references to the deleted image
book.ImageUrl = null;
book.ImageThumbnailUrl = null;
book.ImagePublicId = null;
```

### Explanation

- **DeleteResourcesAsync Method**:
  - **`_cloudinary.DeleteResourcesAsync(...)`**: This method from the `Cloudinary` class handles the deletion of resources from Cloudinary.
  - **`DelResParams`**: This parameter class allows specifying the deletion parameters, such as the list of `PublicIds` for images to delete.

- **Parameters Passed**:
  - **PublicIds**: A list of `PublicIds` identifies which images to delete. Here, `book.ImagePublicId` refers to the `PublicId` associated with the specific image, which is stored in the database when the image is uploaded.

- **Clear Image References in the Database**:
  - After deleting the image from Cloudinary, it's crucial to update the database record by setting the `ImageUrl`, `ImageThumbnailUrl`, and `ImagePublicId` fields to `null`. This step ensures that the application no longer holds references to the deleted image, preventing broken links and errors.

This approach efficiently manages images on Cloudinary and keeps the database records in sync with the image storage state.