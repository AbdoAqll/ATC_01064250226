// DOM Elements
const menuToggle = document.querySelector('#menu-open');
const navLinks = document.querySelector('.nav-links');
const searchBar = document.querySelector('.search');
const darkLightToggle = document.querySelector('#theme-toggle');
const container = document.querySelector('.container');

// Event Listeners
darkLightToggle.addEventListener('click', () => {
    document.body.classList.toggle('dark');

    // Change icon between moon and sun
    if (document.body.classList.contains('dark')) {
        darkLightToggle.classList.remove('bxs-moon');
        darkLightToggle.classList.add('bxs-sun');
    } else {
        darkLightToggle.classList.remove('bxs-sun');
        darkLightToggle.classList.add('bxs-moon');
    }
});

menuToggle.addEventListener('click', () => {
    navLinks.classList.toggle('active');
    container.classList.toggle('menu-toggled');

    if (navLinks.classList.contains('active')) {
        // If active, hide search bar and show nav links with gap
        setTimeout(() => {
            searchBar.style.display = 'none';
        }, 300); // Match CSS transition duration
        navLinks.style.display = 'flex';
        navLinks.style.flexDirection = 'column'; // Stack links vertically on mobile
        navLinks.style.gap = '15px';
    } else {
        // If not active, show search bar and reset nav links
        searchBar.style.display = 'flex';
        navLinks.style.display = '';
        navLinks.style.gap = '';
    }
});

// Handle screen size changes
window.addEventListener('resize', () => {
    if (window.innerWidth > 992) { // Match media query breakpoint
        // On desktop, always show both search bar and nav links
        searchBar.style.display = 'flex';
        navLinks.classList.remove('active');
        navLinks.style.display = '';
        navLinks.style.flexDirection = '';
        container.classList.remove('menu-toggled');
    }
});

// Pagination functionality
const eventsPerPage = 6; // Number of events per page
const events = document.querySelectorAll('.left-section .event-card');
const totalEvents = events.length;
const totalPages = Math.ceil(totalEvents / eventsPerPage);

let currentPage = 1;

function displayEvents(page) {
    const start = (page - 1) * eventsPerPage;
    const end = page * eventsPerPage;

    events.forEach((event, index) => {
        if (index >= start && index < end) {
            event.style.display = 'flex'; // Show event card
        } else {
            event.style.display = 'none'; // Hide event card
        }
    });
}

function updatePagination() {
    const paginationLinks = document.querySelectorAll('.page-num');
    paginationLinks.forEach((link, index) => {
        link.classList.remove('active');
        if (index === currentPage - 1) {
            link.classList.add('active');
        }
    });

    // Enable/disable prev/next buttons based on current page
    const prevButton = document.querySelector('.prev');
    const nextButton = document.querySelector('.next');

    prevButton.classList.toggle('disabled', currentPage === 1);
    nextButton.classList.toggle('disabled', currentPage === totalPages);
}

document.querySelector('.pagination').addEventListener('click', (event) => {
    event.preventDefault();

    if (event.target.classList.contains('page-num')) {
        currentPage = parseInt(event.target.textContent);
    } else if (event.target.classList.contains('next') && currentPage < totalPages) {
        currentPage++;
    } else if (event.target.classList.contains('prev') && currentPage > 1) {
        currentPage--;
    }

    displayEvents(currentPage);
    updatePagination();
});

// Initialize the first page
displayEvents(currentPage);
updatePagination();