using Kasir.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Kasir.DbContexts
{
    public class DbSeeder
    {
        public static async Task StartSeederAsync(iCassierDbContext dbContext)
        {
            await dbContext.Database.EnsureCreatedAsync();

            await SeedUsers(dbContext, 20);
            await SeedCategory(dbContext, 50);
            await SeedRole(dbContext);
            await SeedProduct(dbContext, 20);
            await SeedReceiveItems(dbContext);
            await SeedCustomers(dbContext, 150);
            await SeedTransaction(dbContext);
            await SeedTransactionItem(dbContext);
        }

        public static async Task SeedUsers(iCassierDbContext dbContext, int total = 10)
        {
            string[] firstNames = { "John", "Alice", "Michael", "Emma", "David", "Olivia", "William", "Sophia", "James", "Isabella", "Daniel", "Lily", "Joseph", "Ava", "Benjamin", "Grace", "Samuel", "Charlotte", "Henry", "Ella" };
            string[] lastNames = { "Smith", "Johnson", "Brown", "Lee", "Davis", "Wilson", "Evans", "Garcia", "Martin", "Anderson", "Harris", "Wright", "Perez", "Jackson", "Roberts", "Taylor", "Lewis", "Hill", "Young", "Turner" };

            Random random = new Random();

            for (int i = 0; i < total; i++)
            {
                string firstName = firstNames[random.Next(firstNames.Length)];
                string lastName = lastNames[random.Next(lastNames.Length)];
                string fullName = $"{firstName} {lastName}";
                User user = new User
                {
                    UserName = firstName.ToLower(),
                    AvatarUrl = null,
                    CreatedAt = DateTime.Now,
                    Email = $"{firstName.ToLower()}.{lastName.ToLower()}@gmail.com",
                    Name = fullName,
                    Password = BCrypt.Net.BCrypt.HashPassword("password"),
                    Role = random.Next(0,2),
                    LastUpdatedAt = DateTime.Now,
                };
                await dbContext.Users.AddAsync(user);
            }
            await dbContext.SaveChangesAsync();
        }

        public static async Task SeedCategory(iCassierDbContext dbContext, int total = 10)
        {
            string[] daftarKategoriBarang = new string[]
            {
                "Elektronik",
                "Pakaian",
                "Makanan",
                "Minuman",
                "Kesehatan dan Kecantikan",
                "Otomotif",
                "Alat Rumah Tangga",
                "Mainan",
                "Perhiasan",
                "Perabotan",
                "Olahraga",
                "Alat Musik",
                "Buku",
                "Film dan Musik",
                "Peralatan Taman",
                "Komputer dan Aksesoris",
                "Perlengkapan Bayi",
                "Kamera dan Fotografi",
                "Barang Koleksi",
                "Alat Tulis",
                "Alat Dapur",
                "Sepatu",
                "Tas",
                "Perjalanan",
                "Hobi dan Kerajinan",
                "Elektronik Rumah Tangga",
                "Alat Pancing",
                "Alat Kesehatan",
                "Alat Olahraga",
                "Barang Antik",
                "Alat Pemotong",
                "Perlengkapan Camping",
                "Alat Pertukangan",
                "Alat Perkakas",
                "Alat Listrik",
                "Alat Bantu Belajar",
                "Boneka",
                "Alat Bantu Masak",
                "Bahan Makanan",
                "Pakaian Anak",
                "Pakaian Bayi",
                "Peralatan Kantor",
                "Peralatan Sekolah",
                "Peralatan Seni",
                "Perlengkapan Pesta",
                "Peralatan Kebersihan",
                "Peralatan Mandi",
                "Peralatan Tidur",
                "Peralatan Makan",
                "Peralatan Minum",
                "Peralatan Masak",
                "Peralatan P3K",
                "Peralatan Kecantikan",
                "Peralatan Perawatan Rambut",
                "Peralatan Perawatan Kulit",
                "Alat Olahraga Outdoor",
                "Alat Olahraga Indoor",
                "Alat Bermain Anak",
                "Perlengkapan Bayi Perempuan",
                "Perlengkapan Bayi Laki-laki",
                "Perlengkapan Ibu Hamil",
                "Perlengkapan Makan Bayi",
                "Perlengkapan Tidur Bayi",
                "Perlengkapan Mandi Bayi",
                "Perlengkapan Mainan Bayi",
                "Perlengkapan Perawatan Bayi",
                "Perlengkapan Kesehatan Bayi",
                "Perlengkapan Keamanan Bayi",
                "Perlengkapan Perjalanan Bayi",
                "Perlengkapan Perlindungan Bayi",
                "Kacamata",
                "Perhiasan Emas",
                "Perhiasan Perak",
                "Perhiasan Berlian",
                "Perhiasan Batu Mulia",
                "Perhiasan Logam Mulia",
                "Perhiasan Imitasi",
                "Furniture Ruang Tamu",
                "Furniture Kamar Tidur",
                "Furniture Dapur",
                "Furniture Ruang Makan",
                "Furniture Kantor",
                "Furniture Outdoor",
                "Meja",
                "Kursi",
                "Lemari",
                "Rak Buku",
                "Sofa",
                "Kasur",
                "Komputer Desktop",
                "Laptop",
                "Tablet",
                "Printer",
                "Smartphone",
                "Konsol Game",
                "Keyboard",
                "Mouse",
                "Monitor",
                "Speaker",
                "Headset",
                "Kamera DSLR",
                "Kamera Mirrorless",
                "Kamera Pocket",
                "Kamera Video",
                "Lensa Kamera",
                "Tripod",
                "Tas Kamera",
                "Film DVD",
                "Musik CD",
                "Vinyl Record",
                "Alat Musik Tradisional",
                "Not Balok",
                "Partitur Musik",
                "Buku Fiksi",
                "Buku Non-Fiksi",
                "Buku Anak-anak",
                "Buku Pendidikan",
                "Buku Agama",
                "Buku Sejarah",
                "Buku Kuliner",
                "Buku Hobi",
                "Buku Kesehatan",
                "Buku Self-Help",
                "Buku Biografi",
                "Buku Novel",
                "Buku Komik",
                "Buku Puisi",
                "Buku Travel",
                "Buku Bisnis",
                "Buku Seni",
                "Peralatan Taman Kecil",
                "Alat Pemanggang",
                "Alat Penyeduh Kopi",
                "Alat Pembuat Roti",
                "Alat Pengolah Makanan",
                "Alat Pelangsing",
                "Suplemen Kesehatan",
                "Obat-obatan",
                "Alat Medis",
                "Perlengkapan Perawatan Kulit",
                "Perlengkapan Rambut",
                "Perlengkapan Mandi dan Tubuh",
                "Perlengkapan Kebersihan Gigi",
                "Perlengkapan Kebersihan Wajah",
                "Perlengkapan Kebersihan Tubuh",
                "Perlengkapan Kebersihan Tangan",
                "Perlengkapan Olahraga Air",
                "Perlengkapan Olahraga Lapangan",
                "Perlengkapan Olahraga Indoor",
                "Perlengkapan Olahraga Outdoor",
                "Perlengkapan Olahraga Fitness",
                "Perlengkapan Olahraga Yoga",
                "Perlengkapan Olahraga Pilates",
                "Perlengkapan Olahraga Jalan",
                "Perlengkapan Olahraga Bersepeda",
                "Perlengkapan Olahraga Renang",
                "Perlengkapan Olahraga Mendaki",
                "Perlengkapan Olahraga Berkuda",
                "Perlengkapan Olahraga Panjat Tebing",
                "Perlengkapan Olahraga Bulutangkis",
                "Perlengkapan Olahraga Tenis",
                "Perlengkapan Olahraga Sepak Bola",
                "Perlengkapan Olahraga Basket",
                "Perlengkapan Olahraga Golf",
                "Perlengkapan Olahraga Baseball",
                "Perlengkapan Olahraga Volleyball",
                "Perlengkapan Olahraga Ski",
                "Perlengkapan Olahraga Snowboard",
                "Perlengkapan Olahraga Selam",
            };
            for (int i = 0; i < total; i++)
            {
                await dbContext.Categories.AddAsync(new Category()
                {
                    Name = daftarKategoriBarang[i]
                });
            }
            await dbContext.SaveChangesAsync();
        }

        public static async Task SeedRole(iCassierDbContext dbContext)
        {
            await dbContext.Roles.AddRangeAsync(new Role()
            {
                Permission = 128,
                RoleName = "Admin"
            },
            new Role()
            {
                Permission = 64,
                RoleName = "Staff"
            });
            await dbContext.SaveChangesAsync();
        }

        public static async Task SeedProduct(iCassierDbContext dbContext, int total = 10)
        {
            string[] daftarNamaBarang = new string[]
            {
                "Laptop",
                "Smartphone",
                "Kamera DSLR",
                "Televisi",
                "Kulkas",
                "Mesin Cuci",
                "Pakaian Wanita",
                "Pakaian Pria",
                "Sepatu Sport",
                "Jam Tangan",
                "Perhiasan Emas",
                "Perhiasan Perak",
                "Kacamata Hitam",
                "Tas Kulit",
                "Botol Minum Stainless",
                "Alat Masak Set",
                "Meja Kayu",
                "Kursi Ergonomis",
                "Sofa Ruang Tamu",
                "Kasur Queen Size",
                "Komputer Desktop",
                "Printer Laser",
                "Mouse Wireless",
                "Keyboard Mekanik",
                "Konsol Game PS5",
                "Konsol Game Xbox Series X",
                "Monitor Gaming",
                "Speaker Bluetooth",
                "Headset Gaming",
                "Kamera Mirrorless",
                "Kamera Pocket",
                "Lensa Kamera Wide",
                "Tripod Aluminium",
                "Kaos Polos",
                "Jeans Denim",
                "Sneaker Kanvas",
                "Kemeja Flanel",
                "Topi Snapback",
                "Kalung Berlian",
                "Cincin Permata",
                "Earphone Wireless",
                "Ransel Backpack",
                "Botol Parfum",
                "Gelang Kulit",
                "Celana Jogger",
                "Dompet Kulit",
                "Mug Keramik",
                "Piring Mangkuk",
                "Gelas Minum",
            };

            List<Category> categories = await dbContext.Categories.ToListAsync();
            Random random = new Random();

            for (int i = 0; i<total; i++)
            {
                Product produk = new Product()
                {
                    Barcode = GenerateRandomBarcode(10),
                    CategoryID = categories[random.Next(categories.Count())].Id,
                    Name = daftarNamaBarang[i],
                    Price = random.Next(1,20)*1000,
                    PromoPrice = null,
                    Notes = null
                };
                await dbContext.Products.AddAsync(produk);
            }
            await dbContext.SaveChangesAsync();
        }

        public static async Task SeedReceiveItems(iCassierDbContext dbContext, int total = 10)
        {
            List<Product> produk = await dbContext.Products.ToListAsync();
            Random random = new Random();

            for (int i = 0; i < total; i++)
            {
                Product selectedProduct = produk[random.Next(produk.Count())];
                ReceiveItems receiveItems = new ReceiveItems()
                {
                    ProductID = selectedProduct.Id,
                    PurchasePrice = selectedProduct.Price -1000,
                    Quantity = random.Next(10,30)
                };
                await dbContext.ReceiveItems.AddAsync(receiveItems);
            }
            await dbContext.SaveChangesAsync();
        }

        public static async Task SeedCustomers(iCassierDbContext dbContext, int total = 10)
        {
            string[] firstNames = { "John", "Alice", "Michael", "Emma", "David", "Olivia", "William", "Sophia", "James", "Isabella", "Daniel", "Lily", "Joseph", "Ava", "Benjamin", "Grace", "Samuel", "Charlotte", "Henry", "Ella" };
            string[] lastNames = { "Smith", "Johnson", "Brown", "Lee", "Davis", "Wilson", "Evans", "Garcia", "Martin", "Anderson", "Harris", "Wright", "Perez", "Jackson", "Roberts", "Taylor", "Lewis", "Hill", "Young", "Turner" };

            string[] cities = { "Bekasi","Karawang","Cikarang","Bandung","Palembang","Banjarmasin" };

            Random random = new Random();

            for (int i = 0; i < total; i++)
            {
                string firstName = firstNames[random.Next(firstNames.Length)];
                string lastName = lastNames[random.Next(lastNames.Length)];
                string fullName = $"{firstName} {lastName}";
                Customer customer = new Customer
                {
                    PhoneNumber = "08"+ GenerateRandomBarcode(10).ToString(),
                    Email = $"{firstName.ToLower()}.{lastName.ToLower()}@gmail.com",
                    Name = fullName,
                    Country = "Indonesia",
                    City = cities[random.Next(cities.Count())],
                    Phone = null,
                    Fax = null,
                    Address = null
                };
                await dbContext.Customers.AddAsync(customer);
            }
            await dbContext.SaveChangesAsync();
        }

        public static async Task SeedTransaction(iCassierDbContext dbContext, int total = 5)
        {
            Random random = new Random();
            List<Customer> customers = await dbContext.Customers.ToListAsync();
            for (int i = 0; i < total; i++) {
                Customer customer = customers[random.Next(customers.Count())];
                Transaction transaction = new Transaction()
                {
                    CusomerID = random.Next(0,2) == 0 ? null : customer.Id,
                    DateTransaction = DateTime.UtcNow,
                    TotalPrice = random.Next(1,100)*1000,
                    TotalPayment = 100_000
                };
                await dbContext.Transactions.AddAsync(transaction); 
            }
            await dbContext.SaveChangesAsync();
        }

        public static async Task SeedTransactionItem(iCassierDbContext dbContext, int total = 10)
        {
            Random random = new Random();
            List<Transaction> transactions = await dbContext.Transactions.ToListAsync();
            List<Product> produk = await dbContext.Products.ToListAsync();
            for (int i = 0; i < total; i++)
            {
                Transaction transaction = transactions[random.Next(transactions.Count())];
                Product currentProduct = produk[random.Next(produk.Count())];
                TransactionItem transactionItem = new TransactionItem()
                {
                    ProductID = currentProduct.Id,
                    ItemPrice = currentProduct.Price,
                    Qty = random.Next(1,5),
                    TransactionID = transaction.Id,
                };
                await dbContext.TransactionItems.AddAsync(transactionItem);
            }
            await dbContext.SaveChangesAsync();
        }

        private static string GenerateRandomBarcode(int length)
        {
            Random random = new Random();
            string characters = "0123456789"; 
            char[] barcode = new char[length];

            for (int i = 0; i < length; i++)
            {
                barcode[i] = characters[random.Next(0, characters.Length)];
            }

            return new string(barcode);
        }
    }
}
