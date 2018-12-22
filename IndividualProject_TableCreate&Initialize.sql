CREATE TABLE LoginCredentials
(
	username VARCHAR(20) NOT NULL PRIMARY KEY, 
	passphrase VARCHAR(20) NOT NULL
);

CREATE TABLE CurrentLoginCredentials
(
	username VARCHAR(20) NOT NULL, 
	passphrase VARCHAR(20) NOT NULL,
	currentStatus VARCHAR(10) NOT NULL
);

CREATE TABLE UserLevelAccess
(
	username VARCHAR(20) NOT NULL FOREIGN KEY REFERENCES LoginCredentials(username) ON DELETE CASCADE,
	userRole VARCHAR(20) NOT NULL
);

CREATE TABLE CustomerTickets
(
	ticketID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	dateCreated DATETIME,
	username VARCHAR(20) NOT NULL FOREIGN KEY REFERENCES LoginCredentials(username) ON DELETE CASCADE,
	userAssignedTo VARCHAR(20),
	ticketStatus VARCHAR(10) NOT NULL,
	comments VARCHAR(500)
);

CREATE TABLE DeletedCustomerTickets
(
	ticketID INT NOT NULL PRIMARY KEY,
	dateCreated DATETIME,
	username VARCHAR(20) NOT NULL FOREIGN KEY REFERENCES LoginCredentials(username),
	userAssignedTo VARCHAR(20),
	ticketStatus VARCHAR(10) NOT NULL,
	comments VARCHAR(500)
);

CREATE TRIGGER StoreDeletedTickets ON CustomerTickets
FOR DELETE
AS
BEGIN
	DECLARE @ticketID INT, @dateCreated DATETIME, @username VARCHAR(20), @userAssignedTo VARCHAR(20), @ticketStatus VARCHAR(10), @comments VARCHAR(500)
	SET @ticketID = (SELECT	ticketID FROM DELETED);
	SET @dateCreated = (SELECT dateCreated FROM DELETED);
	SET @username = (SELECT username FROM DELETED);
	SET @userAssignedTo = (SELECT userAssignedTo FROM DELETED);
	SET @ticketStatus = (SELECT ticketStatus FROM DELETED);
	SET @comments = (SELECT comments FROM DELETED);

	INSERT INTO DeletedCustomerTickets VALUES(@ticketID, @dateCreated, @username, @userAssignedTo, @ticketStatus, @comments)
END

CREATE PROCEDURE RemoveUsernameFromDatabase
@username VARCHAR(20)
AS
BEGIN
	DELETE FROM UserLevelAccess WHERE username = @username
	DELETE FROM LoginCredentials WHERE username = @username
	PRINT 'Username ' + CAST(@username AS VARCHAR(20)) + ' has been successfully removed from Database.'
END

INSERT INTO LoginCredentials VALUES ('admin', 'admin')
INSERT INTO CurrentLoginCredentials VALUES ('Not Registered', 'inactive')
INSERT INTO UserLevelAccess VALUES ('admin', 'super_admin')