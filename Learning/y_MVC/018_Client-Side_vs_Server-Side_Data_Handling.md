## Client-Side vs. Server-Side Data Handling

### When to Use Client-Side or Server-Side Data Handling

- **Client-Side:** Suitable when working with smaller datasets (generally up to a few thousand records). Here, the client fetches all the data from the server at once, allowing users to sort, search, and paginate on the client side without further server interaction. This minimizes server calls and allows for quick, responsive data interaction.
  
- **Server-Side:** Recommended for larger datasets (5,000+ records). This approach reduces the load on the client and network by only fetching data as needed (for example, page by page). It helps prevent performance issues on the client by offloading work to the server.

---

### Client-Side Data Handling Example

With client-side data handling, the server sends all data to the client, which then handles all user interactions, like filtering, sorting, and pagination. Below is an example setup for client-side data handling in an ASP.NET Core MVC application.

### 1. Controller

The `AuthorsController` fetches all author records from the database and sends them to the client. 

```csharp
public class AuthorsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AuthorsController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var authors = _context.Authors
            .AsNoTracking()
            .ToList();

        var viewModel = _mapper.Map<IEnumerable<AuthorViewModel>>(authors);

        return View(viewModel);
    }
}
```

### Explanation:

- **Database Querying:** Fetches all authors using `_context.Authors.AsNoTracking().ToList()` to minimize memory usage and enable read-only data access.
- **Mapping Data:** Maps the list of `Author` entities to `AuthorViewModel` instances using AutoMapper for easier use in the view.
- **Returning the View:** Sends the view model to the `Index` view to display the data on the client.

---

### 2. View (Index.cshtml)

The view displays the list of authors in a DataTable, with all interactions handled on the client side.

```cshtml
<div class="card shadow-sm">
    <partial name="_DataTablesCardHeader" /> <!-- Contains search input and export buttons -->
    <div class="card-body pt-0">
        <div class="table-responsive">
            <table class="table table-row-dashed table-row-gray-300 gy-7 js-datatables" data-document-title="Authors Report">
                <thead>
                    <tr class="fw-bold fs-6 text-gray-800">
                        <th>Name</th>
                        <th>Status</th>
                        <th>Created On</th>
                        <th>Last Updated On</th>
                        <th class="js-no-export">Action</th>
                    </tr>
                </thead>
                <tbody>
                    @foreach (var author in Model)
                    {
                        <partial name="_AuthorRow" model="@author" />
                    }
                </tbody>
            </table>
        </div>
    </div>
</div>

@Html.AntiForgeryToken()

@section Plugins {
    <!-- DataTables JS -->
    <script src="~/assets/plugins/datatables/datatables.bundle.js"></script>
}

@section Scripts {
    <!-- Client Side Validation -->
    <partial name="_ValidationScriptsPartial" />
}
```

### Explanation:

- **DataTables Integration:** A DataTable (`js-datatables`) is used for displaying data. The library provides client-side sorting, searching, and pagination functionality.
- **Partial View (_DataTablesCardHeader):** Includes additional UI elements like search inputs and export buttons.
- **Token Inclusion:** The `@Html.AntiForgeryToken()` ensures security for form submissions within this view.

---

### 3. Partial View for Row Content (_AuthorRow.cshtml)

The `_AuthorRow` partial view renders a single row for each author.

```cshtml
@model AuthorViewModel
<tr>
    <td>@Model.Name</td>
    <td>
        <span class="badge badge-light-@(Model.IsDeleted ? "danger" : "success") js-status">
            @(Model.IsDeleted ? "Deleted" : "Available")
        </span>
    </td>
    <td>@Model.CreatedOn</td>
    <td class="js-updated-on">@Model.LastUpdatedOn</td>
    <td>
        <button type="button" class="btn btn-sm btn-icon btn-color-primary" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-end">
            <i class="bi bi-three-dots fs-2x align-self-start"></i>
        </button>
        <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg-light-primary fw-semibold w-200px py-3" data-kt-menu="true">
            <div class="menu-item px-3">
                <a href="javascript:;" class="menu-link px-3 js-render-modal" data-title="Edit Author" data-url="Authors/Edit/@Model.Id" data-update="true">
                    Edit
                </a>
            </div>
            <div class="menu-item px-3">
                <a href="javascript:;" class="menu-link flex-stack px-3 js-toggle-status" data-url="Authors/ToggleStatus/@Model.Id">
                    Toggle Status
                </a>
            </div>
        </div>
    </td>
</tr>
```

### Explanation:

- **Row Data Binding:** Each row displays details like the author's name, status, created date, and updated date.
- **Conditional Styling:** The status label style changes dynamically based on the `IsDeleted` property.
- **Action Menu:** Provides options to edit or toggle an author's status without reloading the page by using modals and AJAX requests.

---

### Benefits and Limitations

#### Client-Side Benefits:
- **Less Server Load:** Fewer server requests after initial data load.
- **Immediate Data Interaction:** Quick response times for sorting, filtering, and pagination, as all data is on the client.

#### Client-Side Limitations:
- **Memory and Processing on Client:** Large datasets can slow down the client device.
- **Initial Load Time:** Loading all data initially can be slow if the dataset is large.

In **server-side** processing, the server handles these operations, sending only the required data for each request. This is beneficial for large datasets and is typically recommended for tables with more than 5,000 records. Server-side processing reduces the load on the client and improves overall performance.

### Server-Side

Server-side data handling is a powerful way to manage large datasets efficiently by offloading data processing to the server rather than the client. This approach ensures faster load times for users and reduces strain on client devices, especially when dealing with large tables or complex data queries.

