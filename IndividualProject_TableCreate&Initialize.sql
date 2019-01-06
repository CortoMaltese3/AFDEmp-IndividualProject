--Table/procedure/trigger creation. Followed up by initializing values 

CREATE TABLE UserCredentials
(
	username VARCHAR(20) NOT NULL PRIMARY KEY, 
	passphrase VARCHAR(20) NOT NULL,
	userRole VARCHAR(20) NOT NULL,
	currentStatus VARCHAR(10) NOT NULL,
	lastLoginDateTime DATETIME
);

CREATE TABLE CustomerTickets
(
	ticketID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	dateCreated DATETIME,
	username VARCHAR(20),
	userAssignedTo VARCHAR(20),
	ticketStatus VARCHAR(10) NOT NULL,
	comments VARCHAR(500)
);

CREATE TABLE DeletedCustomerTickets
(
	ticketID INT NOT NULL PRIMARY KEY,
	dateCreated DATETIME,
	username VARCHAR(20) NOT NULL,
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

---------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE RemoveUsernameFromDatabase
@username VARCHAR(20)
AS
BEGIN
	DELETE FROM UserCredentials WHERE username = @username
	PRINT 'Username ' + CAST(@username AS VARCHAR(20)) + ' has been successfully removed from Database.'
END

---------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE InsertNewUserIntoDatabase @username VARCHAR(20), @passphrase VARCHAR(20), @userRole VARCHAR(20)
AS
BEGIN
	INSERT INTO UserCredentials VALUES (@username, @passphrase, @userRole, 'inactive', NULL)
END

---------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE SelectCurrentUserFromDatabase
AS
BEGIN
	SELECT TOP 1 username FROM UserCredentials ORDER BY lastLoginDateTime DESC
END

---------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE SelectCurrentUserRoleFromDatabase
AS
BEGIN
	SELECT userRole FROM (SELECT TOP 1 * FROM UserCredentials ORDER BY lastLoginDateTime DESC) currentUser
END

---------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE SelectCurrentUserStatusFromDatabase
AS
BEGIN
	SELECT currentStatus FROM (SELECT TOP 1 * FROM UserCredentials ORDER BY lastLoginDateTime DESC) currentUser
END

---------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE SetCurrentUserStatusToInactive
AS
BEGIN
	UPDATE currentUser
	SET currentStatus = 'inactive'
	FROM  (SELECT TOP 1 currentStatus FROM UserCredentials ORDER BY lastLoginDateTime DESC) currentUser
END

---------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE SetCurrentUserStatusToActive @username VARCHAR(20)
AS
BEGIN
	UPDATE UserCredentials
	SET currentStatus = 'active', lastLoginDateTime = GETDATE()
	WHERE username = @username
END

---------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE ViewAvailableUsersInDatabase
AS
BEGIN
	SELECT * from UserCredentials EXCEPT (SELECT * FROM UserCredentials WHERE username = 'admin')
END

---------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE CheckUniqueCredentials @usernameCheck VARCHAR(20), @passphraseCheck VARCHAR(20)
AS
BEGIN
	SELECT COUNT(*) FROM UserCredentials
	WHERE username = @usernameCheck AND passphrase = @passphraseCheck
END	

---------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE CheckUniqueUsername @usernameCheck VARCHAR(20)
AS
BEGIN
	SELECT COUNT(*) FROM UserCredentials
	WHERE username = @usernameCheck
END	

---------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE OpenNewTechnicalTicket 
				 @username VARCHAR(20), @userAssignedTo VARCHAR(20), @comments VARCHAR(500) 
AS
BEGIN
	INSERT INTO CustomerTickets VALUES (GETDATE(), @username, @userAssignedTo, 'open', @comments)
END	

---------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE fetchNewTicketID 
AS
BEGIN
	SELECT TOP 1 ticketID FROM CustomerTickets ORDER BY ticketID DESC
END	

---------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE SetTicketStatusToClosed @ticketID INT
AS
BEGIN
	UPDATE CustomerTickets SET ticketStatus = 'closed' WHERE ticketID = @ticketID
END	

---------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE DeleteCustomerTicket @ticketID INT
AS
BEGIN
	DELETE FROM CustomerTickets WHERE ticketID = @ticketID
END	

---------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE SelectOpenCustomerTickets
AS
BEGIN
	SELECT * FROM CustomerTickets WHERE ticketStatus = 'open'
END	

---------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE SelectSingleCustomerTicket @ticketID INT
AS
BEGIN
	SELECT * FROM CustomerTickets WHERE ticketID = @ticketID
END	

---------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE EditCustomerTicketCommentSection @ticketComment VARCHAR (500), @ID INT
AS
BEGIN
	UPDATE CustomerTickets SET comments = @ticketComment WHERE ticketID = @ID
END	

---------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE ChangeUserAssignedTo @username VARCHAR (500), @ID INT
AS
BEGIN
	UPDATE CustomerTickets SET userAssignedTo = @username WHERE ticketID = @ID
END	

---------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE SelectTicketIDWithOpenStatus
AS
BEGIN
	SELECT ticketID FROM CustomerTickets WHERE ticketStatus = 'open'
END	

---------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE SelectUsersAndRolesInDatabase
AS
BEGIN
	SELECT username, userRole FROM UserCredentials
END	

---------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE SelectSingleUserRole @username VARCHAR(20)
AS
BEGIN
	SELECT userRole FROM UserCredentials WHERE username = @username
END	

---------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE UpdateUserRole @username VARCHAR(20), @userRole VARCHAR(20)
AS
BEGIN
	UPDATE UserCredentials SET userRole = @userRole WHERE username = @username
END	

---------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE CountOpenTicketsAssignedToUser @userAssignedTo VARCHAR(20)
AS
BEGIN
	SELECT COUNT(*) FROM CustomerTickets
	WHERE userAssignedTo = @userAssignedTo AND ticketStatus = 'Open'
END	

---------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE SelectOpenTicketsAssignedToUser 
@userAssignedTo VARCHAR(20)
AS
BEGIN
	DECLARE @Temp TABLE 
	(
		ticketID INT,
		dateCreated DATETIME,
		username VARCHAR(20) NOT NULL,
		userAssignedTo VARCHAR(20),
		ticketStatus VARCHAR(10) NOT NULL,
		comments VARCHAR(500)
	)

	INSERT INTO @Temp EXECUTE SelectOpenCustomerTickets

	SELECT * FROM @Temp 
	WHERE userAssignedTo = @userAssignedTo
END

---------------------------------------------------------------------------------------------------------------------------------------------------------
--Test users/TTs. Only admin-admin needs to be initialized

EXECUTE InsertNewUserIntoDatabase 'admin', 'admin', 'super_admin'
EXECUTE InsertNewUserIntoDatabase 'agent', 'agent', 'User'
EXECUTE InsertNewUserIntoDatabase 'secondLevel', 'secondLevel', 'Moderator'
EXECUTE InsertNewUserIntoDatabase 'thirdLevel', 'thirdLevel', 'Administrator'
EXECUTE OpenNewTechnicalTicket 'agent', 'secondLevel', 'Customer complains about his internet connection being slow. Live port monitor reveals excessive accumulation of CRC/FEC errors. Procceed with Netword/Equipment check'
EXECUTE OpenNewTechnicalTicket 'secondLevel', 'thirdLevel', 'Modem/Router not synching. Communication with device failed. Proceed with Network check'
EXECUTE OpenNewTechnicalTicket 'agent', 'agent', 'Led Power indicator on UPS device, battery not charging. Equipment replacement is imminent'
EXECUTE OpenNewTechnicalTicket 'admin', 'giorgos', 'Led Power indicator on UPS device, battery not charging. Equipment replacement is imminent'
---------------------------------------------------------------------------------------------------------------------------------------------------------

select * from UserCredentials
select * from CustomerTickets
select * from DeletedCustomerTickets

DELETE FROM UserCredentials WHERE username = 'giorgos'