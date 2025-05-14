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