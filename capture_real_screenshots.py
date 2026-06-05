from playwright.sync_api import sync_playwright
import time
import os

def capture_screenshots():
    os.makedirs("/home/jules/verification", exist_ok=True)
    with sync_playwright() as p:
        browser = p.chromium.launch(headless=True)
        page = browser.new_page(viewport={'width': 1280, 'height': 800})

        # Wait for server
        max_retries = 30
        for i in range(max_retries):
            try:
                page.goto("http://localhost:5001")
                break
            except:
                if i == max_retries - 1: return
                time.sleep(2)

        # 1. Search Doctors
        page.goto("http://localhost:5001")
        page.wait_for_timeout(2000)
        page.screenshot(path="dr24CoreNet/screenshots/search_doctors.png")

        # 2. Booking Countdown (Simulate by showing the div)
        page.evaluate("() => { document.getElementById('bookingTimer').classList.remove('hidden'); startBookingTimer(600); }")
        page.screenshot(path="dr24CoreNet/screenshots/booking_countdown.png")

        # 3. Medical Audit Trail
        page.goto("http://localhost:5001/Admin/Audit")
        page.wait_for_timeout(1000)
        page.screenshot(path="dr24CoreNet/screenshots/medical_audit_trail.png")

        # 4. Enterprise Analytics
        page.goto("http://localhost:5001/Admin/Analytics")
        page.wait_for_timeout(2000)
        page.screenshot(path="dr24CoreNet/screenshots/enterprise_analytics.png")

        # 5. Chat Online
        page.goto("http://localhost:5001/Chat/105")
        page.evaluate("() => appendMessage('دکتر', 'سلام، علائم شما از کی شروع شده؟', false)")
        page.screenshot(path="dr24CoreNet/screenshots/chat_online.png")

        # 6. Medical History
        page.goto("http://localhost:5001/Patient/History")
        page.wait_for_timeout(1000)
        page.screenshot(path="dr24CoreNet/screenshots/medical_history.png")

        browser.close()

if __name__ == "__main__":
    capture_screenshots()
