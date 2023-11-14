static string GetFormattedDate()
{
    // Get the current date and time
    DateTime currentDate = DateTime.Now;

    // Format the date string as "dddd MM/dd/yyyy   h:mm tt"
    string formattedDate = currentDate.ToString("dddd MM/dd/yyyy h:mm:ss tt");

    return formattedDate;
}