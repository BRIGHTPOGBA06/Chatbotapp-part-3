# CyberShield Chatbot (Part 3)

## Project Overview
**CyberShield Chatbot** is a desktop application developed in **.NET 8 (WPF)** with a **MySQL** backend. It combines natural language processing (NLP) for cybersecurity guidance, an interactive quiz module, and a full CRUD task assistant to help users manage security checklists and operational tasks.

---

## Key Features
* **WPF Graphical User Interface:** A modern, dark-themed desktop layout featuring custom ASCII branding and smooth user interaction.
* **NLP Chat Engine:** Real-time conversational assistant capable of personalizing responses with user names and answering cybersecurity topics (e.g., firewalls, phishing, and passwords).
* **Database Integration (MySQL):** Full CRUD (Create, Read, Update, Delete) functionality linking the WPF user interface to a locally hosted MySQL database (`CyberShieldDB`).
* **Cybersecurity Quiz Module:** Interactive mini-game testing users' security knowledge with immediate feedback.
* **Audio Integration:** Built-in audio playback using native `.NET` features to welcome users upon startup.

---

## Tech Stack
* **Framework:** .NET 8 (WPF / Windows Presentation Foundation)
* **Language:** C#
* **Database:** MySQL Server / MySQL Workbench
* **IDE:** Visual Studio 2022

---

## Setup & Installation Instructions

### 1. Database Configuration
1. Open **MySQL Workbench**.
2. Run the provided database setup script located in the repository under `ChatBotDB/CyberShieldDB.sql` (or execute the script below):
   ```sql
   CREATE DATABASE IF NOT EXISTS CyberShieldDB;
   USE CyberShieldDB;

   CREATE TABLE IF NOT EXISTS Tasks (
       Id INT AUTO_INCREMENT PRIMARY KEY,
       Title VARCHAR(255) NOT NULL,
       Description TEXT,
       ReminderDate DATETIME,
       IsCompleted BOOLEAN DEFAULT FALSE
   );
