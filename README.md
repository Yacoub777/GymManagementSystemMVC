<div align="center">

<img src="https://readme-typing-svg.demolab.com?font=Fira+Code&weight=700&size=32&pause=1000&color=FF6B6B&center=true&vCenter=true&width=700&lines=🏋️+Gym+Management+System;Built+with+ASP.NET+Core+MVC;Clean+Architecture+%7C+EF+Core+%7C+SQL+Server" alt="Typing SVG" />

<br/>

![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core_MVC-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![EF Core](https://img.shields.io/badge/EF_Core-7B2FBE?style=for-the-badge&logo=dotnet&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-FF6B9D?style=for-the-badge&logo=bootstrap&logoColor=white)

<br/>

> 🏆 A complete, scalable web-based gym management platform built with **ASP.NET Core MVC** and **Clean 3-Layer Architecture**

</div>

---

## 🌟 Overview

The **Gym Management System** centralizes all core gym operations in one powerful platform:

| 💪 | Feature |
|:---:|:---|
| 👥 | Manage **Members**, **Trainers**, **Plans** & **Health Records** |
| 🗓️ | Schedule & manage **Training Sessions** |
| 🎟️ | Book sessions for members with capacity control |
| 📊 | Track memberships, attendance & analytics |
| 🖥️ | Admin dashboard with full operational insights |

---

## ✨ Core Features

<details>
<summary>👥 <b>Member Management</b></summary>
<br/>

- ✅ Full CRUD operations
- 📧 Unique email & phone validation
- 📅 Automatic join date assignment
- 🏥 Health record required on creation
- 📱 Egyptian phone number validation
- 🔒 Cannot delete member with active bookings

</details>

<details>
<summary>🧑‍🏫 <b>Trainer Management</b></summary>
<br/>

- ✅ Full CRUD operations
- 📧 Unique email & phone validation
- 📅 Automatic hire date assignment
- 🎯 Must have at least one specialty
- 🔒 Cannot delete trainer with future sessions

</details>

<details>
<summary>🗓️ <b>Session Management</b></summary>
<br/>

- ✅ Full CRUD operations
- 👥 Capacity range: **1 – 25 members**
- 📅 EndDate must be after StartDate
- 🔗 Requires valid trainer & category
- 🔒 Cannot delete future sessions
- 🏃 Members can attend multiple sessions

</details>

<details>
<summary>🧾 <b>Plans & Memberships</b></summary>
<br/>

- 🔄 Activate / deactivate plans
- ⏱️ Duration: **1 – 365 days**
- 🔒 Cannot modify plans with active memberships
- 📅 Membership EndDate auto-calculated
- ❌ No overlapping active memberships per member

</details>

<details>
<summary>🎟️ <b>Booking Management</b></summary>
<br/>

- ✅ Only active members can book sessions
- 🪑 Session must have available capacity
- ❌ Prevent duplicate bookings
- 🚫 Only future bookings can be cancelled
- 📋 Attendance tracked for ongoing sessions

</details>

---

## 🏗️ Architecture

```
┌─────────────────────────────────────────────┐
│           🖥️  Presentation Layer             │
│     ASP.NET MVC Controllers + Razor Views    │
│              (Bootstrap UI)                  │
└─────────────────┬───────────────────────────┘
                  │
┌─────────────────▼───────────────────────────┐
│          ⚙️  Business Logic Layer            │
│   TrainerService · SessionService · etc.     │
└─────────────────┬───────────────────────────┘
                  │
┌─────────────────▼───────────────────────────┐
│           🗄️  Data Access Layer              │
│     Repository Pattern + Unit of Work        │
│              (EF Core DbContext)             │
└─────────────────────────────────────────────┘
```

---

## 🧩 Entity Relationships

```
Member ──────── HealthRecord (1:1)
  │
  ├──── Booking ──── Session ──── Trainer
  │                      │
  └──── Membership ── Plan    └── Category
```

| Entity | Description |
|:---|:---|
| 👤 **Member** | Basic info + photo · HealthRecord · Plan · Sessions via Bookings |
| 🏋️ **Trainer** | Personal info + specialties · conducts many Sessions |
| 📋 **Plan** | Name, price, duration · can be activated/deactivated |
| 🗓️ **Session** | Trainer + Category · capacity & schedule · Members via Booking |
| 🏥 **HealthRecord** | Height, weight, blood type · belongs to one Member |
| 🎟️ **Booking** | Junction table: Members ↔ Sessions |
| 🔗 **Membership** | Junction table: Members ↔ Plans |

---

## 📁 Controllers Summary

| Controller | Actions |
|:---|:---|
| 🧑 **MemberController** | Index · Create · Edit · Delete · MemberDetails · HealthRecordDetails |
| 🏋️ **TrainerController** | Index · Create · Edit · Delete · Details |
| 🗓️ **SessionController** | Index · Create · Edit · Delete · Details |
| 📋 **PlanController** | Index · Details · Edit · Activate |

---

## 🛢️ Database Highlights

- 📱 **Egyptian phone validation** regex enforced at DB level
- 📅 **Auto-calculated dates** — JoinDate, HireDate, BookingDate
- 🗑️ **Soft Delete** supported for Plans
- 🌱 **Seeded Categories** — Cardio · Strength · Yoga · and more

---

## 🚀 Tech Stack

<div align="center">

| Layer | Technology |
|:---|:---|
| 🖥️ Frontend | ASP.NET Core MVC · Razor Views · Bootstrap · Custom CSS |
| ⚙️ Backend | ASP.NET Core · C# · AutoMapper · Repository Pattern · Unit of Work |
| 🗄️ Database | SQL Server · Entity Framework Core |

</div>

---

## 🔮 Future Enhancements

- [ ] 🔐 Authentication & Authorization (ASP.NET Identity)
- [ ] 💳 Payment Integration
- [ ] 🔔 Notifications System
- [ ] 📷 QR Code Attendance Tracking
- [ ] 📈 Advanced Analytics Dashboard

---

<div align="center">

**Built with ❤️ by [Mostafa Yacoub](https://github.com/Yacoub777)**

![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=flat-square&logo=csharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white)
![EF Core](https://img.shields.io/badge/EF_Core-7B2FBE?style=flat-square&logo=dotnet&logoColor=white)

</div>
