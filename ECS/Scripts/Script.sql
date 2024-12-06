
CREATE TABLE [Role] (
  [RoleId] int IDENTITY(1,1) PRIMARY KEY,
  [RoleName] nvarchar(100),
  [BaseSalary] decimal(15,2)
)
GO

CREATE TABLE [Service] (
  [ServiceId] int IDENTITY(1,1) PRIMARY KEY,
  [ServiceName] nvarchar(255)
)
GO
CREATE TABLE [Departments] (
  [DepartmentID] int IDENTITY(1,1) PRIMARY KEY,
  [DepartmentName] nvarchar(255),
  [ManagerId] UNIQUEIDENTIFIER
)
GO

CREATE TABLE [Employee] (
  [EmployeeId] UNIQUEIDENTIFIER PRIMARY KEY,
  [FirstName] nvarchar(255),
  [LastName] nvarchar(255),
  [Email] nvarchar(255),
  [PhoneNumber] nvarchar(15),
  [Password] NVARCHAR(max),
  [RoleId] int,
  [DepartmentID] int,
  [CreateAt] datetime
  FOREIGN KEY (DepartmentId) REFERENCES [Departments]([DepartmentID])
)
GO



CREATE TABLE [AttendanceRecords] (
  [AttendanceId] int IDENTITY(1,1) PRIMARY KEY,
  [EmployeeId] UNIQUEIDENTIFIER,
  [AttendanceDate] date,
  [AttendanceStatus] nvarchar(100)
)
GO

CREATE TABLE [Client] (
  [ClientId] UNIQUEIDENTIFIER PRIMARY KEY,
  [ClientName] nvarchar(255),
  [ContactPerson] nvarchar(255),
  [Email] nvarchar(255),
  [PhoneNumber] nvarchar(15),
  [Address] nvarchar(max),
  [Password] NVARCHAR(max),
  [CreateAt] datetime
)
GO

CREATE TABLE [EmployeeCategory] (
  [EmployeeCategoryId] int IDENTITY(1,1) PRIMARY KEY,
  [EmployeeId] UNIQUEIDENTIFIER,
  [CategoryId] int
)
GO

CREATE TABLE [ProductCategory] (
  [CategoryId] int IDENTITY(1,1) PRIMARY KEY,
  [CategoryName] nvarchar(255)
)
GO

CREATE TABLE [Product] (
  [ProductId] UNIQUEIDENTIFIER PRIMARY KEY,
  [ClientId] UNIQUEIDENTIFIER,
  [CategoryId] int,
  [ProductName] nvarchar(max),
  [Discreption] nvarchar(max),
  [CrateAt] datetime
)
GO

CREATE TABLE [ProductService] (
  [ProductService] int IDENTITY(1,1) PRIMARY KEY,
  [ServiceId] int,
  [ProductId] UNIQUEIDENTIFIER,
  [StartDate] datetime,
  [EndDate] datetime
)
GO

CREATE TABLE [CallHistory] (
  [CallId] int IDENTITY(1,1) PRIMARY KEY,
  [EmployeeId] UNIQUEIDENTIFIER,
  [CallDatetime] datetime,
  [CallStatus] nvarchar(100),
  [Notes] text DEFAULT NULL
)
GO

CREATE TABLE [Order] (
  [OrderId] int IDENTITY(1,1) PRIMARY KEY,
  [CallId] int,
  [Orderer] nvarchar(255),
  [OrderDate] datetime,
  [TotalAmount] decimal(15,2),
  [Recipient_Name] nvarchar(255),
  [Recipient_Phone] nvarchar(15),
  [Recipient_Address] nvarchar(max),
  [OrderStatus] nvarchar(100)
)
GO

CREATE TABLE [OrderDetail] (
  [OrderDetailId] int IDENTITY(1,1) PRIMARY KEY,
  [OrderId] int,
  [ProductId] UNIQUEIDENTIFIER,
  [Quantity] int,
  [TotalPrice] decimal(15,2)
)
GO

ALTER TABLE [Employee] ADD FOREIGN KEY ([RoleId]) REFERENCES [Role] ([RoleId])
GO

