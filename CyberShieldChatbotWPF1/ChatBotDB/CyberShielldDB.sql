CREATE DATABASE IF NOT EXISTS CyberShieldDB;

USE CyberShieldDB;

CREATE TABLE IF NOT EXISTS Tasks (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Description TEXT,
    ReminderDate DATETIME,
    IsCompleted BOOLEAN DEFAULT FALSE
);

INSERT INTO Tasks (Title, Description, ReminderDate, IsCompleted) 
VALUES 
('Review Firewall Configurations', 'Check incoming and outgoing rule sets on local network.', '2026-11-15 09:30:00', FALSE),
('Update Password Policies', 'Ensure all system users enforce strong multi-factor authentication.', '2026-11-16 14:00:00', FALSE),
('Scan System Logs', 'Inspect event logs for unauthorized access attempts.', '2026-11-17 11:15:00', TRUE);

SELECT * FROM Tasks;