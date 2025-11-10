// ===========================================================
// Module 2 - File Management
// Phụ trách: Hoàng Thị Mỹ Dung
// Mục đích: Quản lý việc đọc và ghi dữ liệu của hệ thống ra file (CSV).
// Các struct/class: FileManager (tĩnh), sử dụng lại Movie, Showtime, Booking, User
// Các hàm: LoadMoviesFromFile, SaveMoviesToFile, LoadShowtimesFromFile, SaveShowtimesToFile, LoadBookingsFromFile, SaveBookingsToFile, LoadUsersFromFile, SaveUsersToFile, Hàm private: EnsureFileBackup(tạo file sao lưu)
// Cac enum: UserRole (dùng lại từ Module 1)
// ===========================================================
using System;
using System.Collections.Generic;
using System.IO;

namespace MovieBookingSystem
{
    // Class tĩnh quản lý đọc/ghi file dữ liệu hệ thống
    public static class FileManager
    {
        // Đường dẫn các file dữ liệu
        private static string moviesFile = "movies.txt";
        private static string showtimesFile = "showtimes.txt";
        private static string bookingsFile = "bookings.txt";
        private static string usersFile = "users.txt";

        // ===================== MOVIE =====================
        public static List<Movie> LoadMoviesFromFile()
        {
            var movies = new List<Movie>();

            try
            {
                if (!File.Exists(moviesFile))
                    return movies;

                foreach (var line in File.ReadAllLines(moviesFile))
                {
                    var parts = line.Split(',');

                    if (parts.Length >= 4)
                    {
                        movies.Add(new Movie
                        {
                            Id = int.Parse(parts[0]),
                            Title = parts[1],
                            TicketPrice = decimal.Parse(parts[2]),
                            Genre = parts[3]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Error] Cannot load movies: " + ex.Message);
            }

            return movies;
        }

        public static void SaveMoviesToFile(List<Movie> movies)
        {
            EnsureFileBackup(moviesFile);

            try
            {
                using (var writer = new StreamWriter(moviesFile))
                {
                    foreach (var m in movies)
                    {
                        writer.WriteLine($"{m.Id},{m.Title},{m.TicketPrice},{m.Genre}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Error] Cannot save movies: " + ex.Message);
            }
        }

        // ===================== SHOWTIME =====================
        // Ghi nhận kích thước ghế (rows, cols) để module khác khởi tạo lại
        public static List<Showtime> LoadShowtimesFromFile()
        {
            var showtimes = new List<Showtime>();

            try
            {
                if (!File.Exists(showtimesFile)) return showtimes;

                foreach (var line in File.ReadAllLines(showtimesFile))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var parts = line.Split(',');

                    if (parts.Length >= 5 &&
                        int.TryParse(parts[0], out int id) &&
                        int.TryParse(parts[1], out int room) &&
                        DateTime.TryParse(parts[2], out DateTime startTime) &&
                        int.TryParse(parts[3], out int rows) &&
                        int.TryParse(parts[4], out int cols))
                    {

                        var st = new Showtime(room, rows, cols)
                        {
                            Id = id,
                            StartTime = startTime
                        };

                        showtimes.Add(st);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Error] Cannot load showtimes: " + ex.Message);
            }

            return showtimes;
        }

        public static void SaveShowtimesToFile(List<Showtime> showtimes)
        {
            EnsureFileBackup(showtimesFile);

            try
            {
                using (var writer = new StreamWriter(showtimesFile))
                {
                    foreach (var st in showtimes)
                    {
                        writer.WriteLine($"{st.Id},{st.RoomNumber},{st.StartTime},{st.Seating.GetLength(0)},{st.Seating.GetLength(1)}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Error] Cannot save showtimes: " + ex.Message);
            }
        }

        // ===================== BOOKING =====================
        // Ghế được lưu dạng "row-col;row-col"
        public static List<Booking> LoadBookingsFromFile()
        {
            var bookings = new List<Booking>();

            try
            {
                if (!File.Exists(bookingsFile)) return bookings;

                foreach (var line in File.ReadAllLines(bookingsFile))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var parts = line.Split(',');

                    if (parts.Length >= 4 &&
                        int.TryParse(parts[0], out int bid) &&
                        int.TryParse(parts[1], out int sid) &&
                        int.TryParse(parts[2], out int uid))
                    {
                        var booking = new Booking(bid, sid, uid);

                        var seatsStr = parts[3].Split(';');
                        foreach (var s in seatsStr)
                        {
                            if (string.IsNullOrWhiteSpace(s)) continue;
                            var xy = s.Split('-');
                            if (xy.Length == 2 &&
                                int.TryParse(xy[0], out int r) &&
                                int.TryParse(xy[1], out int c))
                                booking.Seats.Add((r, c));
                        }

                        bookings.Add(booking);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Error] Cannot load bookings: " + ex.Message);
            }

            return bookings;
        }

        public static void SaveBookingsToFile(List<Booking> bookings)
        {
            EnsureFileBackup(bookingsFile);

            try
            {
                using (var writer = new StreamWriter(bookingsFile))
                {
                    foreach (var b in bookings)
                    {
                        string seats = string.Join(";", b.Seats.ConvertAll(s => $"{s.SeatRow}-{s.SeatCol}"));
                        writer.WriteLine($"{b.BookingId},{b.ShowtimeId},{b.UserId},{seats}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Error] Cannot save bookings: " + ex.Message);
            }
        }

        // ===================== USER =====================
        public static List<User> LoadUsersFromFile()
        {
            var users = new List<User>();

            try
            {
                if (!File.Exists(usersFile)) return users;

                foreach (var line in File.ReadAllLines(usersFile))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var parts = line.Split(',');

                    if (parts.Length >= 3 &&
                        int.TryParse(parts[0], out int id) &&
                        Enum.TryParse(parts[2], out UserRole role))
                    {
                        users.Add(new User
                        {
                            Id = id,
                            Name = parts[1],
                            Role = role
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Error] Cannot load users: " + ex.Message);
            }

            return users;
        }

        public static void SaveUsersToFile(List<User> users)
        {
            EnsureFileBackup(usersFile);

            try
            {
                using (var writer = new StreamWriter(usersFile))
                {
                    foreach (var u in users)
                    {
                        writer.WriteLine($"{u.Id},{u.Name},{u.Role}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Error] Cannot save users: " + ex.Message);
            }
        }

        // ===================== BACKUP =====================
        private static void EnsureFileBackup(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string backupPath = filePath + ".bak";
                    File.Copy(filePath, backupPath, true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Warning] Cannot create backup for " + filePath + ": " + ex.Message);
            }
        }

        // ===================== TÍCH HỢP TỔNG =====================
        // Dùng cho M7 – Main Program
        public static void LoadAll(DataStorage storage)
        {
            storage.Movies = LoadMoviesFromFile();
            storage.Showtimes = LoadShowtimesFromFile();
            storage.Bookings = LoadBookingsFromFile();
            storage.Users = LoadUsersFromFile();
        }

        public static void SaveAll(DataStorage storage)
        {
            SaveMoviesToFile(storage.Movies);
            SaveShowtimesToFile(storage.Showtimes);
            SaveBookingsToFile(storage.Bookings);
            SaveUsersToFile(storage.Users);
        }
    }
}
