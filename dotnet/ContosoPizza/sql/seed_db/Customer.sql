DECLARE @j INT = 1;

-- Temp tables for names
DECLARE @FirstNames TABLE (Name NVARCHAR(50));
DECLARE @LastNames TABLE (Name NVARCHAR(50));

-- Populate name sets
INSERT INTO @FirstNames VALUES ('Rushikesh'), ('Anushka'), ('Pravin'), ('Shubham'), ('Aaditi'), ('Khushi'), ('Sumit'), ('Priyanka'), ('Shelly'), ('Ketan');
INSERT INTO @LastNames VALUES ('Shelar'), ('Matey'), ('Rajnale'), ('Jankar'), ('Mali'), ('Makwana'), ('Chahuhan'), ('Rao'), ('Jha'), ('Chavan');

WHILE @j <= 100
BEGIN
    DECLARE @FirstName NVARCHAR(50) = (SELECT TOP 1 Name FROM @FirstNames ORDER BY NEWID());
    DECLARE @LastName NVARCHAR(50) = (SELECT TOP 1 Name FROM @LastNames ORDER BY NEWID());
    DECLARE @FullName NVARCHAR(100) = @FirstName + ' ' + @LastName;
    DECLARE @Email NVARCHAR(100) = LOWER(REPLACE(@FirstName + '.' + @LastName + CAST(@j AS NVARCHAR), ' ', '')) + '@example.com';
    DECLARE @Phone NVARCHAR(100) = CONCAT('9', FORMAT(ABS(CHECKSUM(NEWID())) % 1000000000, '000000000'));
    DECLARE @Address NVARCHAR(255) = CONCAT('House No ', @j, ', Some Street, City ', @j);

    INSERT INTO Customers (Name, Email, PhoneNumber, Address)
    VALUES (@FullName, @Email, @Phone, @Address);

    SET @j = @j + 1;
END;
