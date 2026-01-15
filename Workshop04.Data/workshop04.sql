ALTER TABLE Customers
ADD ProfileImagePath NVARCHAR(200) NULL;



Add-Migration InitialBaseline
Update-Database

Add-Migration AddIdentityTables
Update-Database

Add-Migration FixCustEmailMapping
Update-Database

Add-Migration UpdateCustomerBookingRelations
Update-Database

ALTER TABLE AspNetUsers
DROP CONSTRAINT AK_AspNetUsers_CustomerId;

ALTER TABLE AspNetUsers
DROP COLUMN CustomerId;