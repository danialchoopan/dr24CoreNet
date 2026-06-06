namespace dr24CoreNet.WebUI.Helpers;

public static class Loc
{
    private static readonly Dictionary<string, Dictionary<string, string>> _strings = new()
    {
        ["fa"] = new()
        {
            ["SearchDoctor"] = "جستجوی پزشک",
            ["DoctorPanel"] = "پنل پزشک",
            ["AdminPanel"] = "پنل مدیریت",
            ["PlatformTitle"] = "dr24CoreNet پلتفرم نوبت‌دهی",
            ["Slogan"] = "انتخاب هوشمند پزشک، رزرو آنی نوبت",
            ["ThemeToggle"] = "تغییر تم (تاریک/روشن)",
            ["AdvancedSearch"] = "جستجوی پیشرفته پزشکان",
            ["NameOrSpec"] = "نام یا تخصص:",
            ["City"] = "شهر:",
            ["SearchBtn"] = "جستجو و فیلتر",
            ["MedicalCouncil"] = "نظام پزشکی",
            ["Reserved"] = "رزرو شده",
            ["AllCities"] = "همه شهرها",
            ["Tehran"] = "تهران",
            ["Isfahan"] = "اصفهان"
        },
        ["en"] = new()
        {
            ["SearchDoctor"] = "Search Doctor",
            ["DoctorPanel"] = "Doctor Panel",
            ["AdminPanel"] = "Admin Panel",
            ["PlatformTitle"] = "dr24CoreNet Appointment Platform",
            ["Slogan"] = "Smart Doctor Selection, Instant Booking",
            ["ThemeToggle"] = "Toggle Theme (Dark/Light)",
            ["AdvancedSearch"] = "Advanced Doctor Search",
            ["NameOrSpec"] = "Name or Specialization:",
            ["City"] = "City:",
            ["SearchBtn"] = "Search & Filter",
            ["MedicalCouncil"] = "Medical Council",
            ["Reserved"] = "Reserved",
            ["AllCities"] = "All Cities",
            ["Tehran"] = "Tehran",
            ["Isfahan"] = "Isfahan"
        }
    };

    public static string Get(string key, string lang)
    {
        lang = lang?.ToLower() == "en" ? "en" : "fa";
        if (_strings[lang].TryGetValue(key, out var val)) return val;
        return key;
    }
}
