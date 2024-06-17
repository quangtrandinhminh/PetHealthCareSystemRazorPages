USE [PetHealthCareSys]
GO

/* Insert Data to table Service */
DELETE Service
GO

INSERT INTO [dbo].[Service] (Name, Description, Duration, Price, CreatedBy, LastUpdatedBy, DeletedBy, CreatedTime, LastUpdatedTime, DeletedTime)
VALUES 
    (N'Kiểm soát bọ chét khi tắm (theo toa)', NULL, 30, 200000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Tư vấn/Đào tạo Hành vi', NULL, 60, 500000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Giấy chứng nhận sức khỏe (Bán hàng & Du lịch)', NULL, 45, 150000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Nội trú và Chăm sóc ban ngày', NULL, 1440, 3000000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Nhập viện', NULL, 1440, 5000000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Hoàn thành các bài kiểm tra đánh giá y tế', NULL, 60, 700000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Phòng thí nghiệm chẩn đoán trong nhà', NULL, 30, 1000000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Vi mạch da liễu', NULL, 30, 800000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Nhận biết', NULL, 15, 50000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Chăm sóc nha khoa', NULL, 60, 1200000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Tư vấn chế độ ăn uống', NULL, 30, 200000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Tiệm thuốc', NULL, 15, 100000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Siêu âm kỹ thuật số', NULL, 45, 1500000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'X-quang kỹ thuật số', NULL, 45, 1200000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Vắc-xin', NULL, 15, 300000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Nội soi sợi quang', NULL, 60, 1800000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Chương trình chăm sóc sức khỏe', NULL, 1440, 10000000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL);
GO

DELETE Cage
GO

/* Insert Data to table Cage */
INSERT INTO [dbo].[Cage] ([Capacity], [Material], [Room], [Address], [Description], [Note], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [IsAvailable])
VALUES 
    (4, 'Metal', 101, '123 Pet St', 'Large metal cage for dogs', NULL, 1, 1, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 1),
    (2, 'Plastic', 102, '123 Pet St', 'Small plastic cage for cats', NULL, 1, 1, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 1),
    (3, 'Wood', 103, '123 Pet St', 'Medium wooden cage for rabbits', NULL, 1, 1, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 1),
    (5, 'Metal', 104, '123 Pet St', 'Extra large metal cage for large dogs', NULL, 1, 1, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 1),
    (1, 'Plastic', 105, '123 Pet St', 'Small plastic cage for hamsters', NULL, 1, 1, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 1),
    (3, 'Metal', 106, '123 Pet St', 'Medium metal cage for birds', NULL, 1, 1, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 1),
    (4, 'Wood', 107, '123 Pet St', 'Large wooden cage for small animals', NULL, 1, 1, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 1),
    (2, 'Plastic', 108, '123 Pet St', 'Small plastic cage for reptiles', NULL, 1, 1, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 1)
GO

DELETE TimeTable
GO

/* Insert Data to table TimeTable */
-- Helper variables for times
DECLARE @StartTime TIME = '08:00:00';
DECLARE @EndTime TIME = '12:00:00';
DECLARE @Interval INT = 30; -- interval in minutes

-- Insert Book intervals for 08:00 to 12:00
WHILE @StartTime < @EndTime
BEGIN
    INSERT INTO [dbo].[TimeTable] ([Note], [CreatedTime], [LastUpdatedTime], [StartTime], [EndTime])
    VALUES ('Book', SYSDATETIMEOFFSET(),  SYSDATETIMEOFFSET(), @StartTime, DATEADD(MINUTE, @Interval, @StartTime));
    
    SET @StartTime = DATEADD(MINUTE, @Interval, @StartTime);
END

-- Reset the start and end times for the next interval
SET @StartTime = '13:00:00';
SET @EndTime = '17:00:00';

-- Insert Book intervals for 13:00 to 17:00
WHILE @StartTime < @EndTime
BEGIN
    INSERT INTO [dbo].[TimeTable] ([Note], [CreatedTime], [LastUpdatedTime], [StartTime], [EndTime])
    VALUES ('Book', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), @StartTime, DATEADD(MINUTE, @Interval, @StartTime));
    
    SET @StartTime = DATEADD(MINUTE, @Interval, @StartTime);
END

-- Insert Check intervals for 18:00 to 22:00
DECLARE @CheckIntervals TABLE (StartTime TIME, EndTime TIME)
INSERT INTO @CheckIntervals VALUES ('18:00:00', '19:00:00'), ('19:00:00', '20:00:00'), ('20:00:00', '21:00:00'), ('21:00:00', '22:00:00');

