// Function to handle the live search
function liveSearch() {
    var searchTerm = document.getElementById('searchInput').value.toLowerCase(); // Get the search term
    var tableRows = document.querySelectorAll('table tbody tr'); // Get all table rows

    // Loop through table rows and filter based on the search term
    tableRows.forEach(function (row) {
        var rowText = row.textContent.toLowerCase();
        if (rowText.includes(searchTerm)) {
            row.style.display = 'table-row'; // Show the row if it matches the search term
        } else {
            row.style.display = 'none'; // Hide the row if it doesn't match
        }
    });
}