### Why Use Server-Side Data Handling?

For applications with thousands of records (e.g., over 5000), server-side data handling is highly recommended. This setup allows:

1. **Efficient Data Loading**: Only the requested data is sent to the client, reducing initial load time and memory usage on the client side.
2. **Advanced Filtering, Sorting, and Paging**: The server can handle heavy queries, making it faster to navigate large datasets.
3. **Improved Performance on Low-End Devices**: Offloading processing to the server reduces the workload on the client device, especially for devices with limited processing power or memory.

In contrast to client-side handling, where all data is loaded initially and handled within the browser, server-side processing allows the server to manage data requests as they come.

---

### Setting Up Server-Side Data Handling in ASP.NET Core

Let’s explore how to set up server-side data handling for a table of `Authors` in ASP.NET Core. 

---

### Example: Implementing Server-Side Data Handling for the `Authors` Table

#### Step 1: Setting Up the Controller

In the controller, we need to define an endpoint that handles requests from the DataTables client library on the server side. This endpoint should support filtering, sorting, and paging as required by DataTables.

**Controller (Server-Side)**

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class AuthorsController : Controller
{
    private readonly ApplicationDbContext _context;

    public AuthorsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> LoadAuthors([FromBody] DataTableRequest request)
    {
        var query = _context.Authors.AsQueryable();

        // Filtering
        if (!string.IsNullOrEmpty(request.Search.Value))
        {
            query = query.Where(a => a.Name.Contains(request.Search.Value));
        }

        // Sorting
        var columnToSort = request.Columns[request.Order[0].Column].Name;
        var isAscending = request.Order[0].Dir == "asc";
        query = isAscending ? query.OrderBy(columnToSort) : query.OrderByDescending(columnToSort);

        // Paging
        var totalRecords = await query.CountAsync();
        var authors = await query.Skip(request.Start).Take(request.Length).ToListAsync();

        // Preparing the response for DataTables
        var response = new DataTableResponse
        {
            Draw = request.Draw,
            RecordsTotal = totalRecords,
            RecordsFiltered = totalRecords,
            Data = authors
        };

        return Json(response);
    }
}
```

### Explanation of Each Section in the Controller

1. **DataTableRequest**: This model holds DataTables parameters sent in the request, including filters, sorting options, and paging values.
2. **Filtering**: This section checks if there is a search value and filters the query accordingly.
3. **Sorting**: Determines the column to sort by and the sort order (ascending or descending) based on DataTables' order request.
4. **Paging**: Specifies how many records to skip and how many to take, based on the `Start` and `Length` properties.
5. **Response**: Returns the filtered, sorted, and paged data, along with the total record count.

---

### Step 2: Creating the View for Server-Side Processing

We now need to create a view that includes DataTables with server-side processing enabled.

**View (Server-Side DataTable)**

```cshtml
@model IEnumerable<AuthorViewModel>

<div class="card shadow-sm">
    <div class="card-body pt-0">
        <div class="table-responsive">
            <table class="table table-striped table-bordered" id="authorsTable">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Status</th>
                        <th>Created On</th>
                        <th>Last Updated On</th>
                    </tr>
                </thead>
            </table>
        </div>
    </div>
</div>

@section Scripts {
    <script>
        $(document).ready(function () {
            $('#authorsTable').DataTable({
                "processing": true,
                "serverSide": true,
                "ajax": {
                    "url": "/Authors/LoadAuthors",
                    "type": "POST",
                    "dataSrc": "data"
                },
                "columns": [
                    { "data": "Name" },
                    { "data": "Status" },
                    { "data": "CreatedOn" },
                    { "data": "LastUpdatedOn" }
                ]
            });
        });
    </script>
}
```

### Explanation of the DataTables Setup in the View

- **Processing and ServerSide Options**: These settings indicate that the table will use server-side processing.
- **AJAX URL**: The URL pointing to the server-side endpoint we created in `LoadAuthors`.
- **Columns Definition**: Maps each column to the corresponding property in the `AuthorViewModel`.

---

### Client-Side JavaScript (Optional)

In some cases, you may need additional JavaScript to handle interactions or custom filters.

```javascript
$('#authorsTable').DataTable({
    "processing": true,
    "serverSide": true,
    "ajax": {
        "url": "/Authors/LoadAuthors",
        "type": "POST",
        "data": function (d) {
            d.customFilter = $('#customFilter').val();
        }
    },
    "columns": [
        { "data": "Name" },
        { "data": "Status" },
        { "data": "CreatedOn" },
        { "data": "LastUpdatedOn" }
    ]
});
```

### Explanation of Additional JavaScript

- **Custom Filter Example**: If you want to add a custom filter, you can send additional data to the server by extending the `data` function in the AJAX request.

---

### Summary: Server-Side vs. Client-Side Handling

| Feature                  | Client-Side                                | Server-Side                                 |
|--------------------------|--------------------------------------------|---------------------------------------------|
| **Data Size**            | Limited (better for small datasets)        | Scalable (works well with large datasets)   |
| **Processing Location**  | Client Browser                             | Server                                      |
| **Data Load Time**       | Slower for large datasets                  | Faster for large datasets                   |
| **Resource Usage**       | High on the client’s device                | Offloaded to the server                     |
| **Example**              | Fetch all records in one call              | Fetch records page by page                  |

Server-side data handling is a vital tool for managing larger datasets in a way that is efficient, reduces memory load on the client, and provides a smooth user experience. This method ensures the application remains performant and scalable as the data grows.