DECLARE @CheckStartTime TIME;
DECLARE @CheckEndTime TIME;

DECLARE CheckCursor CURSOR FOR
SELECT StartTime, EndTime FROM @CheckIntervals;

OPEN CheckCursor;
FETCH NEXT FROM CheckCursor INTO @CheckStartTime, @CheckEndTime;

WHILE @@FETCH_STATUS = 0
BEGIN
    INSERT INTO [dbo].[TimeTable] ([Note], [CreatedTime], [LastUpdatedTime], [StartTime], [EndTime])
    VALUES ('Check', SYSDATETIMEOFFSET(),  SYSDATETIMEOFFSET(), @CheckStartTime, @CheckEndTime);
    
    FETCH NEXT FROM CheckCursor INTO @CheckStartTime, @CheckEndTime;
END;

CLOSE CheckCursor;
DEALLOCATE CheckCursor;


DELETE MedicalItem
GO

/* Insert Data to table MedicalItem */
-- Thuốc cho chó
INSERT INTO [dbo].[MedicalItem] ([Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [CreatedTime], [LastUpdatedTime], [MedicalItemType], [Note])
VALUES 
(N'Bravecto Chews', N'Điều trị ve và bọ chét cho chó', 50, 100, 1, 1, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1, N'Dành cho chó trên 6 tháng tuổi'),
(N'Heartgard Plus', N'Phòng ngừa giun tim cho chó', 45, 150, 1, 1, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1, N'Dạng nhai hàng tháng'),
(N'Simparica', N'Điều trị ve và bọ chét cho chó', 55, 120, 1, 1, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1, N'Dạng nhai hàng tháng'),
(N'Capstar', N'Điều trị bọ chét cho chó và mèo', 35, 200, 1, 1, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1, N'Viên uống'),
(N'NexGard', N'Điều trị ve và bọ chét cho chó', 55, 130, 1, 1, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1, N'Dạng nhai hàng tháng'),
(N'Frontline Plus', N'Phòng ngừa ve và bọ chét cho chó', 50, 140, 1, 1, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1, N'Dung dịch bôi hàng tháng'),
(N'Interceptor Plus', N'Phòng ngừa giun tim cho chó', 45, 110, 1, 1, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1, N'Dạng nhai hàng tháng'),
(N'Proin', N'Điều trị tiểu không tự chủ ở chó', 40, 85, 1, 1, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1, N'Viên uống hàng ngày'),
(N'Vetoryl', N'Điều trị hội chứng Cushing ở chó', 75, 60, 1, 1, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1, N'Viên nang uống hàng ngày'),
(N'Deramaxx', N'Điều trị viêm khớp và đau sau phẫu thuật cho chó', 65, 70, 1, 1, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1, N'Viên nhai hàng ngày');

-- Vắc-xin cho chó
INSERT INTO [dbo].[MedicalItem] ([Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [CreatedTime], [LastUpdatedTime], [MedicalItemType], [Note])
VALUES 
(N'Rabies Vaccine', N'Vắc-xin phòng bệnh dại cho chó và mèo', 20, 300, 2, 2, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 0, N'Vắc-xin hàng năm'),
(N'Distemper Vaccine', N'Vắc-xin phòng bệnh sốt chó cho chó', 25, 250, 2, 2, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 0, N'Vắc-xin hàng năm'),
(N'Parvovirus Vaccine', N'Vắc-xin phòng bệnh parvo cho chó', 30, 200, 2, 2, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 0, N'Vắc-xin hàng năm'),
(N'Bordetella Vaccine', N'Vắc-xin phòng ho cũi chó cho chó', 35, 220, 2, 2, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 0, N'Vắc-xin hàng năm'),
(N'Leptospirosis Vaccine', N'Vắc-xin phòng bệnh leptospirosis cho chó', 40, 180, 2, 2, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 0, N'Vắc-xin hàng năm'),
(N'Lyme Disease Vaccine', N'Vắc-xin phòng bệnh Lyme cho chó', 50, 190, 2, 2, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 0, N'Vắc-xin hàng năm'),
(N'Canine Influenza Vaccine', N'Vắc-xin phòng bệnh cúm chó cho chó', 30, 210, 2, 2, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 0, N'Vắc-xin hàng năm'),
(N'Canine Hepatitis Vaccine', N'Vắc-xin phòng viêm gan ở chó', 30, 200, 2, 2, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 0, N'Vắc-xin hàng năm'),
(N'Parainfluenza Vaccine', N'Vắc-xin phòng viêm phổi do virus Parainfluenza ở chó', 25, 230, 2, 2, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 0, N'Vắc-xin hàng năm'),
(N'Coronavirus Vaccine', N'Vắc-xin phòng bệnh do virus Corona ở chó', 35, 240, 2, 2, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 0, N'Vắc-xin hàng năm');

-- Thuốc cho mèo
INSERT INTO [dbo].[MedicalItem] ([Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [CreatedTime], [LastUpdatedTime], [MedicalItemType], [Note])
VALUES 
(N'Revolution for Cats', N'Phòng ngừa ve, bọ chét và giun tim cho mèo', 60, 80, 1, 1, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1, N'Dung dịch bôi ngoài'),
(N'Capstar', N'Điều trị bọ chét cho chó và mèo', 35, 200, 1, 1, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1, N'Viên uống'),
(N'Advantage II', N'Phòng ngừa bọ chét cho mèo', 40, 90, 1, 1, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1, N'Dung dịch bôi hàng tháng'),
(N'Cheristin', N'Phòng ngừa bọ chét cho mèo', 30, 160, 1, 1, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1, N'Dung dịch bôi ngoài'),
(N'Comfortis', N'Điều trị bọ chét cho mèo', 60, 110, 1, 1, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1, N'Viên uống hàng tháng'),
(N'Drontal', N'Điều trị giun sán cho mèo', 20, 140, 1, 1, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1, N'Viên uống'),
(N'Cerenia', N'Điều trị nôn mửa ở mèo', 25, 100, 1, 1, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1, N'Viên uống hàng ngày'),
(N'Methimazole', N'Điều trị cường giáp ở mèo', 35, 70, 1, 1, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1, N'Viên uống hàng ngày'),
(N'Prednisolone', N'Điều trị viêm và dị ứng ở mèo', 30, 130, 1, 1, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1, N'Viên uống hằng ngày')

-- Vắc-xin cho mèo
INSERT INTO [dbo].[MedicalItem] ([Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [CreatedTime], [LastUpdatedTime], [MedicalItemType], [Note])
VALUES 
(N'Rabies Vaccine', N'Vắc-xin phòng bệnh dại cho chó và mèo', 20, 300, 2, 2, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 0, N'Vắc-xin hàng năm'),
(N'FVRCP Vaccine', N'Vắc-xin phòng bệnh viêm mũi khí quản, calicivirus và bệnh giảm bạch cầu cho mèo', 25, 270, 2, 2, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 0, N'Vắc-xin hàng năm'),
(N'FeLV Vaccine', N'Vắc-xin phòng bệnh bạch cầu ở mèo', 35, 230, 2, 2, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 0, N'Vắc-xin hàng năm'),
(N'FIP Vaccine', N'Vắc-xin phòng bệnh viêm phúc mạc truyền nhiễm ở mèo', 45, 150, 2, 2, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 0, N'Vắc-xin hàng năm'),
(N'Bordetella Vaccine for Cats', N'Vắc-xin phòng bệnh ho cũi mèo', 30, 180, 2, 2, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 0, N'Vắc-xin hàng năm'),
(N'Chlamydia Vaccine', N'Vắc-xin phòng bệnh chlamydia cho mèo', 35, 190, 2, 2, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 0, N'Vắc-xin hàng năm'),
(N'Panleukopenia Vaccine', N'Vắc-xin phòng bệnh giảm bạch cầu ở mèo', 25, 240, 2, 2, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 0, N'Vắc-xin hàng năm'),
(N'Calicivirus Vaccine', N'Vắc-xin phòng bệnh calicivirus ở mèo', 30, 210, 2, 2, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 0, N'Vắc-xin hàng năm'),
(N'Rhinotracheitis Vaccine', N'Vắc-xin phòng bệnh viêm mũi khí quản ở mèo', 30, 220, 2, 2, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 0, N'Vắc-xin hàng năm'),
(N'Pneumonitis Vaccine', N'Vắc-xin phòng bệnh viêm phổi ở mèo', 35, 160, 2, 2, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 0, N'Vắc-xin hàng năm');
GO

-- Insert sample data into Transaction table
INSERT INTO [Transaction] (CustomerId, Total, Status, CreatedTime, LastUpdatedTime)
VALUES 
    (1, 1000.00, 1, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET()),
    (2, 1500.00, 2, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET());

-- Insert sample data into TransactionDetail table
INSERT INTO TransactionDetail (TransactionId, ServiceId, Quantity, SubTotal)
VALUES 
    (1, 1, 2, 400.00),
    (2, 2, 1, 500.00);

INSERT INTO TransactionDetail (TransactionId, MedicalItemId, Quantity, SubTotal)
VALUES 
    (1, 1, 5, 100.00),
    (2, 2, 3, 90.00);

