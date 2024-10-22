# Uploading Images in ASP.NET Core Web Application

In web development, working with images is a common task, and handling file uploads properly is essential for building robust and secure applications. There are three common methods to store images in web applications:

1. **Saving the image directly into the application file system (wwwroot folder)**.
2. **Saving the image in a cloud storage service (e.g., Cloudinary, AWS S3, etc.) and saving the URL**.
3. **Saving the image directly in the database** (not recommended due to performance concerns).

This document focuses on **saving the image into the application file system**, specifically the **wwwroot** directory in an ASP.NET Core application, while ensuring validation for file types and sizes.

## Key Concepts to Handle File Uploads

### File Validation
Before processing any uploaded file, ensure that it meets your application's requirements. Validation should check for:

1. **Allowed file extensions**: Ensuring the file has the correct extension (e.g., `.jpg`, `.png`).
2. **Maximum file size**: Limiting the file size to prevent users from uploading large files that could harm the application's performance.

### Saving the File to the Application

Once the file is validated, you can save it to a specific directory in your project, often located in the `wwwroot` folder, which is a publicly accessible directory. You will use the `IWebHostEnvironment` interface to determine the root path for the application.

---

## Understanding `IWebHostEnvironment` Interface

The `IWebHostEnvironment` interface provides information about the web hosting environment an ASP.NET Core application is running in, and it includes properties for determining the application's root paths. The key properties include:

- **WebRootPath**: The absolute path to the directory that contains the web-servable content, such as images, CSS, and JavaScript files. By default, this is the `wwwroot` folder.
- **ContentRootPath**: The absolute path to the application’s content files (non-web files).

In this context, `IWebHostEnvironment` helps us identify the correct folder to store uploaded images in the `wwwroot` directory.

---

## Example Code: Saving an Image in the Application File System

### Step-by-step breakdown of the example:

1. **File validation**: Validate the uploaded file based on its extension and size.
2. **Determine the save location**: Use `IWebHostEnvironment.WebRootPath` to locate the `wwwroot/images/books` directory.
3. **Save the file**: Save the uploaded image in the determined location with a unique filename.
4. **Store the image URL**: After saving, store the image URL in the database for later use.

### Full Example Code with Comments

```csharp
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookify.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment; // Provides environment-specific paths (like wwwroot)
        private readonly ApplicationDbContext _context; // Database context to interact with the database
        private readonly IMapper _mapper; // AutoMapper to map between view models and entities

        // Define allowed image extensions and the maximum file size (2 MB)
        private readonly List<string> AllowedImageExtensions = new List<string> { ".jpg", ".jpeg", ".png" };
        private readonly long MaxImageSize = 2 * 1024 * 1024; // 2MB

        // Constructor injection of dependencies
        public BooksController(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment; // Inject IWebHostEnvironment to access webroot path
        }

        // Handle the form submission for creating a new book
        [HttpPost]
        [ValidateAntiForgeryToken] // Prevent Cross-Site Request Forgery (CSRF)
        public IActionResult Create(BookFormViewModel model)
        {
            // If model state is invalid, repopulate the view model and return to the form
            if (!ModelState.IsValid)
            {
                model = populateViewModel(model);
                return View("Form", model);
            }

            // Map the view model to the Book entity
            var book = _mapper.Map<Book>(model);

            // Handle image upload if an image is provided
            if (model.Image is not null)
            {
                // Get the file extension of the uploaded image
                var imageExtension = Path.GetExtension(model.Image.FileName);
                
                // Validate the file extension
                if (!AllowedImageExtensions.Contains(imageExtension))
                {
                    ModelState.AddModelError(nameof(model.Image), $"Invalid image format. Only {string.Join(", ", AllowedImageExtensions)} are allowed.");
                    model = populateViewModel(model);
                    return View("Form", model);
                }

                // Validate the file size
                if (model.Image.Length > MaxImageSize)
                {
                    ModelState.AddModelError("Image", "Image size should not exceed 2MB.");
                    model = populateViewModel(model);
                    return View("Form", model);
                }

                // Define the folder path to save the image (wwwroot/images/books)
                var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images/books");
                
                // Generate a unique filename to avoid overwriting existing files
                var uniqueFileName = $"{Guid.NewGuid()}{imageExtension}";
                
                // Create the full file path
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the image file to the specified path
                using (var fileStream = System.IO.File.Create(filePath))
                {
                    model.Image.CopyTo(fileStream);
                }

                // Set the ImageUrl in the Book entity to the unique filename (for later retrieval)
                book.ImageUrl = uniqueFileName;
            }

            // Add the book to the database and save changes
            _context.Books.Add(book);
            _context.SaveChanges();

            // Add the selected categories for the book to the join table
            foreach (var categoryId in model.SelectedCategories)
            {
                var bookCategory = new BookCategory
                {
                    BookId = book.Id,
                    CategoryId = categoryId
                };
                _context.BookCategories.Add(bookCategory);
            }

            // Save the book-category relationships
            _context.SaveChanges();

            // Redirect to the book list (index) after successful creation
            return RedirectToAction(nameof(Index));
        }

        // Populate the view model with category and author options for dropdowns
        private BookFormViewModel populateViewModel(BookFormViewModel? model = null)
        {
            // Fetch active categories and authors from the database
            var categories = _context.Categories.Where(c => !c.IsDeleted).OrderBy(c => c.Name).ToList();
            var authors = _context.Authors.Where(a => !a.IsDeleted).OrderBy(a => a.Name).ToList();

            // Populate the view model with categories and authors
            var viewModel = model ?? new BookFormViewModel();
            viewModel.Categories = _mapper.Map<IEnumerable<SelectListItem>>(categories);
            viewModel.Authors = _mapper.Map<IEnumerable<SelectListItem>>(authors);

            return viewModel;
        }
    }
}
```

---

### Key Steps in the Example:

1. **Dependencies and Setup**:
    - The `IWebHostEnvironment` is injected into the controller to access the application's `wwwroot` folder, where we will save the uploaded images.
    - The `ApplicationDbContext` and `IMapper` are also injected for database operations and model mapping, respectively.

2. **File Validation**:
    - The allowed extensions (`.jpg`, `.jpeg`, `.png`) and maximum file size (2 MB) are defined.
    - The file's extension and size are validated before any further processing.

3. **File Saving**:
    - The image is saved in the `wwwroot/images/books` directory.
    - A unique file name is generated using `Guid.NewGuid()` to prevent overwriting files with the same name.
    - The file is saved using `System.IO.File.Create()`.

4. **Database Interaction**:
    - The image URL (file name) is stored in the `Book` entity, which is then saved to the database along with the selected categories.

---

## Conclusion

This method of saving images directly into the `wwwroot` folder using `IWebHostEnvironment` is simple and efficient for applications that do not require cloud storage. By validating file types and sizes, you can prevent malicious or oversized files from being uploaded, ensuring better security and performance for your application.
