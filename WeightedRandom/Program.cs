// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography;
using WeightedRandom;

// Hàm sinh số ngẫu nhiên an toàn với RandomNumberGenerator
static int GetRandomInt(int minValue, int maxValue)
{
    if (minValue >= maxValue)
    {
        throw new ArgumentOutOfRangeException(nameof(minValue), "minValue phải nhỏ hơn maxValue");
    }

    using (var rng = RandomNumberGenerator.Create())
    {
        byte[] randomBytes = new byte[4]; // Dùng 4 byte (32-bit) cho số nguyên
        rng.GetBytes(randomBytes);
        uint randomUInt32 = BitConverter.ToUInt32(randomBytes, 0);

        // Chuẩn hóa về khoảng [minValue, maxValue)
        return (int)(minValue + (randomUInt32 % (maxValue - minValue)));
    }
}

static GameItem? GetRandomItemByWeight(List<GameItem> gameItems)
{
    // Tính tổng trọng số
    var totalWeight = gameItems.Sum(s => s.Percentage);

    // Lấy ra một số ngẫu nhiên dùng để lựa chọn ngẫu nhiên một item trong list gameItems
    var randomNumber = GetRandomInt(0, totalWeight);

    // Duyệt qua mảng gameItems để lấy ra một item theo randomNumber
    int currentWeight = 0;
    foreach (var item in gameItems)
    {
        currentWeight += item.Percentage;
        if (randomNumber <= currentWeight)
        {
            return item;
        }
    }

    return null;
}

var gameItems = new List<GameItem>
{
    new GameItem { Name = "Sword", Percentage = 10 },
    new GameItem { Name = "Shield", Percentage = 20 },
    new GameItem { Name = "Armor", Percentage = 30 },
    new GameItem { Name = "Potion", Percentage = 25 },
    new GameItem { Name = "Ring", Percentage = 15 }
};

// Test thử lấy ngẫu nhiên 10 lần
for (var i = 0; i < 20; i++)
{
    var gameItem = GetRandomItemByWeight(gameItems);
    if (gameItem == null)
    {
        Console.WriteLine($"Error no item found");
        continue;
    }

    Console.WriteLine($"Random Item Name {gameItem.Name} Percentage {gameItem.Percentage}");
}

Console.ReadLine();