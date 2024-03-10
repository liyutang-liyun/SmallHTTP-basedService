document.addEventListener("DOMContentLoaded", function () {
    loadLeaderboard();
});

function loadLeaderboard() {
    displaycustomer();
}

function displaycustomer() {
    fetch("/api/leaderboard")
        .then(response => response.json())
        .then(data => {
            displayLeaderboard(data);
        })
        .catch(error => {
            console.error("Error loading leaderboard:", error);
            displayError("Failed to load leaderboard. Please try again later.");
        });
}

function displayLeaderboard(customers) {
    const leaderboardDiv = document.getElementById("leaderboard");
    leaderboardDiv.innerHTML = "";

    if (customers.length === 0) {
        leaderboardDiv.innerHTML = "<p>No customers found.</p>";
        return;
    }

    const table = document.createElement("table");
    const headerRow = table.insertRow();
    const headers = ["Rank", "Customer ID", "Score"];

    headers.forEach(headerText => {
        const header = document.createElement("th");
        header.textContent = headerText;
        headerRow.appendChild(header);
    });

    customers.forEach(customer => {
        const row = table.insertRow();
        row.insertCell().textContent = customer.rank;
        row.insertCell().textContent = customer.customerId;
        row.insertCell().textContent = customer.score;
    });

    leaderboardDiv.appendChild(table);
}

function displayError(message) {
    const errorDiv = document.createElement("div");
    errorDiv.classList.add("error");
    errorDiv.textContent = message;

    const leaderboardDiv = document.getElementById("leaderboard");
    leaderboardDiv.innerHTML = "";
    leaderboardDiv.appendChild(errorDiv);
}

function updateScore(customerId, scoreDelta) {
    fetch(`/api/customer/${customerId}/score/${scoreDelta}`, {
        method: 'POST'
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to update score');
            }
            return response.json();
        })
        .then(data => {
            console.log('Score updated successfully:', data);
            displaycustomer();
        })
        .catch(error => {
            console.error('Error updating score:', error);
        });
}
document.getElementById("updateScoreForm").addEventListener("submit", function (event) {
    event.preventDefault();
    const customerId = document.getElementById("customerId").value;
    const scoreDelta = document.getElementById("scoreDelta").value;
    updateScore(customerId, scoreDelta);
});

function getCustomerData() {
    const customerId = prompt("Enter Customer ID:");
    if (customerId) {
        getCustomerById(customerId, 0, 100); 

    }
}
function getCustomerById(customerId, high, low) {
    fetch(`/api/leaderboard/${customerId}?high=${high}&low=${low}`)
        .then(response => response.json())
        .then(data => {
            console.log('Customer data:', data);
            displaycustomer();
        })
        .catch(error => {
            console.error('Error fetching customer data:', error);
        });
}
