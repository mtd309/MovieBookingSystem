// ===============================
// Module 6 – Admin Workflows
// Phụ trách: Nguyễn Hồng Hạnh 
// Mục đích: Quản lý toàn bộ thao tác của quản trị viên trên hệ thống.
// Các method: AddMovie, EditMovie, DeleteMovie, SortMovies, FindMovieById,ViewAllMovies, GenerateRevenueReport, Logout
// Cấu trúc điều khiển: if/else, for, switch, try/catch
// ===============================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; 

namespace MovieBookingSystem
{
    public class AdminWorkflow
    {
        // ==========================================================
        // Chức năng 1: Thêm phim mới
        // Tương tác: M1 (DataStorage, Movie), M4 (Console input/output)
        // ==========================================================
        public void AddMovie(DataStorage data)
        {
            Console.WriteLine("\n--- THÊM PHIM MỚI ---");
            Console.Write("Nhập tên phim: ");
            string title = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Lỗi: Tên phim không được để trống.");
                return;
            }

            Console.Write("Nhập thể loại: ");
            string genre = Console.ReadLine();

            Console.Write("Nhập giá vé: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price < 0)
            {
                Console.WriteLine("Lỗi: Giá vé không hợp lệ!");
                return;
            }

            // Tính toán ID phim mới để đảm bảo không trùng lặp
            int newMovieId = data.Movies.Any() ? data.Movies.Max(m => m.Id) + 1 : 1;

            var newMovie = new Movie
            {
                Id = newMovieId,
                Title = title,
                Genre = genre,
                TicketPrice = price
            };
            data.Movies.Add(newMovie);

            Console.WriteLine($"Thêm phim '{title}' (ID: {newMovieId}) thành công!");
        }

        // ==========================================================
        // Chức năng 2: Sửa thông tin phim
        // Tương tác: M1 (DataStorage), M4 (Console input/output)
        // ==========================================================
        public void EditMovie(DataStorage data)
        {
            Console.WriteLine("\n--- SỬA THÔNG TIN PHIM ---");
            Console.Write("Nhập ID phim cần sửa: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Lỗi: ID không hợp lệ!");
                return;
            }

            // Dùng FindIndex để có thể cập nhật trực tiếp object trong List
            int index = data.Movies.FindIndex(m => m.Id == id);
            if (index == -1)
            {
                Console.WriteLine($"Lỗi: Không tìm thấy phim có ID = {id}.");
                return;
            }

            Movie movieToEdit = data.Movies[index];

            Console.WriteLine($"Đang sửa phim: '{movieToEdit.Title}' (ID: {movieToEdit.Id})");
            Console.WriteLine("(Bỏ trống và nhấn Enter để giữ lại giá trị cũ)");

