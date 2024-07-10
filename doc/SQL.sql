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
    (3, 'Metal', 103, '123 Pet St', 'Medium metal cage for rabbits', NULL, 1, 1, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 1),
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

-- Insert Appointment intervals for 08:00 to 12:00
WHILE @StartTime < @EndTime
BEGIN
    INSERT INTO [dbo].[TimeTable] ([Type], [CreatedTime], [LastUpdatedTime], [StartTime], [EndTime])
    VALUES (1, SYSDATETIMEOFFSET(),  SYSDATETIMEOFFSET(), @StartTime, DATEADD(MINUTE, @Interval, @StartTime));
    
    SET @StartTime = DATEADD(MINUTE, @Interval, @StartTime);
END

-- Reset the start and end times for the next interval
SET @StartTime = '13:00:00';
SET @EndTime = '17:00:00';

-- Insert Appointment intervals for 13:00 to 17:00
WHILE @StartTime < @EndTime
BEGIN
    INSERT INTO [dbo].[TimeTable] ([Type], [CreatedTime], [LastUpdatedTime], [StartTime], [EndTime])
    VALUES (1, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), @StartTime, DATEADD(MINUTE, @Interval, @StartTime));
    
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
    INSERT INTO [dbo].[TimeTable] ([Type], [CreatedTime], [LastUpdatedTime], [StartTime], [EndTime])
    VALUES (2, SYSDATETIMEOFFSET(),  SYSDATETIMEOFFSET(), @CheckStartTime, @CheckEndTime);
    
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
INSERT INTO [dbo].[Transaction] ([CustomerId], Total, Status, CreatedTime, LastUpdatedTime)
VALUES 
    (1, 650000, 1, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET()),
    (1, 635000, 2, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET());

-- Insert sample data into TransactionDetail table
INSERT INTO TransactionDetails (TransactionId, ServiceId, [Name], [Price], Quantity, SubTotal)
VALUES 
    (1, 1, N'Kiểm soát bọ chét khi tắm (theo toa)', 200000, 2, 400000),
    (2, 2, N'Tư vấn/Đào tạo Hành vi', 500000, 1, 500000);

INSERT INTO TransactionDetails (TransactionId, MedicalItemId, [Name], [Price], Quantity, SubTotal)
VALUES 
    (1, 1, N'Bravecto Chews',50000, 5, 250000),
    (2, 2, N'Heartgard Plus',45000, 3, 135000);

INSERT INTO Configurations (ConfigKey, Value, CreatedTime, LastUpdatedTime)
VALUES 
    (N'HospitalizationPrice', '100000', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET()),
    (N'RefundPercentage', '0.7', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET());

