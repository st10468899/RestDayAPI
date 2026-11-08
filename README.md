# RaceDay API & Web Application

A full-stack ASP.NET Core MVC and Web API solution designed for managing race registrations, user authentication, and event management.

##  Project Overview

The **RaceDay** system is a web application built as part of the Portfolio of Evidence (POE) submission. It features an ASP.NET Core Web API backend linked with an MVC user interface, utilizing Entity Framework Core for data management and dynamic view components.

## 🛠️ Tech Stack & Prerequisites

- **Framework:** .NET 8.0 (ASP.NET Core MVC & Web API)
- **Database:** SQL Server / Entity Framework Core
- **Documentation & Testing:** Swagger UI, xUnit (RaceDayAPI.Tests)
- **Containerization:** Docker
- **IDE:** Visual Studio 2022

##  Repository Structure

```text
RestDayAPI/
├── RaceDay.web/         # ASP.NET Core MVC Frontend Application
├── RestDayAPI/          # ASP.NET Core Web API Backend Service
├── RaceDayAPI.Tests/    # Unit Testing Suite
├── Dockerfile           # Multi-stage Docker container build definition
└── RestDayAPI.sln       # Visual Studio Solution File
