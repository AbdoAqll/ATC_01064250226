/* toggle style switcher */
const styleSwitcherToggle = document.querySelector(".style-switcher-toggler");
styleSwitcherToggle.addEventListener("click", () => {
    document.querySelector(".style-switcher").classList.toggle("open");
})


// hide style switcher on scroll
window.addEventListener("scroll", () => {
    if(document.querySelector(".style-switcher").classList.contains("open"))
    {
        document.querySelector(".style-switcher").classList.remove("open");
    }
})

/* theme light/dark mode */
const dayNight = document.querySelector(".day-night");

// Function to toggle dark mode
function toggleDarkMode(isDark) {
    if (isDark) {
        document.body.classList.add("dark");
        dayNight.querySelector("i").classList.remove("bxs-moon");
        dayNight.querySelector("i").classList.add("bxs-sun");
    } else {
        document.body.classList.remove("dark");
        dayNight.querySelector("i").classList.remove("bxs-sun");
        dayNight.querySelector("i").classList.add("bxs-moon");
    }
}

// Load stored theme preferences on page load
window.addEventListener("load", () => {
    // Retrieve stored color theme
    const storedColor = localStorage.getItem("selectedColor");
    if (storedColor) {
        setActiveStyle(storedColor);
    }

    // Retrieve stored dark mode preference OR set default to dark mode
    const storedDarkMode = localStorage.getItem("darkMode");
    const isDarkMode = storedDarkMode === null || storedDarkMode === "true"; // Default to dark if not set
    toggleDarkMode(isDarkMode);
});

// Toggle dark mode and store the preference
dayNight.addEventListener("click", () => {
    const isDark = !document.body.classList.contains("dark");
    localStorage.setItem("darkMode", isDark);
    toggleDarkMode(isDark);
});


/* Language Selection */


// Function to change language
function changeLanguage(lang, shouldReload = true) {
    document.documentElement.setAttribute("lang", lang);

    // Set direction for right-to-left languages
    const isRTL = lang === "ar";
    document.documentElement.setAttribute("dir", isRTL ? "rtl" : "ltr");
    document.body.setAttribute("dir", isRTL ? "rtl" : "ltr");

    // Store selected language in localStorage
    localStorage.setItem("selectedLanguage", lang);

    // Send request to server to update culture cookie
    fetch(`/User/Home/SetLanguage?culture=${lang}`, {
        method: "GET",
        headers: { "Content-Type": "application/json" }
    }).then(response => {
        if (response.ok) {
            console.log("Language changed successfully.");
            if (shouldReload) {
                location.reload(); // Refresh only when manually changed
            }
        } else {
            console.error("Failed to change language.");
        }
    }).catch(error => console.error("Error changing language:", error));
}

// Function to load stored language preference (default to Arabic)
function loadLanguage() {
    const savedLanguage = localStorage.getItem("selectedLanguage") || "ar"; // Default to Arabic
    const currentLang = document.documentElement.getAttribute("lang");

    // Only change language if it's different from the current one
    if (savedLanguage !== currentLang) {
        changeLanguage(savedLanguage, false); // Don't reload when loading initially
    }

    // Update language select dropdown
    const languageSelect = document.getElementById("languageSwitcher");
    if (languageSelect) {
        languageSelect.value = savedLanguage;
    }
}

// Set the default language on page load
window.addEventListener("load", loadLanguage);

// Event listener for language selection
const languageSelect = document.querySelector(".language-select");
if (languageSelect) {
    languageSelect.addEventListener("change", (event) => {
        changeLanguage(event.target.value); // Allow reload when manually changed
    });
} else {
    console.error("Language select element not found!");
}