ALTER TABLE [Departments] ADD FOREIGN KEY ([ManagerId]) REFERENCES [Employee] ([EmployeeId])
GO

ALTER TABLE [AttendanceRecords] ADD FOREIGN KEY ([EmployeeId]) REFERENCES [Employee] ([EmployeeId])
GO

ALTER TABLE [EmployeeCategory] ADD FOREIGN KEY ([EmployeeId]) REFERENCES [Employee] ([EmployeeId])
GO

ALTER TABLE [EmployeeCategory] ADD FOREIGN KEY ([CategoryId]) REFERENCES [ProductCategory] ([CategoryId])
GO

ALTER TABLE [Product] ADD FOREIGN KEY ([CategoryId]) REFERENCES [ProductCategory] ([CategoryId])
GO

ALTER TABLE [Product] ADD FOREIGN KEY ([ClientId]) REFERENCES [Client] ([ClientId])
GO

ALTER TABLE [ProductService] ADD FOREIGN KEY ([ProductId]) REFERENCES [Product] ([ProductId])
GO

ALTER TABLE [ProductService] ADD FOREIGN KEY ([ServiceId]) REFERENCES [Service] ([ServiceId])
GO

ALTER TABLE [CallHistory] ADD FOREIGN KEY ([EmployeeId]) REFERENCES [Employee] ([EmployeeId])
GO

ALTER TABLE [Order] ADD FOREIGN KEY ([CallId]) REFERENCES [CallHistory] ([CallId])
GO

ALTER TABLE [OrderDetail] ADD FOREIGN KEY ([OrderId]) REFERENCES [Order] ([OrderId])
GO

ALTER TABLE [OrderDetail] ADD FOREIGN KEY ([ProductId]) REFERENCES [Product] ([ProductId])
GO
ALTER TABLE Product
ADD UnitPrice DECIMAL(15, 2) NOT NULL DEFAULT 0; -- Giá bán sản phẩm

ALTER TABLE Product
ADD InitialQuantity INT NOT NULL DEFAULT 0; -- Số lượng sản phẩm ban đầu

-- Phiếu lương
CREATE TABLE PaySlip (
    PaySlipId INT PRIMARY KEY IDENTITY(1,1),     -- Tự động tăng ID phiếu lương
    EmployeeId UNIQUEIDENTIFIER NOT NULL,                     -- ID nhân viên
    PeriodStartDate DATE NOT NULL,               -- Ngày bắt đầu kỳ kinh doanh
    PeriodEndDate DATE NOT NULL,                 -- Ngày kết thúc kỳ kinh doanh
    BaseSalary DECIMAL(15, 2) NOT NULL,          -- Lương cơ bản
    TotalRevenue DECIMAL(15, 2) NOT NULL,        -- Tổng doanh thu trong kỳ
    Bonus DECIMAL(15, 2) NOT NULL,               -- Thưởng doanh thu (1% doanh thu)
    TotalSalary DECIMAL(15, 2) NOT NULL,         -- Tổng lương (lương cơ bản + thưởng)
    CONSTRAINT FK_PaySlipEmployee FOREIGN KEY (EmployeeId) REFERENCES Employee(EmployeeId)
);
GO
ALTER TABLE Product
ADD [IsActive] BIT DEFAULT 0;
GO
ALTER TABLE Product
ADD [Status] NVARCHAR(30) DEFAULT 'pending' ;
GO

GO
CREATE PROCEDURE [dbo].[AddDepartments]
    @DepartmentName NVARCHAR(255),
    @ManagerId UNIQUEIDENTIFIER = NULL
AS
BEGIN
    INSERT INTO [dbo].[Departments] ([DepartmentName], [ManagerId])
    VALUES (@DepartmentName, @ManagerId)
END
GO
CREATE PROCEDURE [dbo].[DeleteDepartments]
    @DepartmentId INT
AS
BEGIN
    DELETE FROM [dbo].[Departments]
    WHERE [DepartmentID] = @DepartmentId
END
GO
CREATE PROCEDURE [dbo].[UpdateDepartments]
    @DepartmentId INT,
    @DepartmentName NVARCHAR(255),
    @ManagerId UNIQUEIDENTIFIER = NULL