            // Cho phép người dùng bỏ trống để giữ lại giá trị cũ
            Console.Write($"Nhập tên mới (hiện tại: {movieToEdit.Title}): ");
            string newTitle = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newTitle))
            {
                movieToEdit.Title = newTitle;
            }

            Console.Write($"Nhập thể loại mới (hiện tại: {movieToEdit.Genre}): ");
            string newGenre = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newGenre))
            {
                movieToEdit.Genre = newGenre;
            }

            Console.Write($"Nhập giá mới (hiện tại: {movieToEdit.TicketPrice:N0}): VNĐ");
            string priceInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(priceInput))
            {
                if (decimal.TryParse(priceInput, out decimal newPrice) && newPrice >= 0)
                {
                    movieToEdit.TicketPrice = newPrice;
                }
                else
                {
                    Console.WriteLine("Giá vé nhập vào không hợp lệ, giữ lại giá cũ.");
                }
            }

            // Cập nhật lại phim trong danh sách
            data.Movies[index] = movieToEdit;

            Console.WriteLine("Cập nhật thông tin phim thành công!");
        }

        // ==========================================================
        // Chức năng 3: Xóa phim
        // Tương tác: M1 (DataStorage), M4 (Console input/output)
        // ==========================================================
        public void DeleteMovie(DataStorage data)
        {
            Console.WriteLine("\n--- XÓA PHIM ---");
            Console.Write("Nhập ID phim cần xóa: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Lỗi: ID không hợp lệ!");
                return;
            }

            // Tìm phim trước khi xóa để xác nhận
            var movieToDelete = data.Movies.FirstOrDefault(m => m.Id == id);
            if (movieToDelete == null)
            {
                Console.WriteLine($"Lỗi: Không tìm thấy phim có ID = {id}.");
                return;
            }

            // Xác nhận xóa
            Console.Write($"Bạn có chắc muốn xóa phim '{movieToDelete.Title}' và các suất chiếu liên quan? (y/n): ");
            if (Console.ReadKey().KeyChar.ToString().ToLower() != "y")
            {
                Console.WriteLine("\nĐã hủy thao tác xóa.");
                return;
            }
            Console.WriteLine(); // Xuống dòng sau khi nhấn phím

            // Xóa phim khỏi danh sách
            int moviesRemoved = data.Movies.RemoveAll(m => m.Id == id);

            // Giả định: Xóa luôn các suất chiếu liên quan đến phim này
            // Trong M1, RoomNumber được dùng để liên kết với MovieId
            int showsRemoved = data.Showtimes.RemoveAll(s => s.RoomNumber == id);

            if (moviesRemoved > 0)
            {
                Console.WriteLine($"Đã xóa thành công {moviesRemoved} phim và {showsRemoved} suất chiếu liên quan.");
            }
        }

        // ==========================================================
        // Chức năng 4: Sắp xếp phim theo tên
        // Tương tác: M1 (DataStorage), M3 (BusinessLogic), M4 (ConsoleUI)
        // ==========================================================
        public void SortMovies(DataStorage data, BusinessLogic logic)
        {
            Console.WriteLine("\n--- SẮP XẾP PHIM THEO TÊN (A-Z) ---");

            // Gọi hàm logic sắp xếp từ Module 3
            logic.SortMoviesByTitle(data.Movies);

            Console.WriteLine("Đã sắp xếp danh sách phim. Kết quả:");

            // Gọi hàm UI để hiển thị kết quả từ Module 4
            ConsoleUI.DisplayMovieInfo(data.Movies);
        }

        // ==========================================================
        // Chức năng 5: Tìm phim theo ID
        // Tương tác: M1 (DataStorage), M3 (BusinessLogic), M4 (Console output)
        // ==========================================================
        public void FindMovieById(DataStorage data, BusinessLogic logic)
        {
            Console.WriteLine("\n--- TÌM PHIM THEO ID (BINARY SEARCH) ---");
            Console.Write("Nhập ID phim cần tìm: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Lỗi: ID không hợp lệ!");
                return;
            }

            // Binary search yêu cầu danh sách phải được sắp xếp theo key tìm kiếm (ID)
            data.Movies.Sort((m1, m2) => m1.Id.CompareTo(m2.Id));

            // Gọi hàm logic tìm kiếm từ Module 3
            int index = logic.BinarySearchMovieById(data.Movies, id);

            if (index != -1)
            {
                Movie foundMovie = data.Movies[index];
                Console.WriteLine("Đã tìm thấy phim:");
                Console.WriteLine($"  ID         : {foundMovie.Id}");
                Console.WriteLine($"  Tên phim   : {foundMovie.Title}");
                Console.WriteLine($"  Thể loại   : {foundMovie.Genre}");
                Console.WriteLine($"  Giá vé     : {foundMovie.TicketPrice:N0} VNĐ");
            }
            else
            {
                Console.WriteLine($"Không tìm thấy phim nào có ID = {id}.");
            }
        }

        // ==========================================================
        // Chức năng 6: Xem tất cả phim
        // Tương tác: M1 (DataStorage), M4 (ConsoleUI)
        // ==========================================================
        public void ViewAllMovies(DataStorage data)
        {
            // Chỉ cần gọi hàm hiển thị từ Module 4
            ConsoleUI.DisplayMovieInfo(data.Movies);
        }

        // ==========================================================
        // Chức năng 7: Báo cáo doanh thu
        // Tương tác: M1 (DataStorage), M4 (Console output)
        // ==========================================================
        public void GenerateRevenueReport(DataStorage data)
        {
            Console.WriteLine("\n--- BÁO CÁO DOANH THU ---");

            if (!data.Bookings.Any())
            {
                Console.WriteLine("Chưa có giao dịch nào được ghi nhận.");
                return;
            }

            decimal totalRevenue = 0;
            var reportDetails = new List<string>
            {
                "BookingID,ShowtimeID,MovieTitle,Tickets,Price,SubTotal"
            };

            foreach (var booking in data.Bookings)
            {
                // Tìm suất chiếu từ booking
                var showtime = data.Showtimes.FirstOrDefault(s => s.Id == booking.ShowtimeId);
                if (showtime == null) continue;

                // Tìm phim từ suất chiếu
                var movie = data.Movies.FirstOrDefault(m => m.Id == showtime.RoomNumber);
                if (movie == null) continue;

                int ticketCount = booking.Seats.Count;
                decimal subTotal = ticketCount * movie.TicketPrice;
                totalRevenue += subTotal;

                reportDetails.Add($"{booking.BookingId},{showtime.Id},\"{movie.Title}\",{ticketCount},{movie.TicketPrice},{subTotal}");
            }

            Console.WriteLine($"Tổng số giao dịch: {data.Bookings.Count}");
            Console.WriteLine($"Tổng doanh thu: {totalRevenue:N0} VNĐ");

            // Ghi báo cáo ra file CSV
            try
            {
                string filePath = "RevenueReport.csv";
                File.WriteAllLines(filePath, reportDetails);
                Console.WriteLine($"Đã lưu báo cáo chi tiết vào file: {Path.GetFullPath(filePath)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi ghi file báo cáo: {ex.Message}");
            }
        }

        // ==========================================================
        // Chức năng 8: Đăng xuất
        // Chức năng này thường được xử lý ở vòng lặp chính (main loop)
        // bằng cách thoát khỏi menu admin.
        // ==========================================================
        public void Logout()
        {
            Console.WriteLine("\nĐã đăng xuất. Quay về menu chính...");
        }
    }
}
