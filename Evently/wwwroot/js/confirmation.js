function showConfirmation(url) {
    document.getElementById("confirmation-overlay").style.display = "block";

    document.getElementById("confirm-delete").onclick = function () {
        deleteItem(url);
    };
}

function hideConfirmation() {
    document.getElementById("confirmation-overlay").style.display = "none";
}

function deleteItem(url) {
    fetch(url, { method: "DELETE" })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                // Store message in localStorage before reloading
                localStorage.setItem("toastrMessage", data.message);
                localStorage.setItem("toastrType", "error");
            } else {
                localStorage.setItem("toastrMessage", data.message);
                localStorage.setItem("toastrType", "warning");
            }

            location.reload(); // Reload the page
        })
        .catch(error => {
            alert("An error occurred: " + error);
        });

    hideConfirmation();
}

// Check for stored message after page reload
window.onload = function () {
    let message = localStorage.getItem("toastrMessage");
    let type = localStorage.getItem("toastrType");

    if (message) {
        toastr[type](message); // Show toaster message
        localStorage.removeItem("toastrMessage"); // Remove after showing
        localStorage.removeItem("toastrType");
    }
};

