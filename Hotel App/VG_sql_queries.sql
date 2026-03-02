--1) JOIN: Lista bokningar med kund + rum
SELECT 
    b.Id AS BookingId,
    c.FirstName,
    c.LastName,
    r.RoomNumber,
    b.StartDate,
    b.EndDate,
    b.Status
FROM Bookings b
JOIN Customers c ON b.CustomerId = c.Id
JOIN Rooms r ON b.RoomId = r.Id
ORDER BY b.CreatedAt DESC;

-- 2) JOIN: Lista fakturor med bokning + kund
SELECT
    i.Id AS InvoiceId,
    i.TotalAmount,
    i.IssuedAt,
    i.PaidAt,
    b.Id AS BookingId,
    c.FirstName,
    c.LastName
FROM Invoices i
JOIN Bookings b ON i.BookingId = b.Id
JOIN Customers c ON b.CustomerId = c.Id
ORDER BY i.IssuedAt DESC;

-- 3) GROUP BY: Hur många bokningar per rum
SELECT
    r.RoomNumber,
    COUNT(*) AS NumberOfBookings
FROM Bookings b
JOIN Rooms r ON b.RoomId = r.Id
GROUP BY r.RoomNumber
ORDER BY NumberOfBookings DESC;

-- 4) GROUP BY: Total fakturerat per kund
SELECT
    c.FirstName,
    c.LastName,
    SUM(i.TotalAmount) AS TotalInvoiced
FROM Invoices i
JOIN Bookings b ON i.BookingId = b.Id
JOIN Customers c ON b.CustomerId = c.Id
GROUP BY c.FirstName, c.LastName
ORDER BY TotalInvoiced DESC;

-- 5) SUBQUERY: Visa alla kunder som har minst 1 bokning
SELECT *
FROM Customers
WHERE Id IN (
    SELECT CustomerId
    FROM Bookings
);