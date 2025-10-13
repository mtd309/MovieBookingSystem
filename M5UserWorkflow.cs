// ===============================
// Module 5 – User Workflows
// Phụ trách: Trần Thị Hoài
// Mục đích: Xử lý luồng thao tác cho người dùng (Customer) – đăng ký, đăng nhập, đặt vé, hủy vé.
// Các method: UserRegister, UserLogin, ShowMoviesAndShowtimes, SearchMovie, BookTicket, CancelBooking, Logout
// ===============================

using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieBookingSystem
{
    public class UserWorkflow
    {
        private BusinessLogic logic = new BusinessLogic();

        #region Authentication
        public void UserRegister(DataStorage data)
        {
            Console.Write("Nhập tên người dùng mới: ");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Tên không được để trống.");
                return;
            }

            int newId = data.Users.Any() ? data.Users.Max(u => u.Id) + 1 : 1;
            var user = new User
            {
                Id = newId,
                Name = name,
                Role = UserRole.Customer
            };

            data.Users.Add(user);
            Console.WriteLine($"Đăng ký thành công! ID của bạn là: {newId}. Vui lòng sử dụng ID này để đăng nhập.");
        }

        public User UserLogin(DataStorage data)
        {
            Console.Write("Nhập ID người dùng: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID không hợp lệ! Vui lòng nhập một số.");
                return null;
            }

            var user = data.Users.Find(u => u.Id == id);
            if (user == null)
            {
                Console.WriteLine("Không tìm thấy người dùng.");
                return null;
            }

            Console.WriteLine($"Chào mừng trở lại, {user.Name}!");
            return user;
        }
        #endregion

        // ==========================================================
        // CÁC TÍNH NĂNG CHÍNH SAU KHI ĐĂNG NHẬP
        // ==========================================================

        /// <summary>
        /// 1. Luồng xem danh sách phim và các suất chiếu hiện có.
        /// </summary>
        public void ShowMoviesAndShowtimes(DataStorage data)
        {
            Console.Clear();
            ConsoleUI.DisplayMovieInfo(data.Movies);
            ConsoleUI.DisplayShowtimesTable(data);
        }

        /// <summary>
        /// 2. Luồng tìm kiếm phim theo tên.
        /// </summary>
        public void SearchMovie(DataStorage data, BusinessLogic logic)
        {
            Console.Clear();
            Console.WriteLine("--- TÌM PHIM THEO TÊN ---");
            Console.Write("Nhập từ khóa tên phim: ");
            string keyword = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                Console.WriteLine("Bạn chưa nhập từ khóa.");
                return;
            }
            // Gọi hàm logic từ M3 để tìm kiếm
            List<Movie> foundMovies = logic.SearchMovieByName(data.Movies, keyword);

             if (foundMovies.Any())
            {
                Console.WriteLine($"Tìm thấy {foundMovies.Count} kết quả:");
                // Gọi hàm UI từ M4 để hiển thị
                ConsoleUI.DisplayMovieInfo(foundMovies);
            }
            else
            {
                Console.WriteLine("Không tìm thấy phim nào khớp với từ khóa.");
            }
        }

        /// <summary>3. Luồng đặt vé xem phim.</summary>
        public void BookTicket(DataStorage data, User currentUser)
        {
            Console.Clear();
            Console.WriteLine("--- ĐẶT VÉ XEM PHIM ---");
            ConsoleUI.DisplayShowtimesTable(data);

            Console.Write("\nNhập ID suất chiếu bạn muốn đặt vé: ");
            if (!int.TryParse(Console.ReadLine(), out int showId))
            {
                Console.WriteLine("ID suất chiếu không hợp lệ.");
                return;
            }

            int showIndex = data.Showtimes.FindIndex(s => s.Id == showId);
            if (showIndex == -1)
            {
                Console.WriteLine("Không tìm thấy suất chiếu có ID này.");
                return;
            }

            Showtime show = data.Showtimes[showIndex];

            // Tìm Movie dựa trên RoomNumber của Showtime.
            Movie movie = data.Movies.Find(m => m.Id == show.RoomNumber);

            if (movie == null)
            {
                Console.WriteLine("Lỗi: Không tìm thấy thông tin phim tương ứng với phòng chiếu này.");
                return;
            }

            ConsoleUI.DisplaySeatingChart(show);

            Console.Write("Nhập các ghế cần đặt, cách nhau bởi dấu phẩy (ví dụ: A1,B3,C5): ");
            var inputSeatsStr = Console.ReadLine();

            var selectedSeats = new List<(int r, int c)>();
            if (!string.IsNullOrWhiteSpace(inputSeatsStr))
            {
                var seatsStr = inputSeatsStr.ToUpper().Split(',');
                foreach (var seatStr in seatsStr.Select(s => s.Trim()).Where(s => s.Length >= 2))
                {
                    char rowChar = seatStr[0];
                    if (int.TryParse(seatStr.Substring(1), out int colNum) && rowChar >= 'A' && rowChar <= 'Z')
                    {
                        selectedSeats.Add((r: rowChar - 'A', c: colNum - 1));
                    }
                }
            }

            if (!selectedSeats.Any())
            {
                Console.WriteLine("Bạn chưa chọn ghế nào hoặc định dạng ghế không hợp lệ.");
                return;
            }

            decimal totalPrice = logic.CalculateTotalPrice(selectedSeats.Count, movie.TicketPrice);
            Console.WriteLine("\n--- XÁC NHẬN ĐẶT VÉ ---");
            Console.WriteLine($"Phim: {movie.Title}");
            Console.WriteLine($"Số lượng vé: {selectedSeats.Count}");
            Console.WriteLine($"Tổng tiền: {totalPrice:N0} VNĐ");
            Console.Write("Bạn có chắc chắn muốn đặt vé? (y/n): ");

            if (Console.ReadLine().Trim().ToLower() != "y")
            {
                Console.WriteLine("Đã hủy thao tác đặt vé.");
                return;
            }

            logic.BookSeats(ref show, selectedSeats, currentUser.Id, out Booking newBooking);

            data.Showtimes[showIndex] = show;

            if (newBooking.BookingId != 0)
            {
                data.Bookings.Add(newBooking);
                Console.WriteLine($"Đặt vé thành công! Mã vé của bạn là: {newBooking.BookingId}");
            }
        }

        /// <summary>4. Luồng hủy vé đã đặt.</summary>
        public void CancelBooking(DataStorage data, User currentUser)
        {
            Console.Clear();
            Console.WriteLine("--- HỦY VÉ ---");

            var userBookings = data.Bookings.Where(b => b.UserId == currentUser.Id).ToList();

            if (!userBookings.Any())
            {
                Console.WriteLine("Bạn chưa đặt vé nào.");
                return;
            }

            Console.WriteLine("Các vé bạn đã đặt:");
            foreach (var booking in userBookings)
            {
                var showtime = data.Showtimes.Find(s => s.Id == booking.ShowtimeId);
                // Tương tự, dùng RoomNumber để tìm phim
                var movie = showtime != null ? data.Movies.Find(m => m.Id == showtime.RoomNumber) : null;
                Console.WriteLine($"- Mã vé: {booking.BookingId}, Phim: {movie?.Title ?? "N/A"}, Suất chiếu: {showtime?.StartTime.ToString("g") ?? "N/A"}");
            }

            Console.Write("\nNhập mã vé bạn muốn hủy: ");
            if (!int.TryParse(Console.ReadLine(), out int bookingIdToCancel))
            {
                Console.WriteLine("Mã vé không hợp lệ.");
                return;
            }

            var bookingToCancel = userBookings.FirstOrDefault(b => b.BookingId == bookingIdToCancel);
            if (bookingToCancel.BookingId == 0)
            {
                Console.WriteLine("Không tìm thấy mã vé này trong danh sách vé của bạn.");
                return;
            }

            var showtimeOfBooking = data.Showtimes.FirstOrDefault(s => s.Id == bookingToCancel.ShowtimeId);
            if (showtimeOfBooking == null)
            {
                Console.WriteLine("Lỗi: Không tìm thấy suất chiếu của vé này, không thể hủy.");
                return;
            }

            bool success = logic.CancelBooking(data.Bookings, bookingIdToCancel, showtimeOfBooking);

            if (success)
            {
                Console.WriteLine($"Đã hủy thành công vé có mã {bookingIdToCancel}.");
            }
        }

        /// <summary>
        /// 5. Đăng xuất.
        /// </summary>
        public void Logout()
        {
            Console.WriteLine("\nĐang đăng xuất... Tạm biệt!");
        }
    }
}