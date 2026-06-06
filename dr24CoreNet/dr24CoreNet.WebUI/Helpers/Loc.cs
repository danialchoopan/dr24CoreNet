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
            ["Isfahan"] = "اصفهان",
            ["BookingTitle"] = "رزرو نوبت",
            ["TimeRemaining"] = "زمان باقی‌مانده برای تکمیل پرداخت:",
            ["SelectVisitTime"] = "انتخاب ساعت ویزیت",
            ["BookingSummary"] = "خلاصه درخواست نوبت",
            ["VisitDate"] = "تاریخ ویزیت",
            ["VisitTime"] = "ساعت ویزیت",
            ["VisitFee"] = "هزینه ویزیت",
            ["TotalPayable"] = "مبلغ قابل پرداخت",
            ["ConfirmAndPay"] = "تایید نهایی و پرداخت",
            ["LockNotice"] = "با کلیک بر روی تایید، اسلات انتخابی به مدت ۱۰ دقیقه برای شما قفل (Lock) می‌شود.",
            ["Currency"] = "ریال",
            ["Saturday"] = "شنبه",
            ["October25"] = "۲۵ مهر ۱۴۰۲",
            ["AM"] = "صبح",
            ["DrName"] = "دکتر امیرعلی علوی",
            ["DrSpec"] = "متخصص قلب و عروق",
            ["DrAddress"] = "تهران، خیابان ولیعصر، ساختمان پزشکان دی",
            ["AnalyticsTitle"] = "داشبورد مدیریت مالی و تحلیل پلتفرم",
            ["RevenueOverview"] = "مرور درآمد و تراکنش‌ها",
            ["TotalVolume"] = "حجم کل معاملات",
            ["PlatformCommission"] = "کارمزد پلتفرم (۱۵٪)",
            ["ActiveAppointments"] = "نوبت‌های فعال",
            ["GrowthRate"] = "نرخ رشد ماهانه",
            ["RecentTransactions"] = "تراکنش‌های اخیر سیستم",
            ["AuditTitle"] = "ردیابی امنیتی عملیات (Audit Trail)",
            ["PatientHistory"] = "پرونده سلامت و سوابق پزشکی",
            ["ChatTitle"] = "مشاوره آنلاین و گفتگو با پزشک"
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
            ["Isfahan"] = "Isfahan",
            ["BookingTitle"] = "Book Appointment",
            ["TimeRemaining"] = "Time remaining to complete payment:",
            ["SelectVisitTime"] = "Select Visit Time",
            ["BookingSummary"] = "Appointment Summary",
            ["VisitDate"] = "Visit Date",
            ["VisitTime"] = "Visit Time",
            ["VisitFee"] = "Visit Fee",
            ["TotalPayable"] = "Total Payable",
            ["ConfirmAndPay"] = "Confirm and Pay",
            ["LockNotice"] = "By clicking confirm, the selected slot will be locked for you for 10 minutes.",
            ["Currency"] = "Rials",
            ["Saturday"] = "Saturday",
            ["October25"] = "October 17, 2023",
            ["AM"] = "AM",
            ["DrName"] = "Dr. Amir Ali Alavi",
            ["DrSpec"] = "Cardiologist",
            ["DrAddress"] = "Tehran, Valiasr St, Dey Medical Center",
            ["AnalyticsTitle"] = "Financial Management & Analytics Dashboard",
            ["RevenueOverview"] = "Revenue and Transaction Overview",
            ["TotalVolume"] = "Total Transaction Volume",
            ["PlatformCommission"] = "Platform Commission (15%)",
            ["ActiveAppointments"] = "Active Appointments",
            ["GrowthRate"] = "Monthly Growth Rate",
            ["RecentTransactions"] = "Recent System Transactions",
            ["AuditTitle"] = "Security Audit Trail",
            ["PatientHistory"] = "Health Records & Medical History",
            ["ChatTitle"] = "Online Consultation & Chat"
        }
    };

    public static string Get(string key, string lang)
    {
        lang = lang?.ToLower() == "en" ? "en" : "fa";
        if (_strings[lang].TryGetValue(key, out var val)) return val;
        return key;
    }
}
