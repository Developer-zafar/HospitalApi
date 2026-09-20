# 🏥 SmartHealthCare Management System Web API (.NET 8 & C#)

An enterprise-grade Healthcare Management System built with **ASP.NET Core Web API** and **Entity Framework Core**, featuring a **Decoupled Repository Pattern**, multi-role relational modeling, **Dynamic Healthcare JWT Claims**, and **Advanced Policy-Based Authorization**.

---

## 🚀 Key Architectural Highlights

- **Decoupled Repository Pattern:** Complete abstraction of Entity Framework Core from API Controllers via `IDoctorRepository`, `IPatientRepository`, and `IAppointmentRepository`, ensuring testability and separation of concerns.
- **Relational Domain Design & Cascade Cycle Resolution:** Designed a multi-level relational junction (`Doctor ➔ Appointment ➔ Patient`). Solved SQL Server's classic **Error 1785 (Multiple Cascade Paths Conflict)** using EF Core Fluent API with `DeleteBehavior.Restrict`.
- **Eager Loading & DTO Flattening:** High-performance multi-level eager loading with `.Include()` and safe anonymous/DTO projections to eliminate circular reference loops.
- **Dynamic Healthcare JWT Claims:** Extended standard JWT tokens to dynamically carry domain-specific claims (`DoctorId` and `PatientId`) alongside identity and role claims.
- **Advanced Policy-Based Authorization:** Moved beyond basic RBAC to multi-condition policies in `Program.cs` (`DoctorOnly` requiring both `Role == "Doctor"` and verified `DoctorId` claim).
- **Verified Security Matrix (Postman):**
  - **No Token:** `401 Unauthorized`
  - **Patient Token on Doctor Endpoint:** `403 Forbidden`
  - **Doctor Token:** `204 No Content` / `200 OK`

---

## 🛠️ Tech Stack & Tools

- **Framework:** ASP.NET Core 8 Web API
- **Language:** C#
- **ORM:** Entity Framework Core 8
- **Database:** Microsoft SQL Server (LocalDB)
- **Architecture:** Repository Pattern, DTO Pattern, Dependency Injection (Scoped)
- **Security:** JWT (JSON Web Tokens), Cryptographic PBKDF2 Password Hashing
- **Testing:** Postman, Swagger / OpenAPI

---

## ⚙️ Getting Started

### 1. Clone the repository
\`\`\`bash
git clone https://github.com/your-username/SmartHealthCare.git
cd SmartHealthCare
\`\`\`

### 2. Configure Connection String
Update `ConnectionStrings:DefaultConnection` in `appsettings.json`:
\`\`\`json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=SmartHealthCare_Db;Trusted_Connection=True;TrustServerCertificate=True"
}
\`\`\`

### 3. Apply Migrations & Update Database
\`\`\`bash
dotnet ef database update
\`\`\`

### 4. Run the Application
\`\`\`bash
dotnet run
\`\`\`

---

## 👤 Author
**M. Zafar Iqbal**  
*Software Engineer | ASP.NET Core Developer*