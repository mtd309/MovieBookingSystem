// ===========================================================
// Module 1 – Core Data Structures
// Phụ trách: Mai Thanh Ngân
// Mục đích: Định nghĩa toàn bộ cấu trúc dữ liệu cốt lõi của hệ thống đặt vé xem phim.
// Cac struct/class: Movie, Showtime, Booking, User, DataStorage
// Cac enum: SeatStatus, UserRole
// Các hằng: const DefaultPassword, readonly RoomNumber
// Biến: int, string, decimal
// Mảng 2D: SeatStatus[,] Seating – mô tả sơ đồ ghế trong mỗi suất chiếu
// Danh sách động:: List<T> – lưu các đối tượng Movies, Showtimes, Users, Bookings
// ===========================================================
using System;
using System.Collections.Generic;

namespace MovieBookingSystem
{
    // =================== ENUM ===================

    /// Trạng thái ghế trong phòng chiếu
    public enum SeatStatus
    {
        Available,  // Ghế trống
        Booked,     // Ghế đã đặt
        Reserved    // Ghế được giữ tạm thời
    }

    /// Vai trò của người dùng trong hệ thống
    public enum UserRole
    {
        Customer,   // Khách hàng
        Admin       // Quản trị viên
    }

    // =================== STRUCT ===================

    /// Cấu trúc đặt vé (Booking)
    public struct Booking
    {
        public int BookingId;       // Mã đặt vé
        public int ShowtimeId;      // Mã suất chiếu
        public int UserId;          // Mã người dùng
        public List<(int SeatRow, int SeatCol)> Seats; // Danh sách ghế đặt

        public Booking(int bookingId, int showtimeId, int userId)
        {
            BookingId = bookingId;
            ShowtimeId = showtimeId;
            UserId = userId;
            Seats = new List<(int, int)>();
        }
    }

    // =================== CLASS ===================
    /// Lớp phim (Movie)
    public class Movie
    {
        public int Id { get; set; }                  // Mã phim
        public string Title { get; set; }            // Tên phim
        public decimal TicketPrice { get; set; }     // Giá vé
        public string? Genre { get; set; }           // Thể loại (nullable)
    }

    /// <summary>
    /// Lớp suất chiếu (Showtime)
    /// </summary>
    public class Showtime
    {
        public int Id { get; set; }                      // Mã suất chiếu
        public readonly int RoomNumber;                  // Số phòng chiếu (readonly)
        public DateTime StartTime { get; set; }          // Thời gian bắt đầu
        public SeatStatus[,] Seating { get; set; }       // Ma trận ghế (2D array)

        public Showtime(int roomNumber, int rows, int cols)
        {
            RoomNumber = roomNumber;
            Seating = new SeatStatus[rows, cols];
        }
    }

    /// Lớp người dùng (User)
    public class User
    {
        public int Id { get; set; }                      // Mã người dùng
        public string Name { get; set; }                 // Tên người dùng
        public UserRole Role { get; set; }               // Vai trò (Admin, Customer)
    }

    /// <summary>
    /// Lớp lưu trữ dữ liệu (DataStorage)
    /// </summary>
    public class DataStorage
    {
        public List<Movie> Movies { get; set; } = new List<Movie>();           // Danh sách phim
        public List<Showtime> Showtimes { get; set; } = new List<Showtime>();  // Danh sách suất chiếu
        public List<Booking> Bookings { get; set; } = new List<Booking>();     // Danh sách đặt vé
        public List<User> Users { get; set; } = new List<User>();              // Danh sách người dùng
    }
}