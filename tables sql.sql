CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL,
    Email NVARCHAR(200) NOT NULL,
    PasswordHash NVARCHAR(256) NOT NULL,  -- Для хранения зашифрованного пароля
    Role NVARCHAR(50) NOT NULL,  -- Администратор, Пользователь и т.д.
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);
CREATE TABLE Projects (
    ProjectId INT PRIMARY KEY IDENTITY(1,1),
    ProjectName NVARCHAR(200) NOT NULL,
    Description NVARCHAR(1000),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);
CREATE TABLE Tasks (
    TaskId INT PRIMARY KEY IDENTITY(1,1),
    ProjectId INT FOREIGN KEY REFERENCES Projects(ProjectId),
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    AssignedUserId INT FOREIGN KEY REFERENCES Users(UserId),  -- Ответственный пользователь
    CreatedUserId INT FOREIGN KEY REFERENCES Users(UserId),   -- Автор задачи
    Status NVARCHAR(50) NOT NULL,  -- Статусы задач (Открыто, В работе, Завершено)
    Priority NVARCHAR(50),         -- Приоритет задачи (Низкий, Средний, Высокий)
    DueDate DATETIME,              -- Срок выполнения
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);
CREATE TABLE TaskStatuses (
    StatusId INT PRIMARY KEY IDENTITY(1,1),
    StatusName NVARCHAR(50) NOT NULL  -- Например: "Открыто", "В работе", "Завершено"
);
CREATE TABLE Attachments (
    AttachmentId INT PRIMARY KEY IDENTITY(1,1),
    TaskId INT FOREIGN KEY REFERENCES Tasks(TaskId),
    FilePath NVARCHAR(500) NOT NULL,  -- Путь к файлу на диске
    FileName NVARCHAR(255) NOT NULL,  -- Название файла
    UploadedAt DATETIME DEFAULT GETDATE()
);
CREATE TABLE Comments (
    CommentId INT PRIMARY KEY IDENTITY(1,1),
    TaskId INT FOREIGN KEY REFERENCES Tasks(TaskId),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    Content NVARCHAR(MAX) NOT NULL,  -- Текст комментария
    CreatedAt DATETIME DEFAULT GETDATE()
);
CREATE TABLE TaskHistory (
    HistoryId INT PRIMARY KEY IDENTITY(1,1),
    TaskId INT FOREIGN KEY REFERENCES Tasks(TaskId),
    ChangedByUserId INT FOREIGN KEY REFERENCES Users(UserId),
    OldStatus NVARCHAR(50),
    NewStatus NVARCHAR(50),
    ChangeDate DATETIME DEFAULT GETDATE(),
    Description NVARCHAR(1000)  -- Описание изменений
);
