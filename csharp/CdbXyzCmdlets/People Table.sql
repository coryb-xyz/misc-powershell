CREATE TABLE People (
	FirstName VARCHAR(50),
	LastName VARCHAR(50),
	EmailAddress VARCHAR(100)
)

INSERT INTO People (FirstName, LastName, EmailAddress) VALUES ('John', 'Doe', 'john.doe@gmail.com')
INSERT INTO People (FirstName, LastName, EmailAddress) VALUES ('Jane', 'Doe', 'janedoe@gmail.com')
INSERT INTO People (FirstName, LastName, EmailAddress) VALUES ('Bob', 'Smith', 'bsmith@yahoo.com')
INSERT INTO People (FirstName, LastName, EmailAddress) VALUES ('Ted', 'Smith', 'teds@hotmail.com')
GO

CREATE LOGIN testuser WITH PASSWORD = 'testpassword'
GO

CREATE USER testuser FOR LOGIN testuser WITH DEFAULT_SCHEMA = dbo
GO

GRANT SELECT TO testuser
GO
