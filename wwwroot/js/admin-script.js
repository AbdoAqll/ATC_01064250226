// Action buttons handling
const actionButtons = document.querySelectorAll('.action-btn');
actionButtons.forEach(btn => {
    btn.addEventListener('click', function (e) {
        e.stopPropagation();
        const action = this.classList.contains('edit') ? 'edit' : 'delete';
        const row = this.closest('tr');
        const itemName = row.querySelector('.event-name') ?
            row.querySelector('.event-name').textContent :
            row.querySelector('.user-name') ?
                row.querySelector('.user-name').textContent :
                'this item';

        if (action === 'edit') {
            // In a real app, this would open an edit modal or navigate to edit page
            alert(`Edit ${itemName}`);
        }
        // Removed the delete confirmation code since we're using a custom confirmation system
    });
});

// Navigation active state handling
function setActiveNavItem() {
    const navItems = document.querySelectorAll('.sidebar .menu ul li');
    const currentPath = window.location.pathname.toLowerCase();
    
    navItems.forEach(item => {
        const link = item.querySelector('a');
        if (link) {
            const href = link.getAttribute('href').toLowerCase();
            // Check if current path matches the menu item's href
            if (currentPath.startsWith(href)) {
                item.classList.add('active');
            } else {
                item.classList.remove('active');
            }
        }
    });
}

// Initialize navigation state
document.addEventListener('DOMContentLoaded', function() {
    // Set initial active state
    setActiveNavItem();

    // Handle navigation clicks
    const navItems = document.querySelectorAll('.sidebar .menu ul li');
    navItems.forEach(item => {
        item.addEventListener('click', function(e) {
            const link = this.querySelector('a');
            if (link) {
                // Remove active class from all items
                navItems.forEach(navItem => navItem.classList.remove('active'));
                // Add active class to clicked item
                this.classList.add('active');
            }
        });
    });
});

// Handle browser back/forward navigation
window.addEventListener('popstate', setActiveNavItem);

document.addEventListener('DOMContentLoaded', function () {
    // Get DOM elements
    const sidebar = document.getElementById('sidebar');
    const mainContent = document.querySelector('.main-content');
    const sidebarToggle = document.getElementById('sidebar-toggle');

    // Single toggle button for both mobile and desktop
    if (sidebarToggle) {
        sidebarToggle.addEventListener('click', function () {
            // On mobile: show/hide sidebar
            if (window.innerWidth <= 768) {
                sidebar.classList.toggle('show');
            }
            // On desktop: collapse/expand sidebar
            else {
                sidebar.classList.toggle('collapsed');
                mainContent.classList.toggle('expanded');
            }
        });
    }

    // Detect outside click to close sidebar in mobile view
    document.addEventListener('click', function (event) {
        const isClickInsideSidebar = sidebar.contains(event.target);
        const isClickOnToggle = sidebarToggle.contains(event.target);

        if (!isClickInsideSidebar && !isClickOnToggle && window.innerWidth <= 768 && sidebar.classList.contains('show')) {
            sidebar.classList.remove('show');
        }
    });

    // Theme toggle
    const themeToggle = document.getElementById('theme-toggle');

    if (themeToggle) {
        // Check for saved theme preference or get from OS preference
        const userPrefersDark = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
        const savedTheme = localStorage.getItem('theme');

        // Apply saved theme or OS preference
        if (savedTheme === 'dark' || (!savedTheme && userPrefersDark)) {
            document.body.classList.add('dark');
            themeToggle.classList.replace('bxs-moon', 'bxs-sun');
        }

        themeToggle.addEventListener('click', function () {
            document.body.classList.toggle('dark');

            if (document.body.classList.contains('dark')) {
                themeToggle.classList.replace('bxs-moon', 'bxs-sun');
                localStorage.setItem('theme', 'dark');
            } else {
                themeToggle.classList.replace('bxs-sun', 'bxs-moon');
                localStorage.setItem('theme', 'light');
            }
        });
    }

    // Active menu item handling
    const menuItems = document.querySelectorAll('.menu ul li');
    const currentPath = window.location.pathname;

    // Function to set active state based on current path
    function setActiveMenuItem() {
        menuItems.forEach(item => {
            const link = item.querySelector('a');
            if (link) {
                const href = link.getAttribute('href');
                // Check if current path starts with the menu item's href
                if (currentPath.startsWith(href)) {
                    item.classList.add('active');
                } else {
                    item.classList.remove('active');
                }
            }
        });
    }

    // Set initial active state
    setActiveMenuItem();

    // Handle menu item clicks
    menuItems.forEach(item => {
        item.addEventListener('click', function (e) {
            // Only handle clicks on the link itself
            if (e.target.tagName === 'A' || e.target.closest('a')) {
                // Remove active class from all items
                menuItems.forEach(i => i.classList.remove('active'));
                // Add active class to clicked item
                this.classList.add('active');
            }
        });
    });

    // Handle browser navigation (back/forward)
    window.addEventListener('popstate', function () {
        setActiveMenuItem();
    });

    // Handle window resize
    window.addEventListener('resize', function () {
        if (window.innerWidth > 768) {
            sidebar.classList.remove('show');
        }

        if (window.innerWidth > 992) {
            sidebar.classList.remove('collapsed');
            mainContent.classList.remove('expanded');
        }
    });
}); 