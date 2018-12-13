CREATE TABLE LoginCredentials
(
	username VARCHAR(20) NOT NULL, 
	passphrase VARCHAR(20) NOT NULL
);

INSERT INTO LoginCredentials (username, passphrase)
VALUES
('admin', 'admin');

CREATE VIEW vShowLoginCredentials
AS
SELECT username, passphrase
FROM LoginCredentials

SELECT * FROM vShowLoginCredentials

CREATE PROCEDURE AddNewUserRequest