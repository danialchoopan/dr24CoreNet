import asyncio
import os
import subprocess
import signal
import time
from playwright.async_api import async_playwright

async def capture():
    async with async_playwright() as p:
        browser = await p.chromium.launch()
        context = await browser.new_context(viewport={'width': 1280, 'height': 900})
        page = await context.new_page()

        # Kill any existing dotnet processes on the port
        os.system("fuser -k 5208/tcp >/dev/null 2>&1 || true")

        # Start the app
        print("Starting development server...")
        proc = subprocess.Popen(
            ["dotnet", "run", "--no-build"],
            cwd="dr24CoreNet/dr24CoreNet.WebUI",
            stdout=subprocess.PIPE,
            stderr=subprocess.PIPE,
            preexec_fn=os.setsid
        )

        # Wait for the server to be ready
        max_retries = 30
        connected = False
        base_url = "http://localhost:5208"

        for i in range(max_retries):
            try:
                # Try simple response check
                response = await page.goto(base_url)
                if response and response.status == 200:
                    print(f"Connected to server on attempt {i+1}")
                    connected = True
                    break
            except Exception as e:
                pass
            await asyncio.sleep(1)

        if not connected:
            print("Could not connect to server or server returned error.")
            os.killpg(os.getpgid(proc.pid), signal.SIGTERM)
            return

        pages = [
            ("/", "home"),
            ("/Booking/1", "booking"),
            ("/Admin/Analytics", "analytics"),
            ("/Admin/Audit", "audit"),
            ("/Patient/History", "history"),
            ("/Chat", "chat")
        ]

        langs = ["fa", "en"]

        if not os.path.exists("screenshots"):
            os.makedirs("screenshots")

        for lang in langs:
            for path, name in pages:
                url = f"{base_url}{path}?lang={lang}"
                print(f"Capturing {url}...")
                try:
                    await page.goto(url, wait_until="networkidle")
                    if name == "chat":
                         await page.evaluate("window.scrollTo(0, document.body.scrollHeight)")

                    await page.screenshot(path=f"screenshots/{name}_{lang}.png", full_page=True)
                except Exception as e:
                    print(f"Failed to capture {url}: {e}")

        await browser.close()
        os.killpg(os.getpgid(proc.pid), signal.SIGTERM)

if __name__ == "__main__":
    asyncio.run(capture())
