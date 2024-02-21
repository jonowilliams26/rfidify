from requests import get
from PiicoDev_Unified import sleep_ms
from PiicoDev_SSD1306 import create_PiicoDev_SSD1306
from time import time

display = create_PiicoDev_SSD1306()
url = "http://api:8080/spotify/currently-playing"
ok = 200
not_found = 404

while True:
    display.fill(0)
    try:
        response = get(url)
        if response.status_code == ok:
            data = response.json()
            display.text(data['name'], 0, 0, 1)
            display.text(data['artists'], 0, 15, 1)
            display.text(data['progress'], 0, 30, 1)
            display.show()
        elif response.status_code == not_found:
            display.text("Nothing playing", 0, 0, 1)
            display.show()
        else:
            response.raise_for_status()
    except:
        display.text("Sorry, something went wrong", 0, 0, 1)
        display.show()

    sleep_ms(750)