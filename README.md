# Evently - Event Booking System

## Abstract

Evently is a full-stack event booking system designed to simplify the process of browsing, booking, and managing events. The platform allows users to explore a variety of events, book events, and manage their bookings seamlessly. Additionally, it features an integrated web-based admin panel that enables event organizers to manage event details, view bookings, and update event information.

## Table of Contents

1. [Used Technology](#used-technology)
2. [Architecture](#architecture)
3. [Authentication](#authentication)
4. [Translation Methodology](#translation-methodology)
5. [Demo Video](#demo-video)
6. [Future Work](#future-work)
7. [Operating Instruction](#operating-instruction)

---

## Used Technology

Evently is built using modern web development technologies that support scalability, performance, and maintainability:

- **Frontend**:

  - HTML, CSS, and JavaScript for building a responsive and interactive user interface with dark light mode support.

- **Backend**:

  - ASP.NET Core MVC for handling server-side logic and routing.
  - Entity Framework Core for ORM (Object Relational Mapping) to interact with the database.

- **Database**:

  - Microsoft SQL Server for reliable, relational data storage and management.

- **Other Tools**:
  - Git for version control
  - Visual Studio for full-stack development and debugging

---

## Architecture

Evently follows a **3-Tier Architecture** to promote separation of concerns, maintainability, and scalability. The system is divided into three main layers:

### 1. Data Access Layer (DAL)

- Implements the **Repository Pattern** and **Unit of Work Pattern** to abstract the database logic from the rest of the application.
- A **Generic Repository** is used to avoid code duplication for common CRUD operations.
- This design enables flexibility, allowing the data source (e.g., SQL Server) to be changed with minimal code impact.

### 2. Business Logic Layer (BLL)

- Contains all core logic and application rules related to event booking and management.
- Maps entities to view models and vice versa.
- Uses a **ServicesProvider** to encapsulate services and manage operations like accessing the Unit of Work or composing services.

### 3. Presentation Layer

- Built using **ASP.NET Core MVC**, this layer handles routing, user interaction, and data presentation through HTML views.
- Communicates with the Business Logic Layer to get and display processed data.

This layered architecture helps ensure that the system is modular, testable, and easy to maintain or extend.

---

## Authentication

Evently uses **ASP.NET Core Identity** for secure user authentication and authorization. The platform is secured using **role-based access control**, with two main roles:

- **User**:

  - Can register, log in, browse available events, book events, and manage personal bookings.

- **Admin**:
  - Has full access to the admin panel.
  - Can manage events, view all bookings, and perform administrative actions.

The use of Identity ensures secure password hashing, user management, and easy extension for features like email confirmation, password reset, and role assignment.

---

## Translation Methodology

Evently supports multilingual functionality using **.NET Localization and Globalization** features for both static and dynamic content:

### 1. Static Content

- Implemented using **resource (.resx) files**.
- Each view uses localized strings to provide language-specific UI text.
- Supports easy extension to new languages by simply adding new resource files.

### 2. Dynamic Content (Stored in Database)

To handle dynamic content translations (such as event names and descriptions from the database), two approaches were considered:

- **Option 1: On-the-fly translation**

  - Using APIs like **Google Translate** or **Gemini (LLM) integration** to dynamically translate content.
  - _Rejected_ due to potential latency and inaccuracies in translation.

- **Option 2: Manual translations stored in the database**
  - _Chosen_ for better accuracy, consistency, and performance.
  - The database schema is designed to store translations for multiple languages, not limited to just English and Arabic.
  - Based on the user's current localization settings, the system retrieves the appropriate translation for each record.

This approach ensures scalable, accurate, and performant multilingual support throughout the application.

---

## Demo Video

[Watch the Demo](https://youtu.be/E7MknY-5tR0) <!-- Replace # with actual video link -->

---

## Future Work

Several enhancements are planned for the future to improve the platform:

- **Unit Testing**:  
  I plan to implement unit testing for the project. Thanks to the layered architecture and use of interfaces, the system is already well-structured and ready for testability. However, due to current commitments with final exams and my graduation project, I was not able to add unit tests at this stage.

- **Payment Integration**:  
  Integration with **Stripe** is planned to enable users to securely pay for event bookings directly through the platform.

- **Deployment**:  
  I will host the application online to make it accessible publicly for real-world usage and testing.

These features will enhance the reliability, usability, and professionalism of the Evently platform.

---

## Operating Instruction

To get started with the Evently project, follow the instructions below:

### Prerequisites

Make sure you have the following installed:

- **Visual Studio 2022+**
- **.NET 8 SDK**
- **Microsoft SQL Server**
- **SSMS (SQL Server Management Studio)** (optional but recommended)

### Installation

1. **Clone the repository**:

   ```bash
   git clone https://github.com/AbdoAqll/ATC_01064250226.git
   cd ATC_01064250226
   ```

2. **Configure the database connection string**:

   - Navigate to the `Evently` project.
   - Open the `appsettings.json` file.
   - Add the following section **after** `"AllowedHosts": "*",`:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Data Source=.;Initial Catalog=EventlyDB;Integrated Security=True;Encrypt=False;Trust Server Certificate=True"
     }
     ```
   - ⚠️ If you're not using the default SQL Server setup, **replace the connection string** with your custom one.

3. **Apply the latest database migration**:

   - Go to **Tools > NuGet Package Manager > Package Manager Console**.
   - Set the **Default Project** in the console to `DataAccess`.
   - Run the following command:
     ```bash
     Update-Database
     ```

4. **Build and run the application**:

   - Build the solution in Visual Studio.
   - Press **F5** or click the **Run** button.

5. **Access the application**:
   - Open your browser and navigate to:
     ```
     https://localhost:44318
     ```
     or
     ```
     https://localhost:7116
     ```

---

## Default Admin Credentials

To log in as an admin and manage events/bookings, use the following credentials:

- **Email**: `admin@admin.com`
- **Password**: `Admin@123`

---

Feel free to contribute, report issues, or suggest improvements!
