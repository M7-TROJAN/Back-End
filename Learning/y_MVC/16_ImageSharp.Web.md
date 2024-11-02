# SixLabors.ImageSharp.Web Documentation

`SixLabors.ImageSharp.Web` is an image-processing library for .NET applications optimized for handling images in web contexts, allowing middleware for resizing, cropping, caching, and transforming images on the fly.

## Table of Contents

1. [Getting Started](#getting-started)
2. [Installation](#installation)
3. [Configuring Middleware](#configuring-middleware)
4. [Key Features and Functions](#key-features-and-functions)
   - [Resizing](#resizing)
   - [Cropping](#cropping)
   - [Image Formats](#image-formats)
   - [Quality Settings](#quality-settings)
   - [Caching](#caching)
5. [Examples with Explanations](#examples-with-explanations)
6. [Further Resources](#further-resources)

---

## Getting Started

ImageSharp.Web is specifically designed for web applications to process and transform images on the server. The middleware intercepts image requests and performs operations like resizing, cropping, format conversion, and caching.

## Installation

Install the ImageSharp.Web package from NuGet:

```bash
dotnet add package SixLabors.ImageSharp.Web
```

Or, using the NuGet Package Manager Console:

```bash
Install-Package SixLabors.ImageSharp.Web
```

## Configuring Middleware

To use `ImageSharp.Web`, add it to your ASP.NET Core pipeline in `Program.cs`:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddImageSharp();

var app = builder.Build();

app.UseImageSharp(); // Enables ImageSharp middleware

app.Run();
```

The `AddImageSharp()` method registers required services, and `UseImageSharp()` enables the middleware to handle image requests.

### Customizing Configuration

```csharp
builder.Services.AddImageSharp(options =>
{
    options.Configuration.Default.MaxWidth = 800;
    options.Configuration.Default.MaxHeight = 600;
    options.OnParseCommandsAsync = context =>
    {
        // Custom command parsing logic if needed
        return Task.CompletedTask;
    };
});
```

## Key Features and Functions

### 1. Resizing

Resizing an image is as easy as passing width and height parameters in the URL. Here’s how to configure the default resizing settings in the pipeline:

```csharp
builder.Services.AddImageSharp(options =>
{
    options.Configuration.Default.MaxWidth = 300;
    options.Configuration.Default.MaxHeight = 200;
});
```

To resize an image to specific dimensions:

URL Example: `/images/sample.jpg?width=300&height=200`

**Explanation:**

- **Width and Height Parameters:** The `width` and `height` parameters are used to resize the image on-the-fly to the specified dimensions.
- **Max Width and Height:** The default max width and height prevent excessively large images from consuming too much server memory.

### 2. Cropping

Crop an image by specifying the region of interest using the `rxywh` parameter, which defines the crop region with coordinates and width/height.

URL Example: `/images/sample.jpg?width=300&height=300&rxywh=100,100,200,200`

**Explanation:**

- **Region Crop (rxywh):** The `rxywh` parameter specifies the cropping region in the format `x, y, width, height`.
- **Center Crop:** ImageSharp automatically handles the crop based on the center if specified. This allows for custom dimensions and placements within the image.

### 3. Image Formats

Convert between formats like JPEG, PNG, BMP, and WebP by passing `format` in the URL query.

URL Example: `/images/sample.jpg?format=webp`

**Explanation:**

- **Format Conversion:** The `format` parameter changes the output format to the specified type, such as `jpeg` or `webp`. WebP often reduces file size without sacrificing quality.

### 4. Quality Settings

Adjust the quality of images, especially JPEG and WebP, using the `quality` parameter, where the value is a percentage from 0 to 100.

URL Example: `/images/sample.jpg?quality=80`

**Explanation:**

- **Quality Adjustment:** Lowering the quality reduces file size, often beneficial for faster load times on the web. Quality settings affect only certain formats, like JPEG and WebP.

### 5. Caching

Configure caching options to prevent re-processing of images on repeated requests.

```csharp
builder.Services.AddImageSharp(options =>
{
    options.BrowserMaxAge = TimeSpan.FromDays(7);
    options.CacheMaxAge = TimeSpan.FromDays(365);
});
```

**Explanation:**

- **Caching Options:** Caching prevents the server from reprocessing the image each time it’s requested. `BrowserMaxAge` specifies the max cache duration in the browser, and `CacheMaxAge` specifies it on the server side.
- **Cache Control:** These settings optimize performance by allowing faster load times and reduced server strain.

## Examples with Explanations

### Example 1: Basic Image Resizing

To resize an image to a width of 150px and height of 100px:

```plaintext
/images/photo.jpg?width=150&height=100
```

**Explanation:**

- This URL applies a basic resize to the image `photo.jpg`, setting the dimensions to 150x100 pixels. It’s useful for generating thumbnails or lower-resolution images for faster page load times.

### Example 2: Advanced Transformation with Crop and Quality Adjustment

To crop, adjust format, and set quality:

```plaintext
/images/photo.jpg?width=300&height=300&rxywh=50,50,150,150&format=jpeg&quality=80
```

**Explanation:**

- **Width and Height:** Sets the output size of the image to 300x300 pixels.
- **Crop Region (rxywh):** Crops to the rectangle at (50,50) with width and height of 150.
- **Format:** Converts to JPEG format, allowing compression.
- **Quality:** Reduces the JPEG quality to 80%, balancing file size and quality.

### Example 3: Using ImageSharp Processing Commands in Code

You can apply transformations directly in code by using the `Image` class from `SixLabors.ImageSharp`:

```csharp
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

public async Task ProcessImageAsync(Stream inputStream, Stream outputStream)
{
    using var image = await Image.LoadAsync(inputStream);
    image.Mutate(x => x
        .Resize(400, 300)
        .Crop(new Rectangle(50, 50, 100, 100))
        .AutoOrient());
    await image.SaveAsync(outputStream);
}
```

**Explanation:**

- **Resize:** The image is resized to 400x300.
- **Crop:** The image is cropped to a 100x100 section starting at coordinates (50,50).
- **AutoOrient:** Automatically adjusts orientation based on EXIF data.
- **SaveAsync:** Saves the processed image to the output stream, which can then be used to serve it to the user.

## Further Resources

For more details, visit the official [ImageSharp.Web documentation on GitHub](https://github.com/SixLabors/ImageSharp.Web). This resource provides in-depth information on customizations, advanced configurations, and additional features.


## Create Example


```csharp
public async Task<IActionResult> Create(BookFormViewModel model)
{
    if (!ModelState.IsValid)
    {
        model = populateViewModel(model);

        return View("Form", model);
    }

    if (model.Image is not null)
    {
        var imageExtension = Path.GetExtension(model.Image.FileName);
        if (!AllowedImageExtensions.Contains(imageExtension))
        {
            ModelState.AddModelError(nameof(model.Image), $"Invalid image format. Only {string.Join(", ", AllowedImageExtensions)} are allowed.");
            model = populateViewModel(model);

            return View("Form", model);
        }

        if (model.Image.Length > MaxImageSize)
        {
            ModelState.AddModelError("Image", "Image size should not exceed 2MB.");
            model = populateViewModel(model);

            return View("Form", model);
        }

        // to uoload the image to the server
        var imageName = $"{Guid.NewGuid()}{imageExtension}"; // make the image name unique

        var path = Path.Combine($"{_hostingEnvironment.WebRootPath}/images/books", imageName); // wwwroot/images/books/imageName
        var thumbPath = Path.Combine($"{_hostingEnvironment.WebRootPath}/images/books/thumb", imageName); // wwwroot/images/books/thumb/imageName

        using var stream = System.IO.File.Create(path); // create the image file
        await model.Image.CopyToAsync(stream); // copy the image to the file
        stream.Dispose(); // close the stream

        model.ImageUrl = $"/images/books/{imageName}";
        model.ImageThumbnailUrl = $"/images/books/thumb/{imageName}";

        using var image = Image.Load(model.Image.OpenReadStream()); // use ImageSharp package to load the image
        var ratio = (float)image.Width / 200; // calculate the ratio (ratio means the width of the image divided by the new width you want)
        var height = image.Height / ratio; // calculate the new height based on the ratio
        image.Mutate(i => i.Resize(width: 200, height: (int)height)); // resize the image
        image.Save(thumbPath); // save the thumbnail image
    }

    var Book = _mapper.Map<Book>(model); // map the view model to the entity model

    _context.Books.Add(Book);

    foreach (var categoryId in model.SelectedCategories)
    {
        Book.Categories.Add(new BookCategory
        {
            CategoryId = categoryId
        });
    }

    _context.SaveChanges();

    return RedirectToAction(nameof(Index));
}
```

the breakdown of the `Create` Action

---

```csharp
public async Task<IActionResult> Create(BookFormViewModel model)
{
    if (!ModelState.IsValid)
    {
        model = populateViewModel(model);

        return View("Form", model);
    }
```

### Explanation

- **Model Validation Check:** This code block verifies if the model passed to the controller contains valid data based on validation attributes.
- **ModelState.IsValid:** If any validation fails, `ModelState.IsValid` returns `false`, meaning we don’t proceed with saving the data and return the form view again with the populated model data.
- **populateViewModel(model):** This is a helper method that likely populates additional data (like dropdowns or lists) in the `BookFormViewModel`.

---

```csharp
if (model.Image is not null)
{
    var imageExtension = Path.GetExtension(model.Image.FileName);
    if (!AllowedImageExtensions.Contains(imageExtension))
    {
        ModelState.AddModelError(nameof(model.Image), $"Invalid image format. Only {string.Join(", ", AllowedImageExtensions)} are allowed.");
        model = populateViewModel(model);

        return View("Form", model);
    }
```

### Explanation

- **Image Null Check:** This block checks if an image file has been uploaded.
- **Get File Extension:** Using `Path.GetExtension`, we extract the file extension of the uploaded image.
- **Allowed Image Extensions Validation:** `AllowedImageExtensions` is likely a predefined list of allowed extensions (e.g., `.jpg`, `.png`). If the uploaded image’s extension isn’t allowed, an error is added to the `ModelState`.
- **ModelState.AddModelError:** Adds a specific error for the image field if the file type is invalid. This will display an error message on the form view.
  
---

```csharp
    if (model.Image.Length > MaxImageSize)
    {
        ModelState.AddModelError("Image", "Image size should not exceed 2MB.");
        model = populateViewModel(model);

        return View("Form", model);
    }
```

### Explanation

- **File Size Check:** Validates that the uploaded image does not exceed the maximum allowed size (`MaxImageSize`). If the file size is too large, an error is displayed.
- **MaxImageSize:** A constant that defines the max allowed file size for images (in bytes).
  
---

```csharp
    // to upload the image to the server
    var imageName = $"{Guid.NewGuid()}{imageExtension}"; // make the image name unique

    var path = Path.Combine($"{_hostingEnvironment.WebRootPath}/images/books", imageName); // wwwroot/images/books/imageName
    var thumbPath = Path.Combine($"{_hostingEnvironment.WebRootPath}/images/books/thumb", imageName); // wwwroot/images/books/thumb/imageName
```

### Explanation

- **Unique Image Name:** Creates a unique filename using a GUID, preventing filename collisions.
- **Image Paths:** Sets the paths for the main image and thumbnail inside the `wwwroot` folder. The `WebRootPath` property points to the root of the `wwwroot` folder, where static files are stored.
  
---

```csharp
    using var stream = System.IO.File.Create(path); // create the image file
    await model.Image.CopyToAsync(stream); // copy the image to the file
    stream.Dispose(); // close the stream
```

### Explanation

- **Stream Creation and Copy:** Opens a file stream to create the file at the specified path and copies the uploaded image data into the file asynchronously.
- **Dispose:** Disposes of the stream to free up resources after copying is complete.
  
---

```csharp
    model.ImageUrl = $"/images/books/{imageName}";
    model.ImageThumbnailUrl = $"/images/books/thumb/{imageName}";
```

### Explanation

- **Set Image URLs:** Saves the paths to the uploaded image and thumbnail within the `BookFormViewModel`. This enables the view to access the images directly from the `wwwroot` folder.
  
---

```csharp
    using var image = Image.Load(model.Image.OpenReadStream()); // use ImageSharp package to load the image
    var ratio = (float)image.Width / 200; // calculate the ratio (ratio means the width of the image divided by the new width you want)
    var height = image.Height / ratio; // calculate the new height based on the ratio
    image.Mutate(i => i.Resize(width: 200, height: (int)height)); // resize the image
    image.Save(thumbPath); // save the thumbnail image
```

### Explanation

- **Load Image:** Loads the image using ImageSharp for further processing, specifically for creating a thumbnail.
- **Calculate Resize Ratio:** Calculates a scaling ratio based on the desired width (`200` pixels).
- **Resizing Image:** Resizes the image using the calculated width and height while maintaining the aspect ratio.
- **Save Thumbnail:** Saves the resized image to the thumbnail path (`thumbPath`).

---

```csharp
var book = _mapper.Map<Book>(model); // map the view model to the entity model
```

### Explanation

- **Mapping ViewModel to Entity Model:** Uses `AutoMapper` to convert the `BookFormViewModel` into a `Book` entity. This ensures that the view model properties are transferred to the entity model before saving.

---

```csharp
_context.Books.Add(book);
```

### Explanation

- **Add Book to Context:** Adds the newly created `Book` object to the database context. This prepares the data to be saved to the database.
  
---

```csharp
foreach (var categoryId in model.SelectedCategories)
{
    book.Categories.Add(new BookCategory
    {
        CategoryId = categoryId
    });
}
```

### Explanation

- **Handling Many-to-Many Relationships:** Loops through the `SelectedCategories` (a list of category IDs associated with the book) and creates new entries in the `BookCategory` join table to establish relationships in the database.

---

```csharp
_context.SaveChanges();
```

### Explanation

- **Save Changes to Database:** Saves all changes made to the `Book` entity and related `BookCategory` entries into the database.

---

```csharp
return RedirectToAction(nameof(Index));
}
```

### Explanation

- **Redirect After Success:** Redirects to the `Index` action of the controller after successfully creating and saving the new book entry. This returns the user to the main book list page.

---

### Summary of Key Concepts

- **Model Validation:** Ensures the input data meets required criteria before saving.
- **File Handling and Validation:** Checks the uploaded image for allowed formats and maximum size.
- **ImageSharp Processing:** Utilizes `ImageSharp` to create and save resized thumbnail images.
- **Entity Mapping:** Uses `AutoMapper` to map between `BookFormViewModel` and `Book` entity.
- **Database Interaction:** Adds the book and related categories to the database context, saving them as a single transaction.
