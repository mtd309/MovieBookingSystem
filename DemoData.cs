// ===============================
// Mục đích: Chứa dữ liệu mẫu để demo (danh sách phim, suất chiếu ban đầu)
// ===============================

using System;

namespace MovieBookingSystem
{
    public static class DemoData
    {
        public static void Load(DataStorage data)
        {
            data.Movies.Add(new Movie { Id = 1, Title = "Mưa Đỏ", Genre = "Chiến tranh", TicketPrice = 90000 });
            data.Movies.Add(new Movie { Id = 2, Title = "Tử Chiến Trên Không", Genre = "Hành động", TicketPrice = 95000 });
            data.Movies.Add(new Movie { Id = 3, Title = "Mai", Genre = "Tâm lý", TicketPrice = 85000 });
            data.Movies.Add(new Movie { Id = 4, Title = "Bố Già", Genre = "Hài", TicketPrice = 80000 });
            data.Movies.Add(new Movie { Id = 5, Title = "Thám Tử Kiên", Genre = "Trinh thám", TicketPrice = 90000 });

            // Tạo 1 suất chiếu demo cho mỗi phim
            foreach (var movie in data.Movies)
            {
                data.Showtimes.Add(new Showtime(movie.Id, 5, 5)
                {
                    Id = movie.Id,
                    StartTime = DateTime.Now.AddHours(movie.Id)
                });
            }

            Console.WriteLine("[DemoData] Dữ liệu mẫu đã được tải!");
        }
    }
}
