# مستندات فنی API اینترپرایز dr24CoreNet

این مستندات شامل لیست Endpointها و منطق پترن‌های پیشرفته پیاده‌سازی شده است.

## سیستم مدیریت همزمانی پیشرفته
در سیستم رزرو، علاوه بر استفاده از `RowVersion` برای جلوگیری از تداخل، از یک لایه `DistributedLockService` در Infrastructure استفاده شده است که تضمین می‌کند درخواست‌های رزرو برای یک اسلات خاص به صورت اتمیک پردازش شوند.

## لیست APIهای کلیدی

### ۱. سیستم ارجاع (Referral)
- **POST `/api/referrals/create`**: صدور توکن دیجیتال توسط پزشک.
- **GET `/api/referrals/validate/{token}`**: بررسی اعتبار و اعمال تخفیف/اولویت.

### ۲. لاگ‌های حسابرسی (Audit)
- **GET `/api/admin/audit-logs`**: مشاهده لیست تغییرات با جزئیات قبل و بعد (Before/After).

### ۳. تحلیل‌های بهینه (Snapshots)
- **GET `/api/analytics/revenue-snapshot`**: دریافت دیتا از کش اسنپ‌شات (بدون کوئری سنگین روی جداول اصلی).

### ۴. مشاوره آنلاین (SignalR)
- **Hub Endpoint**: `/chatHub`
- **Events**: `ReceiveMessage`, `JoinChat`

## پترن‌های استفاده شده
- **Adapter Pattern**: برای تبدیل کدهای قدیمی نظام پزشکی به ساختار مدرن.
- **Strategy Pattern**: برای محاسبه داینامیک کارمزد بر اساس تخصص.
- **Materialized View Simulation**: با استفاده از Background Worker برای تولید دوره‌ای آمارهای تحلیلی.