AS
BEGIN
    UPDATE [dbo].[Departments]
    SET [DepartmentName] = @DepartmentName, 
        [ManagerId] = @ManagerId
    WHERE [DepartmentID] = @DepartmentId
END
GO
CREATE PROCEDURE [dbo].[GetAllDepartments]
AS
BEGIN
    SELECT * FROM Departments;
END
GO
CREATE PROCEDURE [dbo].[GetDepartmentsById]
@DepartmentId INT
AS
BEGIN
    SELECT * FROM Departments WHERE DepartmentID = @DepartmentId;
END
GO
CREATE PROCEDURE AddRole
    @RoleName NVARCHAR(100),
    @BaseSalary DECIMAL(15, 2)
AS
BEGIN
    INSERT INTO [Role] (RoleName, BaseSalary)
    VALUES (@RoleName, @BaseSalary);
END;
GO
CREATE PROCEDURE DeleteRole
    @RoleId INT
AS
BEGIN
    DELETE FROM [Role]
    WHERE RoleId = @RoleId;
END;
GO
CREATE PROCEDURE GetAllRole
AS
BEGIN
    SELECT * FROM [Role];
END;
GO
CREATE PROCEDURE UpdateRole
    @RoleId INT,
    @RoleName NVARCHAR(100),
    @BaseSalary DECIMAL(15, 2)
AS
BEGIN
    UPDATE [Role]
    SET RoleName = @RoleName,
        BaseSalary = @BaseSalary
    WHERE RoleId = @RoleId;
END;
GO
CREATE PROCEDURE GetRoleById
@RoleId INT
AS
BEGIN
  SELECT * FROM Role WHERE RoleId = @RoleId;
END
GO
CREATE PROCEDURE DeleteEmployee
    @EmployeeId UNIQUEIDENTIFIER
AS
BEGIN
    DELETE FROM [Employee]
    WHERE EmployeeId = @EmployeeId;
END;
GO
CREATE PROCEDURE GetEmployeeByEmail
    @Email NVARCHAR(255)
AS
BEGIN
    SELECT *
    FROM [Employee]
    WHERE Email = @Email;
END;
GO
CREATE PROCEDURE RegisterEmployee
    @FirstName NVARCHAR(255),
    @LastName NVARCHAR(255),
    @Email NVARCHAR(255),
    @PhoneNumber NVARCHAR(15),
    @Password NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO [Employee] (EmployeeId, FirstName, LastName, Email, PhoneNumber, Password,RoleId, CreateAt)
    VALUES (NEWID(), @FirstName, @LastName, @Email, @PhoneNumber, @Password, 3, GETDATE());
END;
GO
CREATE PROCEDURE UpdateEmployee
    @EmployeeId UNIQUEIDENTIFIER,
    @FirstName NVARCHAR(255),
    @LastName NVARCHAR(255),
    @Email NVARCHAR(255),
    @PhoneNumber NVARCHAR(15)
AS
BEGIN
    UPDATE [Employee]
    SET FirstName = @FirstName,
        LastName = @LastName,
        Email = @Email,
        PhoneNumber = @PhoneNumber
    WHERE EmployeeId = @EmployeeId;
END;
GO
CREATE PROCEDURE UpdateEmployeeRole
    @EmployeeId UNIQUEIDENTIFIER,
    @RoleId INT
AS
BEGIN
    UPDATE [Employee]
    SET RoleId = @RoleId
    WHERE EmployeeId = @EmployeeId;
END;
GO
CREATE PROCEDURE UpdateDepartmentForEmployee
@EmployeeId UNIQUEIDENTIFIER,
@DepartmentId int 
AS
BEGIN
  UPDATE [Employee]
  SET DepartmentID = @DepartmentId
  WHERE EmployeeId = @EmployeeId;
END
GO
CREATE PROCEDURE SetManagerForDepartment
@DepartmentId INT,
@ManagerId UNIQUEIDENTIFIER
AS
BEGIN
  UPDATE [Departments]
  SET ManagerId = @ManagerId
  WHERE DepartmentID = @DepartmentId;
END
GO