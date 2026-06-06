# dr24CoreNet Medical Appointment Enterprise Platform

dr24CoreNet is a comprehensive and advanced solution for online doctor appointment management, focusing on scalability, concurrency, and medical data security. The project is implemented using Clean Architecture and follows SOLID principles.

## Key Technical Features

### 1. Dual-Layer Concurrency Management (Race Condition)
To prevent simultaneous booking of the same slot by two users, the system uses two mechanisms:
- **Pessimistic Locking**: Uses `DistributedLockService` at the memory level (or Redis in the future) to temporarily lock the slot at the time of request.
- **Optimistic Concurrency**: Uses the `RowVersion` field at the database level (Entity Framework Core) to ensure data hasn't been changed by another transaction.

### 2. Advanced Design Patterns
- **Strategy Pattern**: For dynamic calculation of platform commissions based on doctor specialization without using complex conditional structures.
- **Adapter Pattern**: Simulating connection to legacy Medical Council services and converting traditional protocols to modern system models.
- **Unit of Work & Repository**: Complete separation of data access logic from business logic.

### 3. Security and Tracking (HIPAA Compliance)
- **Medical Audit Trail**: Captures all sensitive operations (booking, prescription issuance, wallet changes) with full before-and-after state details in JSON.
- **Electronic Prescription**: Electronic prescription issuance system with traceability in the digital health record.

### 4. Performance Optimization
- **Analytics Snapshots**: Uses Background Workers to generate statistical snapshots (similar to Materialized Views) to avoid heavy financial queries on main tables.
- **Memory Cache**: Caching doctors and specializations lists with automatic invalidation after data changes.

---

## Project Structure (Clean Architecture)

```text
dr24CoreNet/
├── dr24CoreNet.Domain/          # Entities, Enums, and Core Contracts
├── dr24CoreNet.Application/     # Business Logic, Services, and Interfaces
├── dr24CoreNet.Infrastructure/  # DB Implementation, Logging, Security, and External Services
├── dr24CoreNet.WebAPI/          # RESTful Web Services for Mobile Apps
└── dr24CoreNet.WebUI/           # Web User Interface (Razor Pages) with Tailwind CSS
```

---

## Quick Start Guide

### Prerequisites
- .NET 10.0 SDK
- Docker & Docker Compose

### Quick Run with Docker
To run the entire platform (including PostgreSQL database and application), run the following command in the project root:

```bash
docker-compose up --build
```

### Manual Run (Development)
1. Set the connection string in `appsettings.json`
2. Run database migration commands:
```bash
dotnet ef database update --project dr24CoreNet.Infrastructure --startup-project dr24CoreNet.WebUI
```
3. Run the application:
```bash
dotnet run --project dr24CoreNet.WebUI
```

---

## Operational Preview Screenshots

This section shows a view of the system in operational mode with seeded data and the Vazirmatn font. All static assets, including Tailwind CSS and fonts, are loaded locally.

### 1. Main Platform View and Doctor Search (English)
![Main Platform View](screenshots/home_en.png)

### 2. Booking Process and Concurrency Lock Countdown
![Booking and Concurrency](screenshots/booking_countdown.png)

### 3. Financial Analytics Dashboard
![Financial Analytics](screenshots/enterprise_analytics.png)

### 4. Security Audit Trail and Health Record
![Audit Trail](screenshots/medical_audit_trail.png)

---

## Local Dependencies (No CDN)
All static files including CSS, JS, and Vazirmatn fonts are located locally in the `wwwroot` folder to ensure the platform functions correctly in internal networks and intranets without dependence on the global internet.
