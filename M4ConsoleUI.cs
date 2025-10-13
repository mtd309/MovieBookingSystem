// ========================================
// Module 4 – Console UI
// Phụ trách: Huỳnh Yến Trân
// Mục đích: Quản lý tất cả phần hiển thị và tương tác qua giao diện console.
// Các hàm: DrawMainMenu, DrawUserMenu, DrawAdminMenu, DisplayMovieInfo, DisplayShowtimesTable, DisplaySeatingChart, RenderSeatingMap, PromptAndPause
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieBookingSystem
{
    public static class ConsoleUI
    {
        // =============================
        // 1. VẼ MENU CẤP CAO
        // =============================
        public static void DrawMainMenu()
        {
            Console.WriteLine("\n=== HỆ THỐNG ĐẶT VÉ XEM PHIM ===");
            Console.WriteLine("\n1. Đăng nhập");
            Console.WriteLine("2. Đăng ký");
            Console.WriteLine("3. Thoát");
            Console.Write("Chọn: ");
        }

        public static void DrawUserMenu()
        {
            Console.WriteLine("\n=== MENU NGƯỜI DÙNG ===");
            Console.WriteLine("1. Xem phim & lịch chiếu");
            Console.WriteLine("2. Tìm phim theo tên");
            Console.WriteLine("3. Đặt vé");
            Console.WriteLine("4. Hủy vé");
            Console.WriteLine("5. Đăng xuất");
            Console.Write("Chọn: ");
        }

        public static void DrawAdminMenu()
        {
            Console.WriteLine("\n=== MENU QUẢN TRỊ ===");
            Console.WriteLine("1. Thêm phim");
            Console.WriteLine("2. Sửa phim");
            Console.WriteLine("3. Xóa phim");
            Console.WriteLine("4. Sắp xếp phim (theo tên)");
            Console.WriteLine("5. Tìm phim theo ID (Binary Search)");
            Console.WriteLine("6. Xem tất cả phim");
            Console.WriteLine("7. Báo cáo doanh thu");
            Console.WriteLine("8. Đăng xuất");
            Console.Write("Chọn: ");
        }

        // =============================
        // 2. HIỂN THỊ DANH SÁCH
        // =============================

        // Hàm hiển thị danh sách phim
        public static void DisplayMovieInfo(List<Movie> movies)
        {
            Console.WriteLine("\n=== DANH SÁCH PHIM ===");
            if (movies.Count == 0)
            {
                Console.WriteLine("Không có phim nào trong hệ thống.");
                return;
            }
            foreach (var movie in movies)
            {
                Console.WriteLine($"{movie.Id}. {movie.Title} - {movie.Genre} - Giá: {movie.TicketPrice:N0} VNĐ");
            }
        }

        // Hàm hiển thị suất chiếu chi tiết
        public static void DisplayShowtimesTable(DataStorage data)
        {
            Console.WriteLine("\n=== DANH SÁCH SUẤT CHIẾU ===");
            if (data.Showtimes.Count == 0)
            {
                Console.WriteLine("Chưa có suất chiếu nào.");
                return;
            }

            Console.WriteLine("ID | Phim           | Bắt đầu  | Phòng | Ghế trống");
            Console.WriteLine("---|----------------|----------|--------|-----------");

            foreach (var show in data.Showtimes)
            {
                var movie = data.Movies.Find(m => m.Id == show.RoomNumber);
                string movieTitle = movie?.Title ?? "N/A";

                int available = 0;
                int total = show.Seating.GetLength(0) * show.Seating.GetLength(1);
                foreach (var seat in show.Seating)
                    if (seat == SeatStatus.Available) available++;

                Console.WriteLine($"{show.Id,-2} | {movieTitle,-14} | {show.StartTime:HH:mm} | {show.RoomNumber,-6} | {available}/{total}");
            }
        }

        // =============================
        // 3. HIỂN THỊ SƠ ĐỒ GHẾ
        // =============================
        public static void DisplaySeatingChart(Showtime? showtime = null)
        {
            if (showtime == null || showtime.Seating == null)
            {
                Console.WriteLine("Không có sơ đồ ghế để hiển thị.");
                return;
            }

            RenderSeatingMap(showtime.Seating);
        }

        private static void RenderSeatingMap(SeatStatus[,] seats)
        {
            int originalRows = seats.GetLength(0); // Số hàng thực tế (A, B, C...)
            int originalCols = seats.GetLength(1); // Số cột thực tế (1, 2, 3...)

            // --- Hiển thị màn hình ---
            Console.WriteLine("\n Sơ đồ ghế (O: trống, X: đã đặt, R: giữ chỗ)");
            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine("                  MÀN HÌNH PHÍA TRƯỚC"); // Thêm khoảng trắng để căn giữa
            Console.WriteLine("---------------------------------------------------------");

            // 1. In tiêu đề cột (A, B, C...)
            Console.Write("    "); // 3 khoảng trắng để căn chỉnh với số hàng
            for (int c = 0; c < originalRows; c++)
            {
                char colChar = (char)('A' + c);
                Console.Write($"{colChar} ");
            }
            Console.WriteLine();

            // 2. In từng hàng (1, 2, 3...)
            for (int i = 0; i < originalCols; i++) // i là index cột cũ (0..4), giờ là row index mới (1..5)
            {
                // In nhãn hàng (số) - PadLeft(2) để căn chỉnh
                Console.Write($"{(i + 1).ToString().PadLeft(2)} ");

                for (int j = 0; j < originalRows; j++) // j là index hàng cũ (0..4), giờ là col index mới (A..E)
                {
                    // Truy cập TỌA ĐỘ NGƯỢC (TRANSPOSED): seats[j, i]
                    // j: Lấy ký tự cột (A, B, C)
                    // i: Lấy ký tự hàng (1, 2, 3)
                    char seatChar = seats[j, i] switch
                    {
                        SeatStatus.Available => 'O',
                        SeatStatus.Booked => 'X',
                        SeatStatus.Reserved => 'R',
                        _ => '?'
                    };
                    Console.Write($"{seatChar} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("---------------------------------------------------------");
        }

        // =============================
        // 4. TIỆN ÍCH
        // =============================
        public static void PromptAndPause()
        {
            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey(true);
        }
    }
}