-- INSERT INTO Users (FullName, UserName, Email, EmailConfirmed, PasswordHash, NormalizedUserName, ConcurrencyStamp, CreatedTime, LastUpdatedTime, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
-- VALUES
-- 	(N'System Admin', N'petsystemadmin', N'PetSystemAdmin@email.com', 0, N'$2a$11$wrSjvVjywF3E9WLIfzTLveB5AvKxNHfIWN3QZLWlg6Dw6LybZ65fW', N'PETSYSADMIN', N'bc87b618-69da-46d1-900b-713028dac9f9', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'0122334455', 0, 0, 0, 1),
-- 	(N'System Staff', N'systemstaff', N'staff@email.com', 0, N'$2a$11$wrSjvVjywF3E9WLIfzTLveB5AvKxNHfIWN3QZLWlg6Dw6LybZ65fW', N'SYSTEMSTAFF', N'bc87b618-69da-46d1-900b-713028dac9f9', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'0122334465', 0, 0, 0, 1),
-- 	(N'Dr. Duy Anh', N'duyanhvet', N'duyanhvet@email.com', 0, N'$2a$11$wrSjvVjywF3E9WLIfzTLveB5AvKxNHfIWN3QZLWlg6Dw6LybZ65fW', N'DUYANHVET', N'bc87b618-69da-46d1-900b-713028dac9f9', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'0122334466', 0, 0, 0, 1),
-- 	(N'Dr. Phúc', N'phucvet', N'phucvet@email.com', 0, N'$2a$11$wrSjvVjywF3E9WLIfzTLveB5AvKxNHfIWN3QZLWlg6Dw6LybZ65fW', N'PHUCVET', N'bc87b618-69da-46d1-900b-713028dac9f9', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'0122334467', 0, 0, 0, 1),
-- 	(N'Dr. Luân', N'luanvet', N'luanvet@email.com', 0, N'$2a$11$wrSjvVjywF3E9WLIfzTLveB5AvKxNHfIWN3QZLWlg6Dw6LybZ65fW', N'LUANVET', N'bc87b618-69da-46d1-900b-713028dac9f9', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'0122334468', 0, 0, 0, 1),
-- 	(N'Dr. Phong', N'phongvet', N'phongvet@email.com', 0, N'$2a$11$wrSjvVjywF3E9WLIfzTLveB5AvKxNHfIWN3QZLWlg6Dw6LybZ65fW', N'PHONGVET', N'bc87b618-69da-46d1-900b-713028dac9f9', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'0122334469', 0, 0, 0, 1),
-- 	(N'Dr. Quang', N'quangvet', N'quangvet@email.com', 0, N'$2a$11$wrSjvVjywF3E9WLIfzTLveB5AvKxNHfIWN3QZLWlg6Dw6LybZ65fW', N'QUANGVET', N'bc87b618-69da-46d1-900b-713028dac9f9', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'0122334470', 0, 0, 0, 1),
-- 	(N'Dr. Nam', N'namvet', N'namvet@email.com', 0, N'$2a$11$wrSjvVjywF3E9WLIfzTLveB5AvKxNHfIWN3QZLWlg6Dw6LybZ65fW', N'NAMVET', N'bc87b618-69da-46d1-900b-713028dac9f9', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'0122334471', 0, 0, 0, 1),
-- 	(N'Duy Anh', N'duyanh', N'duyanh@email.com', 0, N'$2a$11$wrSjvVjywF3E9WLIfzTLveB5AvKxNHfIWN3QZLWlg6Dw6LybZ65fW', N'DUYANH', N'bc87b618-69da-46d1-900b-713028dac9f9', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'0122334472', 0, 0, 0, 1),
-- 	(N'Việt Phúc', N'vietphuc', N'vietphuc@email.com', 0, N'$2a$11$wrSjvVjywF3E9WLIfzTLveB5AvKxNHfIWN3QZLWlg6Dw6LybZ65fW', N'VIETPHUC', N'bc87b618-69da-46d1-900b-713028dac9f9', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'0122334473', 0, 0, 0, 1),
-- 	(N'Trọng Luân', N'trongluan', N'trongluan@email.com', 0, N'$2a$11$wrSjvVjywF3E9WLIfzTLveB5AvKxNHfIWN3QZLWlg6Dw6LybZ65fW', N'TRONGLUAN', N'bc87b618-69da-46d1-900b-713028dac9f9', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'0122334474', 0, 0, 0, 1),
-- 	(N'Thanh Phong', N'thanhphong', N'thanhphong@email.com', 0, N'$2a$11$wrSjvVjywF3E9WLIfzTLveB5AvKxNHfIWN3QZLWlg6Dw6LybZ65fW', N'THANHPHONG', N'bc87b618-69da-46d1-900b-713028dac9f9', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'0122334475', 0, 0, 0, 1),
-- 	(N'Minh Quang', N'minhquang', N'minhquang@email.com', 0, N'$2a$11$wrSjvVjywF3E9WLIfzTLveB5AvKxNHfIWN3QZLWlg6Dw6LybZ65fW', N'MINHQUANG', N'bc87b618-69da-46d1-900b-713028dac9f9', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'0122334476', 0, 0, 0, 1),
-- 	(N'Nhật Nam', N'nhatnam', N'nhatnam@email.com', 0, N'$2a$11$wrSjvVjywF3E9WLIfzTLveB5AvKxNHfIWN3QZLWlg6Dw6LybZ65fW', N'NHATNAM', N'bc87b618-69da-46d1-900b-713028dac9f9', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'0122334477', 0, 0, 0, 1);
-- GO

-- INSERT INTO UserRoles (UserId, RoleId, Discriminator)
-- VALUES
-- 	(7, 1, N'UserRoleEntity'),
-- 	(8, 2, N'UserRoleEntity'),
-- 	(9, 3, N'UserRoleEntity'),
-- 	(10, 3, N'UserRoleEntity'),
-- 	(11, 3, N'UserRoleEntity'),
-- 	(12, 3, N'UserRoleEntity'),
-- 	(13, 3, N'UserRoleEntity'),
-- 	(14, 3, N'UserRoleEntity'),
-- 	(15, 4, N'UserRoleEntity'),
-- 	(16, 4, N'UserRoleEntity'),
-- 	(17, 4, N'UserRoleEntity'),
-- 	(18, 4, N'UserRoleEntity'),
-- 	(19, 4, N'UserRoleEntity'),
-- 	(20, 4, N'UserRoleEntity');
-- GO

-- INSERT INTO Pet (Name, Species, Breed, Gender, DateOfBirth, OwnerID, CreatedTime, LastUpdatedTime, IsNeutered)
-- VALUES
-- 	(N'Duy Anh', N'Chó', N'Chó cỏ', N'Đực', SYSDATETIMEOFFSET(), 15, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Minh Quang', N'Chó', N'Corgy', N'Đực', SYSDATETIMEOFFSET(), 15, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Thanh Phong', N'Chó', N'Husky', N'Đực', SYSDATETIMEOFFSET(), 15, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Việt Phúc', N'Mèo', N'Vàng lắm lông', N'Đực', SYSDATETIMEOFFSET(), 15, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Trọng Luân', N'Mèo', N'Tabi', N'Đực', SYSDATETIMEOFFSET(), 15, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Nam', N'Mèo', N'Mun', N'Đực', SYSDATETIMEOFFSET(), 15, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),

-- 	(N'Duy Anh', N'Chó', N'Chó cỏ', N'Đực', SYSDATETIMEOFFSET(), 16, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Minh Quang', N'Chó', N'Corgy', N'Đực', SYSDATETIMEOFFSET(), 16, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Thanh Phong', N'Chó', N'Husky', N'Đực', SYSDATETIMEOFFSET(), 16, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Việt Phúc', N'Mèo', N'Vàng lắm lông', N'Đực', SYSDATETIMEOFFSET(), 16, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Trọng Luân', N'Mèo', N'Tabi', N'Đực', SYSDATETIMEOFFSET(), 16, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Nam', N'Mèo', N'Mun', N'Đực', SYSDATETIMEOFFSET(), 16, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),

-- 	(N'Duy Anh', N'Chó', N'Chó cỏ', N'Đực', SYSDATETIMEOFFSET(), 17, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Minh Quang', N'Chó', N'Corgy', N'Đực', SYSDATETIMEOFFSET(), 17, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Thanh Phong', N'Chó', N'Husky', N'Đực', SYSDATETIMEOFFSET(), 17, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Việt Phúc', N'Mèo', N'Vàng lắm lông', N'Đực', SYSDATETIMEOFFSET(), 17, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Trọng Luân', N'Mèo', N'Tabi', N'Đực', SYSDATETIMEOFFSET(), 17, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Nam', N'Mèo', N'Mun', N'Đực', SYSDATETIMEOFFSET(), 17, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),

-- 	(N'Duy Anh', N'Chó', N'Chó cỏ', N'Đực', SYSDATETIMEOFFSET(), 18, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Minh Quang', N'Chó', N'Corgy', N'Đực', SYSDATETIMEOFFSET(), 18, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Thanh Phong', N'Chó', N'Husky', N'Đực', SYSDATETIMEOFFSET(), 18, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Việt Phúc', N'Mèo', N'Vàng lắm lông', N'Đực', SYSDATETIMEOFFSET(), 18, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Trọng Luân', N'Mèo', N'Tabi', N'Đực', SYSDATETIMEOFFSET(), 18, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Nam', N'Mèo', N'Chó cỏ', N'Đực', SYSDATETIMEOFFSET(), 18, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),

-- 	(N'Duy Anh', N'Chó', N'Chó cỏ', N'Đực', SYSDATETIMEOFFSET(), 19, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Minh Quang', N'Chó', N'Corgy', N'Đực', SYSDATETIMEOFFSET(), 19, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Thanh Phong', N'Chó', N'Husky', N'Đực', SYSDATETIMEOFFSET(), 19, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Việt Phúc', N'Mèo', N'Vàng lắm lông', N'Đực', SYSDATETIMEOFFSET(), 19, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Trọng Luân', N'Mèo', N'Tabi', N'Đực', SYSDATETIMEOFFSET(), 19, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Nam', N'Mèo', N'Mun', N'Đực', SYSDATETIMEOFFSET(), 19, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),

-- 	(N'Duy Anh', N'Chó', N'Chó cỏ', N'Đực', SYSDATETIMEOFFSET(), 20, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Minh Quang', N'Chó', N'Corgy', N'Đực', SYSDATETIMEOFFSET(), 20, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Thanh Phong', N'Chó', N'Husky', N'Đực', SYSDATETIMEOFFSET(), 20, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Việt Phúc', N'Mèo', N'Vàng lắm lông', N'Đực', SYSDATETIMEOFFSET(), 20, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Trọng Luân', N'Mèo', N'Tabi', N'Đực', SYSDATETIMEOFFSET(), 20, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1),
-- 	(N'Nam', N'Mèo', N'Mun', N'Đực', SYSDATETIMEOFFSET(), 20, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 1);
-- GO