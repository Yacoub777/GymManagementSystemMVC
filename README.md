# Gym Management System

A complete web-based management system built using **ASP.NET Core MVC**,
designed to streamline gym operations such as member management, trainer
scheduling, session booking, and membership plans.

------------------------------------------------------------------------

## 🚀 Overview

The Gym Management System centralizes all core gym activities:

-   Manage **Members**, **Trainers**, **Plans**, and **Health Records**
-   Schedule and manage **Training Sessions**
-   Book sessions for members
-   Track memberships and attendance
-   Provide admin dashboards for analytics

Technologies used: - **ASP.NET Core MVC** - **Entity Framework Core** -
**SQL Server** - **Bootstrap & Custom CSS** - **AutoMapper** -
**Repository Pattern & Unit of Work**

------------------------------------------------------------------------

## 📌 Core Features

### ✅ Member Management

-   Full CRUD operations\
-   Unique email & phone validation\
-   Automatic join date\
-   Health record required\
-   Egyptian phone validation\
-   Cannot delete member with active bookings

------------------------------------------------------------------------

### 🧑‍🏫 Trainer Management

-   Full CRUD operations\
-   Unique email & phone validation\
-   Automatic hire date\
-   Must have at least one specialty\
-   Cannot delete trainer with future sessions

------------------------------------------------------------------------

### 🗓️ Session Management

-   CRUD operations for sessions\
-   Capacity range: **1--25**\
-   EndDate must be after StartDate\
-   Requires valid trainer & category\
-   Cannot delete future sessions\
-   Members can attend sessions

------------------------------------------------------------------------

### 🧾 Plans & Memberships

-   Activate/deactivate plans\
-   Duration: **1--365 days**\
-   Cannot modify/deactivate plans with active memberships\
-   Membership EndDate auto-calculated\
-   Members cannot have overlapping active memberships

------------------------------------------------------------------------

### 🎟️ Booking Management

-   Only active members can book sessions\
-   Session must have available capacity\
-   Prevent booking same session twice\
-   Only future bookings can be cancelled\
-   Attendance tracked for ongoing sessions

------------------------------------------------------------------------

## 🧱 Architecture (Three-Layer)

### **1. Presentation Layer**

ASP.NET MVC Controllers + Razor Views (Bootstrap)

### **2. Business Logic Layer**

Service classes (TrainerService, SessionService, etc.)

### **3. Data Access Layer**

Repository Pattern wrapping EF Core DbContext

------------------------------------------------------------------------

## 🧩 Entities Overview

### **Member**

-   Basic info + photo\
-   One HealthRecord\
-   One Plan\
-   Many Sessions (Bookings)

### **Trainer**

-   Personal info + specialties\
-   Conducts many sessions

### **Plan**

-   Name, price, duration\
-   Can be activated/deactivated

### **Session**

-   Trainer + Category\
-   Capacity, schedule\
-   Members attend via Booking table

### **HealthRecord**

-   Height, weight, blood type\
-   Belongs to one Member

### **Booking**

-   Junction table (Members ↔ Sessions)

### **Membership**

-   Junction table (Members ↔ Plans)

------------------------------------------------------------------------

## 📁 MVC Component Summary

### **Controllers**

#### MemberController

-   Index, Create, Edit, Delete\
-   MemberDetails, HealthRecordDetails

#### TrainerController

-   Index, Create, Edit, Delete\
-   Details

#### SessionController

-   Index, Create, Edit, Delete\
-   Details

#### PlanController

-   Index, Details\
-   Edit, Activate

------------------------------------------------------------------------

## 🛢️ Database Notes

-   Egyptian phone validation regex\
-   Auto-calculated dates (JoinDate, HireDate, BookingDate)\
-   Soft Delete supported for Plans\
-   Seeded Categories (Cardio, Strength, Yoga...)

------------------------------------------------------------------------

## 📊 Dashboard (Optional)

-   Members statistics\
-   Active/expired memberships\
-   Upcoming sessions\
-   Trainer schedules

------------------------------------------------------------------------

## 🏁 Conclusion

This system provides a scalable, modular, and well-structured foundation
for managing all gym operations efficiently using ASP.NET Core MVC with
clean architecture and strong business rules.

------------------------------------------------------------------------

Feel free to extend it with: - Authentication/Authorization\
- Payment Integration\
- Notifications\
- Attendance Tracking with QR
