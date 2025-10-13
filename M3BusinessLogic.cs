// ========================================
// Module 3 – Business Logic
// Phụ trách: Nguyễn Hồng Hạnh 
// Mục đích: Xử lý toàn bộ nghiệp vụ chính của hệ thống: tìm kiếm, đặt vé, tính tiền, sắp xếp, hủy vé.
// Các method: IsSeatAvailable, CalculateTotalPrice, BookSeats, SearchMovieByName, CancelBooking, SortMoviesByTitle, BinarySearchMovieById
// Biến sử dụng: int (id, row, col), string (title, keyword), bool (kiểm tra ghế trống), decimal (tổng tiền)
// Mảng 2D: SeatStatus[,] (ghế của suất chiếu)
// Danh sách: List<Movie>, List<Showtime>, List<Booking>
// Vòng lặp: for, foreach
// Câu điều kiện: if/else
// ref/out: BookSeats dùng ref Showtime và out Booking
// ========================================
namespace MovieBookingSystem
{
    using System;
    using System.Collections.Generic;
    using MovieBookingSystem; // dùng các class/struct từ Module 1

    public class BusinessLogic
    {
        // 1. Kiểm tra ghế còn trống hay không
        public bool IsSeatAvailable(Showtime s, int row, int col)
        {
            if (row < 0 || row >= s.Seating.GetLength(0) ||
                col < 0 || col >= s.Seating.GetLength(1))
                return false; // ghế không hợp lệ
            return s.Seating[row, col] == SeatStatus.Available;
        }

        // 2. Tính tổng tiền (có default param cho discount)
        public decimal CalculateTotalPrice(int ticketCount, decimal pricePerTicket, decimal discount = 0)
        {
            decimal total = ticketCount * pricePerTicket;
            if (discount > 0)
                total -= total * discount;
            return total;
        }

        // 3. Đặt ghế (dùng ref và out)
        public void BookSeats(ref Showtime s, List<(int r, int c)> seats, int userId, out Booking createdBooking)
        {
            // Kiểm tra tất cả ghế có hợp lệ và còn trống không
            foreach (var (r, c) in seats)
            {
                if (!IsSeatAvailable(s, r, c))
                {
                    createdBooking = new Booking(); // Trả về booking rỗng nếu có lỗi
                    Console.WriteLine("Lỗi: Một trong các ghế đã được đặt hoặc không hợp lệ.");
                    return;
                }
            }

            // Tạo booking mới
            createdBooking = new Booking(new Random().Next(1000, 9999), s.Id, userId);

            // Đặt từng ghế
            foreach (var (r, c) in seats)
            {
                s.Seating[r, c] = SeatStatus.Booked;
                createdBooking.Seats.Add((r, c));
            }
            Console.WriteLine("Đặt vé thành công!");
        }

        // 4. Tìm phim theo tên (Linear Search - không phân biệt hoa thường)
        public List<Movie> SearchMovieByName(List<Movie> movies, string keyword)
        {
            var result = new List<Movie>();
            foreach (var m in movies)
            {
                if (m.Title.ToLower().Contains(keyword.ToLower()))
                    result.Add(m);
            }
            return result;
        }

        // 5. Overload: Tìm phim có phân biệt hoa thường
        public List<Movie> SearchMovieByName(List<Movie> movies, string keyword, bool caseSensitive)
        {
            var result = new List<Movie>();
            foreach (var m in movies)
            {
                if (m.Title == null) continue;

                if (caseSensitive)
                {
                    if (m.Title.Contains(keyword))
                        result.Add(m);
                }
                else
                {
                    if (m.Title.ToLower().Contains(keyword.ToLower()))
                        result.Add(m);
                }
            }
            return result;
        }

        // 6. Hủy vé (void)
        public bool CancelBooking(List<Booking> bookings, int bookingId, Showtime showtime)
        {
            for (int i = 0; i < bookings.Count; i++)
            {
                if (bookings[i].BookingId == bookingId)
                {
                    // Trả lại ghế
                    foreach (var (r, c) in bookings[i].Seats)
                    {
                        if (r >= 0 && r < showtime.Seating.GetLength(0) &&
                            c >= 0 && c < showtime.Seating.GetLength(1))
                        {
                            showtime.Seating[r, c] = SeatStatus.Available;
                        }
                    }

                    bookings.RemoveAt(i);
                    Console.WriteLine("Hủy vé thành công!");
                    return true;
                }
            }
            Console.WriteLine("Không tìm thấy mã vé cần hủy.");
            return false;
        }

        // 7. Bubble Sort theo tên phim
        public void SortMoviesByTitle(List<Movie> movies)
        {
            for (int i = 0; i < movies.Count - 1; i++)
            {
                for (int j = 0; j < movies.Count - i - 1; j++)
                {
                    if (string.Compare(movies[j].Title, movies[j + 1].Title, StringComparison.Ordinal) > 0)
                    {
                        // đổi chỗ
                        var temp = movies[j];
                        movies[j] = movies[j + 1];
                        movies[j + 1] = temp;
                    }
                }
            }
        }

        // 8. Binary Search theo ID (danh sách phải sắp xếp sẵn theo ID)
        public int BinarySearchMovieById(List<Movie> moviesSortedById, int id)
        {
            int left = 0, right = moviesSortedById.Count - 1;
            while (left <= right)
            {
                int mid = (left + right) / 2;
                if (moviesSortedById[mid].Id == id) return mid;
                else if (moviesSortedById[mid].Id < id) left = mid + 1;
                else right = mid - 1;
            }
            return -1; // không tìm thấy
        }
    }
}