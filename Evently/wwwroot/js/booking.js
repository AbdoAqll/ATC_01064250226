function incrementQuantity() {
    var input = document.getElementById('quantity');
    var value = parseInt(input.value);
    if (value < 10) {
        input.value = value + 1;
        updateTotalPrice();
    }
}

function decrementQuantity() {
    var input = document.getElementById('quantity');
    var value = parseInt(input.value);
    if (value > 1) {
        input.value = value - 1;
        updateTotalPrice();
    }
}

function updateTotalPrice() {
    var quantity = parseInt(document.getElementById('quantity').value);
    var price = parseFloat(document.getElementById('eventPrice').value);
    var total = quantity * price;
    // Use Intl.NumberFormat for proper currency formatting based on culture
    var formatter = new Intl.NumberFormat(document.documentElement.lang, {
        style: 'currency',
        currency: 'USD'
    });
    document.getElementById('totalPrice').textContent = formatter.format(total);
}

// Initialize total price on page load
document.addEventListener('DOMContentLoaded', function() {
    if (document.getElementById('quantity')) {
        updateTotalPrice();
    }
}); 